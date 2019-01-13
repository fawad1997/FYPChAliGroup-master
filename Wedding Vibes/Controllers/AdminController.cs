using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using WeddingVibes.Data;
using WeddingVibes.Models;
using WeddingVibes.Models.AdminVM;

namespace WeddingVibes.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           
          var  reservationsList = _context.Reservation.Where(r => r.ReservationDate <= DateTime.Now && r.ReservationDate >= DateTime.Now.AddMonths(-5)).ToList();

            var monthList = new List<string>();
            var monthCountList = new List<int>();
            for (int i = 0; i <= 5; i++)
            {
                var dateTime = DateTime.Now.AddMonths(-i).ToString("MMM yyyy", CultureInfo.InvariantCulture);
                monthList.Add(dateTime);
                monthCountList.Add(reservationsList.Count(r => r.ReservationDate.ToString("MMM yyyy", CultureInfo.InvariantCulture) == dateTime)); 
            }
            monthList.Reverse();
            monthCountList.Reverse();
            var adminVM = new AdminVM();
            if (monthCountList.Count > 0 && monthList.Count >0)
            {
                 adminVM = new AdminVM();
                adminVM.Reservations = reservationsList;
                var chart = new ChartVM {LastSixMonths = monthList, ReservationCounts = monthCountList};
                adminVM.ChartVm = chart;
            }
            return View(adminVM);
        }

        public async Task<JsonResult> Notifications()
        {
            var list =await _context.Notifications.Where(n=>n.MarkAsRead ==false).ToListAsync();
            var ns = new NotificationVM
            {
                Count = list.Count,
                Notifications = list
            };
            return Json(ns);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsReadNotification(int? id, int[] readAll)
        {
           
             
            if (id !=null && readAll.Length ==0)
            {
                var note = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
                note.MarkAsRead = true;
                _context.Update(note);
                await   _context.SaveChangesAsync();
                return Json($"MarkAsRead success of msg id: {id}");
            } 
            else if (id == null && readAll.Length > 0)
            {
                var list = await _context.Notifications.Where(n => n.MarkAsRead == false).Where(t => readAll.Contains(t.Id)).ToListAsync();
                var  note = list.Select(c => { c.MarkAsRead = true; return c; }).ToList();
                _context.Notifications.UpdateRange(note);
                await _context.SaveChangesAsync();
                return Json($"MarkAsRead success of msg ids: {readAll.ToArray()}");

            }

            return BadRequest("MarkAsRead failed");
        }


    }
}
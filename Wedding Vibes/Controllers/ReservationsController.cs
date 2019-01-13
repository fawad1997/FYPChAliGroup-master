﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingVibes.Data;
using WeddingVibes.Extensions;
using WeddingVibes.Models;
using WeddingVibes.Models.Reservation;
using WeddingVibes.Services;

namespace WeddingVibes.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ReservationsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        // GET: Reservations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetCurrentUser(HttpContext);
          
            
            var applicationDbContext = _context.Reservation.Where(x=>x.UserID == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.user)
                .SingleOrDefaultAsync(m => m.id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetCurrentUser(HttpContext);
            DateTime today = DateTime.Now;
            var upcomingReservedDates = _context.Reservation.Where(X=>X.ReservationDate>today).Select(y=>y.ReservationDate).ToList();
            ViewBag.ReservedDates = upcomingReservedDates;
            var m = _context.Menu.Where(x=>x.UserId == null || x.UserId == user.Id).ToList();
            ViewBag.Menu = new SelectList(m, "Id", "MenuName");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,FirstName,LastName,UserID,Address,HallName,PhoneNo,NumberofGuests,ReservationDate,Title,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetCurrentUser(HttpContext);
                reservation.UserID = user.Id;
                //if(reservation.HallName=="Hall 1")
                //{
                //    if(reservation.NumberofGuests>300)
                //        ModelState.AddModelError()
                //}
                //if (reservation.HallName == "Hall 2")
                //{

                //}
                //if (reservation.HallName == "Hall 3")
                //{

                //}
                await _emailSender.SendEmailReservationAsync(user.Email, new Message{Title = "New Reservation from ", ReservationDate = reservation.ReservationDate, ReserverName = reservation.FirstName+" "+ reservation.LastName});
    
                 _context.Add(reservation);
                await _context.SaveChangesAsync();

                _context.Notifications.Add(new Notification { MarkAsRead = false, Message = $"You have New Reservation from {reservation.FirstName}", RervationId = reservation.id });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reservation.UserID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reservation.UserID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,FirstName,LastName,UserID,Address,HallName,PhoneNo,NumberofGuests,ReservationDate,Title,Status")] Reservation reservation)
        {
            if (id != reservation.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", reservation.UserID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.user)
                .SingleOrDefaultAsync(m => m.id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            if (reservation.ReservationDate < DateTime.Now.AddDays(3) && !User.IsInRole("Admin"))
            {
                TempData["error"] = "You cancel before the 3 days of your reservation date";
                return RedirectToAction(nameof(Index));
            }


            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.id == id);
            if (reservation.ReservationDate < DateTime.Now.AddDays(3) && !User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.id == id);
        }
    }
}

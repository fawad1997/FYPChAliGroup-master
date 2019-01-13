using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WeddingVibes.Services;

namespace WeddingVibes.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
        public static Task SendEmailReservationAsync(this IEmailSender emailSender, string email, Message message)
        {
            return emailSender.SendEmailAsync(email, message.Title,
                $"You Have new reservation on {message.ReservationDate} from Mr./Ms. {message.ReserverName}");
        }
    }
    public class Message {
        public string Title { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReserverName { get; set; }

    }
}

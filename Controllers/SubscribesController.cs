using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignNewsLetter.Data;
using System.Net;
using System.Net.Mail;

namespace SignNewsLetter
{
    public class SignNewsLetterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SignNewsLetterController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
        public IActionResult SuccessSub()
        {
            return View();
        }
        public IActionResult DuplicateEmail()
        {
            return View();
        }

        public async Task<IActionResult> Subscribe([Bind("Id,Name,Email,SubscribesSince,IsActive")] Subscribe subscribe)
        {
            var email = await _context.Subscribes.AnyAsync(x => x.Email == subscribe.Email);

            if (email)
                return RedirectToAction(nameof(DuplicateEmail));

            if (ModelState.IsValid)
            {
                subscribe.Id = Guid.NewGuid();
                subscribe.SubscribesSince = DateTime.Now;

                _context.Add(subscribe);
                await _context.SaveChangesAsync();

                string subject = "Bem-vindo à nossa Newsletter!";

                string body = $"Olá {subscribe.Name},\n\nObrigado por se inscrever na nossa newsletter. Fique atento às novidades!";

                SendEmail(subscribe.Email, subject, body);

                return RedirectToAction(nameof(SuccessSub));
            }

            return View(subscribe);
        }

        public async Task<IActionResult> Unsubscribe(Guid? id)
        {
            var searchId = await _context.Subscribes.AnyAsync(x => x.Id.Equals(id));

            var subscribe = await _context.Subscribes.FirstOrDefaultAsync(x => x.Id == id);


            if (id == null || !searchId)

                return RedirectToAction(nameof(NotFoundPage));


            return View(subscribe);


        }

        [HttpPost, ActionName("Unsubscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var subscribe = await _context.Subscribes.FindAsync(id);

            if (subscribe == null || subscribe.Id != id || id == null)
            {
                return View(nameof(Subscribe));
            }

            if (subscribe != null)
            {
                _context.Subscribes.Remove(subscribe);
                await _context.SaveChangesAsync();

                

                string subject = "Desinscrição da Newsletter";

                string body = $"Olá {subscribe.Name},\n\nVocê foi desinscrito da nossa newsletter. Sentimos muito em vê-lo partir!";

                SendEmail(subscribe.Email, subject, body);
            }

            return RedirectToAction(nameof(Subscribe));
        }


        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string smtpEmail = Environment.GetEnvironmentVariable("SMTP_Email");

                string smtpPassword = Environment.GetEnvironmentVariable("SMTP_Password");

                string smtpHost = Environment.GetEnvironmentVariable("SMTP_Host");
                
                int smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_Port") ?? "587");

                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
                    smtpClient.EnableSsl = true; 
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
            }
        }

    }

}

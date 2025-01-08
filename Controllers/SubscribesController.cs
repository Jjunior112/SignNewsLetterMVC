
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignNewsLetter.Data;

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

                _context.Add(subscribe);

                await _context.SaveChangesAsync();

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

            if (subscribe.Id != id || id == null)
            {
                return View(nameof(Subscribe));
            }
            if (subscribe != null)
            {
                _context.Subscribes.Remove(subscribe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Subscribe));
        }


    }

}

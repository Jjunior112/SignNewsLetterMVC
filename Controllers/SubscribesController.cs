
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NomeDoProjeto.Data;

namespace SignNewsLetter
{
    public class SignNewsLetterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SignNewsLetterController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Subscribe([Bind("Id,Name,Email,SubscribesSince,IsActive")] Subscribe subscribe)
        {
            var email = await _context.Subscribes.AnyAsync(x => x.Email == subscribe.Email);

            if (ModelState.IsValid)
            {
                subscribe.Id = Guid.NewGuid();

                _context.Add(subscribe);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(subscribe);
        }
        public async Task<IActionResult> Unsubscribe(Guid? id)
        {
            if(id==null)
                return NotFound();
                
            var subscribe = await _context.Subscribes.FindAsync(id)
            
            subscribe.IsActive = false;
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(index));
            
        }

    }

}

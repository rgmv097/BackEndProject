using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;

namespace Project.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public ContactController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync();
         
            return View(contact);
        }
        public async Task<IActionResult>  Create()
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync();
            if(contact is not null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _dbContext.AddAsync(new Contact
            {
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                WebSite = model.WebSite,

            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id is null) return NotFound();

            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact is null) return NotFound();

            return View(new ContactUpdateViewModel
            {
                Address = contact.Address,
                PhoneNumber = contact.PhoneNumber,
                WebSite = contact.WebSite,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ContactUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id is null) return NotFound();

            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact is null) return NotFound();
            contact.Address = model.Address;
            contact.PhoneNumber = model.PhoneNumber;
            contact.WebSite = model.WebSite;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact is null) return NotFound();
            _dbContext.Contacts.Remove(contact);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

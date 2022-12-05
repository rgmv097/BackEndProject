using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;

namespace Project.Areas.Admin.Controllers
{
    public class InformationController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public InformationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var footerInformations = await _dbContext.FooterInformations.ToListAsync();
            return View(footerInformations);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InformationCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var information = await _dbContext.FooterInformations.FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == model.Name.Trim().ToLower());
            if (information is not null)
            {
                ModelState.AddModelError("", "There is a Information with this name in the system. Please choose another name");
                return View();
            }
            await _dbContext.AddAsync(new FooterInformation
            {
                Name = model.Name,
                Link=model.Url,

            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var existInformation = await _dbContext.FooterInformations.FindAsync(id);
            if (existInformation is null) return NotFound();

            return View(new InformationUpdateViewModel
            {
                Name = existInformation.Name,
                Url = existInformation.Link
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,InformationUpdateViewModel model)
        {
            if (id is null) return BadRequest();
            var existInformation = await _dbContext.FooterInformations.FindAsync(id);
            if (existInformation is null) return NotFound();
            if (ModelState.IsValid) return View(model);

            model.Name= existInformation.Name;
            model.Url = existInformation.Link;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var information = await _dbContext.FooterInformations.FindAsync(id);
            if (information is null) return NotFound();
            if (information.Id != id) return BadRequest();
            _dbContext.FooterInformations.Remove(information);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

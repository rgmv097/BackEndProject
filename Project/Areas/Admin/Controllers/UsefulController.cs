using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;

namespace Project.Areas.Admin.Controllers
{
    public class UsefulController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public UsefulController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var footerUsefuls = await _dbContext.FooterUsefuls.ToListAsync();
            return View(footerUsefuls);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsefulCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var useful = await _dbContext.FooterUsefuls.FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == model.Name.Trim().ToLower());
            if (useful is not null)
            {
                ModelState.AddModelError("", "There is a Information with this name in the system. Please choose another name");
                return View();
            }
            await _dbContext.AddAsync(new FooterUseful
            {
                Name = model.Name,
                Link = model.Url,

            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var existUseful = await _dbContext.FooterUsefuls.FindAsync(id);
            if (existUseful is null) return NotFound();

            return View(new UsefulUpdateViewModel
            {
                Name = existUseful.Name,
                Url = existUseful.Link
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, UsefulUpdateViewModel model)
        {
            if (id is null) return BadRequest();
            var existUseful = await _dbContext.FooterUsefuls.FindAsync(id);
            if (existUseful is null) return NotFound();
            if (ModelState.IsValid) return View(model);

            model.Name = existUseful.Name;
            model.Url = existUseful.Link;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var useful = await _dbContext.FooterUsefuls.FindAsync(id);
            if (useful is null) return NotFound();
            if (useful.Id != id) return BadRequest();
            _dbContext.FooterUsefuls.Remove(useful);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;

namespace Project.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult>  Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == model.Name.Trim().ToLower());
            if(category is not null)
            {
                ModelState.AddModelError("", "There is a category with this name in the system. Please choose another name");
                return View();
            }
            await _dbContext.AddAsync(new Category
            {
                Name = model.Name,
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        
        public async Task<IActionResult>Update(int? id)
        {
            if (id is null) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null) return NotFound();

            return View(new CategoryUpdateViewModel
            {
                Name = category.Name,
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Update(int? id,CategoryUpdateViewModel model)
        {
            if (id is null) return NotFound();
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null) return NotFound();
            if (category.Id != id) return BadRequest();
            var existCategory = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == model.Name.Trim().ToLower());
            if(existCategory is not null)
            {
                ModelState.AddModelError("", "There is a category with this name in the system. Please choose another name");
                return View();
            }
            category.Name= model.Name;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id is null) return NotFound();
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null) return NotFound();
            if(category.Id != id) return BadRequest();
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

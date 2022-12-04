using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.Models;

namespace Project.ViewComponents
{
    public class BlogSideBarViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public BlogSideBarViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
         public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).Include(c => c.Courses).ToListAsync();
            var blogs = await _dbContext.Blogs.Where(b => !b.IsDeleted).Take(3).OrderByDescending(b => b.Id).ToListAsync();

            var model = new BlogSideBarViewModel
            {
                Blogs = blogs,
                Categories = categories
            };
            return View(model);
        }
    }
}

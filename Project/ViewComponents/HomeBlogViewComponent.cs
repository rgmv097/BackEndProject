using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;

namespace Project.ViewComponents
{
    public class HomeBlogViewComponent : ViewComponent
    {

        private readonly AppDbContext _dbContext;

        public HomeBlogViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _dbContext.Blogs.Where(c => !c.IsDeleted).OrderByDescending(c => c.Id).ToListAsync();
            return View(model);
        }

    }
}

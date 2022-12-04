using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;

namespace Project.ViewComponents
{
   
    public class CoursesViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public CoursesViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _dbContext.Courses.Where(c => !c.IsDeleted).OrderByDescending(c => c.Id).ToListAsync();
            return View(model);
        }
    }
}

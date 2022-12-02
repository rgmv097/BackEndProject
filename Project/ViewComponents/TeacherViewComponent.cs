using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.DAL.Entities;

namespace Project.ViewComponents
{
    public class TeacherViewComponent:ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public TeacherViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take)
        {
            var teachers = _dbContext.Teachers;

            if (take == 0)
            {
                return View(await teachers.Where(x => !x.IsDeleted).ToListAsync());
            }

            return View(await teachers.Take(take).ToListAsync());

        }
    }   
}

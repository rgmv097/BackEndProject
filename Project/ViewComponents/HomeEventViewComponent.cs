using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;

namespace Project.ViewComponents
{
    public class HomeEventViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public HomeEventViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _dbContext.Events.Where(c => !c.IsDeleted).OrderByDescending(e=>e.Id).ToListAsync();
            return View(model);
        }
    }
}

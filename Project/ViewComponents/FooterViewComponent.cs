using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.Models;

namespace Project.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public FooterViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _dbContext.Settings.FirstOrDefaultAsync();
            var usefuls= await _dbContext.FooterUsefuls.ToListAsync();
            var informations= await _dbContext.FooterInformations.ToListAsync();

            var model = new FooterViewModel
            {
                Settings = settings,
                FooterUsefuls = usefuls,
                FooterInformations = informations
            };
            return View(model);
           
        }
    }
}

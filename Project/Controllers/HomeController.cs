using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.Data;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private int _blogCount;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _blogCount = _dbContext.Blogs.Count();

        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.Where(x => !x.IsDeleted).ToListAsync();           
            return View(sliders);
        }

        public async Task<IActionResult> Partial(int skip)
        {
            if (skip >= _blogCount)
                return BadRequest();
            var blogs = await _dbContext.Blogs.Skip(skip).Take(4).ToListAsync();
            return PartialView("_BlogPartial", blogs);
        }





    }
}
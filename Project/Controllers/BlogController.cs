using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using System.Runtime.CompilerServices;

namespace Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.Where(x => !x.IsDeleted).ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _dbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (blog == null) return NotFound();

            return View(blog);
        }
    }
}

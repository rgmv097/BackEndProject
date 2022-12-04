using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using System.Runtime.CompilerServices;

namespace Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;
        private int _blogCount; 

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _blogCount = _dbContext.Blogs.Count();
        }

        public IActionResult Index()
        {
            ViewBag.blogCount = _blogCount;
            var blogs= _dbContext.Blogs.Take(3).ToList();
            return View(blogs);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _dbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (blog == null) return NotFound();

            return View(blog);
        }
        public async Task<IActionResult> Partial(int skip)
        {
            if (skip >= _blogCount)
                return BadRequest();
            var blogs = await _dbContext.Blogs.Skip(skip).Take(4).ToListAsync();
            return PartialView("_BlogPartial",blogs);
        }
    }
}

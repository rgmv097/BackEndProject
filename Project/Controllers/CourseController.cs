using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.Models;

namespace Project.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var course = await _dbContext.Courses.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if(course == null) return NotFound();
            return View(course);
        }
       
        public async Task<IActionResult> Search(string searchText)
        {
            //if (string.IsNullOrEmpty(searchText))
            //    return RedirectToAction(nameof(Index));

            var courses = await _dbContext.Courses
                .Where(c => c.Name.ToLower()
                .Contains(searchText.ToLower()))
                .ToListAsync();
            return PartialView("_SearchedCoursePartialView",courses);
        }
    }
}

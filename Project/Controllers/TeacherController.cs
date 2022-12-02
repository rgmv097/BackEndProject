using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;

namespace Project.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _dbContext;

        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult>Index()
        {
            var teachers = await _dbContext.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            return View(teachers);
        }
        public async Task<IActionResult>Details(int? id)
        {
            if(id is null) NotFound();
            var teacher = await _dbContext.Teachers.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if(teacher is null) NotFound();
            return View(teacher);
        }
    }
}

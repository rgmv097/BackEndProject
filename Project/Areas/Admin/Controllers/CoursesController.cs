using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;
using System.Xml.Linq;

namespace Project.Areas.Admin.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CoursesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var Courses = await _dbContext.Courses.Include(c => c.Category).Where(c => !c.IsDeleted).ToListAsync();
            return View(Courses);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();
            if (categories is null) return NotFound();
            var categoriesSelectList = new List<SelectListItem>();
            categories.ForEach(c => categoriesSelectList.Add(new SelectListItem(c.Name, c.Id.ToString())));
            var model = new CourseCreateViewModel()
            {

                Categories = categoriesSelectList
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            var courses = await _dbContext.Courses
                .Where(c => !c.IsDeleted)
                .Include(x => x.Category)
                .ToListAsync();

            if (courses is null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var categoryListItems = new List<SelectListItem>();

            courses.ForEach(x => categoryListItems.Add(new SelectListItem(x.Name, x.Id.ToString())));

            var viewModel = new CourseCreateViewModel()
            {
                Categories = categoryListItems
            };


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Choose a image format");
                return View();
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");
                return View();
            }

            var unicalFileName = await model.Image.GenerateFile(Constants.CoursePath);



            var createdCourse = new Course
            {
                Name = model.Name,
                About = model.About,
                Description = model.Description,
                Fee = model.Fee,
                StartTime = model.StartTime,
                Duration = model.Duration,
                ClassDuration = model.ClassDuration,
                Apply = model.Apply,
                Assesments = model.Assesments,
                Student = model.Student,
                Certifiaction = model.Certifiaction,
                SkillLevel = model.SkillLevel,
                ImageUrl = unicalFileName,
                CategoryId = model.CategoryId,
                Language = model.Language,
            };

            await _dbContext.Courses.AddAsync(createdCourse);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var existCourse = await _dbContext.Courses.
                Where(c => !c.IsDeleted && c.Id == id).
                Include(c => c.Category).
                FirstOrDefaultAsync();
            if (existCourse == null) return NotFound();
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (categories == null) return NotFound();
            var courseCategories = new List<SelectListItem>();
            categories.ForEach(c => courseCategories
            .Add(new SelectListItem(c.Name, c.Id.ToString())));
           
            var model = new CourseUpdateViewModel
            {
                Name = existCourse.Name,
                About = existCourse.About,
                Fee = existCourse.Fee,
                ImageUrl = existCourse.ImageUrl,
                Apply = existCourse.Apply,
                Description = existCourse.Description,
                Certifiaction = existCourse.Certifiaction,
                Duration = existCourse.Duration,
                ClassDuration = existCourse.ClassDuration,
                SkillLevel = existCourse.SkillLevel,
                Language = existCourse.Language,
                Assesments = existCourse.Assesments,
                StartTime = existCourse.StartTime,
                Student = existCourse.Student,
                Categories = courseCategories,
                CategoryId = existCourse.CategoryId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateViewModel model)
        {
            if (id is null) return NotFound();
            var existCourse = await _dbContext.Courses.
                Where(c => !c.IsDeleted && c.Id == id).
                Include(c => c.Category).
                FirstOrDefaultAsync();
            if (existCourse == null) return NotFound();
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (categories == null) return NotFound();
            var courseCategories = new List<SelectListItem>();
            categories.ForEach(c => courseCategories
            .Add(new SelectListItem(c.Name, c.Id.ToString())));

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartTime) >= 0)
            {
                ModelState.AddModelError("StartTime", "The start time must be later than the current time");
                return View(model);
            }
            if (model.Image != null)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(new EventUpdateViewModel
                    {
                        ImageUrl = existCourse.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(new EventUpdateViewModel
                    {
                        ImageUrl = existCourse.ImageUrl,
                    });
                }
                var path = Path.Combine(Constants.CoursePath, existCourse.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Image.GenerateFile(Constants.CoursePath);

                existCourse.ImageUrl = unicalFileName;
            }
            existCourse.Name = model.Name;
            existCourse.About = model.About;
            existCourse.Description = model.Description;
            existCourse.Duration = model.Duration;
            existCourse.ClassDuration = model.ClassDuration;
            existCourse.Apply = model.Apply;
            existCourse.Assesments = model.Assesments;
            existCourse.Certifiaction = model.Certifiaction;
            existCourse.SkillLevel = model.SkillLevel;
            existCourse.StartTime = model.StartTime;
            existCourse.Fee = model.Fee;
            existCourse.Language = model.Language;
            existCourse.Student = model.Student;
            existCourse.CategoryId = model.CategoryId;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }       

            public async Task<IActionResult> Delete(int? id)
            {
                if (id is null) return BadRequest();
                var existCourse = await _dbContext.Courses
                      .FirstOrDefaultAsync(e => e.Id == id);
                if (existCourse is null) return NotFound();
                if (existCourse.Id != id) return BadRequest();
                var courseImagePath = Path.Combine(Constants.CoursePath, existCourse.ImageUrl);

                if (System.IO.File.Exists(courseImagePath))
                    System.IO.File.Delete(courseImagePath);

                _dbContext.Courses.Remove(existCourse);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var teacherList = await _dbContext.Teachers.ToListAsync();
            return View(teacherList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

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
            var unicalFileName = await model.Image.GenerateFile(Constants.TeacherPath);

            await _dbContext.Teachers.AddAsync(new Teacher
            {
                Fullname=model.Fullname,
                Profession=model.Profession,
                About=model.About,
                Degree=model.Degree,
                Experience=model.Experience,
                Hobbies=model.Hobbies,
                Faculty=model.Faculty,
                Language=model.Language,
                TeamLeader=model.TeamLeader,
                Development=model.Development,
                Design=model.Design,
                Innavation=model.Innavation,
                Communication=model.Communication,
                Email=model.Email,
                PhoneNumber=model.PhoneNumber,
                Skype=model.Skype,
                ImageUrl=unicalFileName
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _dbContext.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            return View(new TeacherUpdateViewModel
            {
                Profession = teacher.Profession,
                ImageUrl = teacher.ImageUrl,
                Fullname= teacher.Fullname,
                About=teacher.About,
                Degree=teacher.Degree,
                Experience=teacher.Experience,
                Hobbies=teacher.Hobbies,
                Faculty=teacher.Faculty,
                Language=teacher.Language,
                TeamLeader=teacher.TeamLeader,
                Development=teacher.Development,
                Design=teacher.Design,
                Innavation=teacher.Innavation,
                Communication = teacher.Communication,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Skype = teacher.Skype,                
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,TeacherUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var teacher=await _dbContext.Teachers.FindAsync(id); 
            if (teacher == null) return NotFound();
            if(teacher.Id!=id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new TeacherUpdateViewModel
                {                   
                    ImageUrl = teacher.ImageUrl,                   
                });
            }
            if (model.Image != null)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(new TeacherUpdateViewModel
                    {    
                        ImageUrl = teacher.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(new TeacherUpdateViewModel
                    {                       
                        ImageUrl = teacher.ImageUrl,                    
                    });
                }
                var path = Path.Combine(Constants.TeacherPath, teacher.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Image.GenerateFile(Constants.TeacherPath);
                teacher.ImageUrl = unicalFileName;
            }
            teacher.Profession = model.Profession;
            teacher.Fullname = model.Fullname;
            teacher.About = model.About;
            teacher.Degree = model.Degree;
            teacher.Experience = model.Experience;
            teacher.Hobbies = model.Hobbies;
            teacher.Faculty = model.Faculty;
            teacher.Language = model.Language;
            teacher.TeamLeader = model.TeamLeader;
            teacher.Development = model.Development;
            teacher.Design = model.Design;
            teacher.Innavation = model.Innavation;
            teacher.Communication = model.Communication;
            teacher.Email = model.Email;
            teacher.PhoneNumber = model.PhoneNumber;
            teacher.Skype = model.Skype;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _dbContext.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            if (teacher.Id != id) BadRequest();

            var path = Path.Combine(Constants.TeacherPath, teacher.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Teachers.Remove(teacher);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

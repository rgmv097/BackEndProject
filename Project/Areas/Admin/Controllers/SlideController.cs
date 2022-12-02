using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;
using Constants = Project.Data.Constants;

namespace Project.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public SlideController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var slideList = await _dbContext.Sliders.ToListAsync();

            return View(slideList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideCreateViewModel model)
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

            var unicalFileName = await model.Image.GenerateFile(Constants.SliderPath);

            await _dbContext.Sliders.AddAsync(new Slider
            {            
                Title=model.Title,
                Subtitle=model.Subtitle,
                ImageUrl = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slide = await _dbContext.Sliders.FindAsync(id);

            if (slide == null) return NotFound();

            if (slide.Id != id) BadRequest();

            var path = Path.Combine(Constants.SliderPath, slide.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Sliders.Remove(slide);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _dbContext.Sliders.FindAsync(id);

            if (slider == null) return NotFound();

            return View(new SlideUpdateViewModel
            {
                ImageName = slider.ImageUrl,
                Title = slider.Title,
                Subtitle = slider.Subtitle,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SlideUpdateViewModel model)
        {
            if (id == null) return NotFound();

            var Slider = await _dbContext.Sliders.FindAsync(id);

            if (Slider == null) return NotFound();

            if (Slider.Id != id) BadRequest();

            if (!ModelState.IsValid)
            {
                return View(new SlideUpdateViewModel
                {
                    ImageName = Slider.ImageUrl,                  

                });
            }
            if (model.Image != null)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(new SlideUpdateViewModel
                    {
                        ImageName = Slider.ImageUrl,                      
                    });
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(new SlideUpdateViewModel
                    {
                        ImageName = Slider.ImageUrl,

                    });
                }
                var path = Path.Combine(Constants.SliderPath, Slider.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Image.GenerateFile(Constants.SliderPath);
                Slider.ImageUrl = unicalFileName;
            }
            Slider.Subtitle = model.Subtitle;
            Slider.Title = model.Title;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }

}

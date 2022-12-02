using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    public class SpeakerController : BaseController
    {
        private readonly AppDbContext _dbContext;
        public SpeakerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var speakerList = await _dbContext.Speakers.ToListAsync();

            return View(speakerList);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
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
            var unicalFileName = await model.Image.GenerateFile(Constants.SpeakerPath);

            await _dbContext.Speakers.AddAsync(new Speaker
            {
                FullName = model.FullName,
                Profession = model.Profession,
                ImageUrl = unicalFileName
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var speaker = await _dbContext.Speakers.FindAsync(id);

            if (speaker == null) return NotFound();

            return View(new SpeakerUpdateViewModel
            {
                Profession = speaker.Profession,
                ImageUrl = speaker.ImageUrl,
                FullName = speaker.FullName,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var speaker = await _dbContext.Speakers.FindAsync(id);
            if (speaker == null) return NotFound();
            if (speaker.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new SpeakerUpdateViewModel
                {
                    ImageUrl = speaker.ImageUrl,
                });
            }
            if (model.Image != null)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(new SpeakerUpdateViewModel
                    {
                        ImageUrl = speaker.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(new SpeakerUpdateViewModel
                    {
                        ImageUrl = speaker.ImageUrl,
                    });
                }
                var path = Path.Combine(Constants.SpeakerPath, speaker.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Image.GenerateFile(Constants.SpeakerPath);
                speaker.ImageUrl = unicalFileName;
            }
            speaker.Profession = model.Profession;
            speaker.FullName = model.FullName;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var speaker = await _dbContext.Speakers.FindAsync(id);

            if (speaker == null) return NotFound();

            if (speaker.Id != id) BadRequest();

            var path = Path.Combine(Constants.SpeakerPath,speaker.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Speakers.Remove(speaker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

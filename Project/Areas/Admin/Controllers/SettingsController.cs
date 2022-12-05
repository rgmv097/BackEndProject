using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public SettingsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _dbContext.Settings.FirstOrDefaultAsync();
            if (settings is null) return View();
            return View(settings);
        }

        public async Task<IActionResult> Create()
        {
            var contact = await _dbContext.Settings.FirstOrDefaultAsync();
            if (contact is not null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingsCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();
            if (!model.HeaderLogo.IsImage())
            {
                ModelState.AddModelError("Image", "Choose a image format");
                return View();
            }
            if (!model.FooterLogo.IsImage())
            {
                ModelState.AddModelError("Image", "Choose a image format");
                return View();
            }

            if (!model.HeaderLogo.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");
                return View();
            }
            if (!model.FooterLogo.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");
                return View();
            }
            var unicalFileNameHeader = await model.HeaderLogo.GenerateFile(Constants.HeaderPath);
            var unicalFileNameFooter = await model.HeaderLogo.GenerateFile(Constants.FooterPath);

            await _dbContext.Settings.AddAsync(new Settings
            {
                HeaderLogoUrl = unicalFileNameHeader,
                FooterLogoUrl = unicalFileNameFooter,
                Facebook = model.Facebook,
                Twitter = model.Twitter,
                Viber = model.Viber,
                Pinterest = model.Pinterest,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberSecond = model.PhoneNumberSecond,
                Email = model.Email,
                EmailSecond = model.EmailSecond,
                Adress = model.Adress,


            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var setting = await _dbContext.Settings.FindAsync(id);

            if (setting == null) return NotFound();

            return View(new SettingsUpdateViewModel
            {
                HeaderLogoUrl = setting.HeaderLogoUrl,
                FooterLogoUrl = setting.FooterLogoUrl,
                Facebook = setting.Facebook,
                Twitter = setting.Twitter,
                Viber = setting.Viber,
                Pinterest = setting.Pinterest,
                PhoneNumber = setting.PhoneNumber,
                PhoneNumberSecond = setting.PhoneNumberSecond,
                Email = setting.Email,
                EmailSecond = setting.EmailSecond,
                Adress = setting.Adress,
                FooterDescription=setting.FooterDescription

            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SettingsUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var setting = await _dbContext.Settings.FindAsync(id);
            if (setting == null) return NotFound();
            if (setting.Id != id) return BadRequest();
            var viewModel = new SettingsUpdateViewModel
            {
                HeaderLogoUrl = setting.HeaderLogoUrl,
                FooterLogoUrl = setting.FooterLogoUrl,
            };
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if (model.HeaderLogo != null)
            {

                if (!model.HeaderLogo.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(viewModel);
                }

                if (!model.HeaderLogo.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(viewModel);
                }
                var path = Path.Combine(Constants.HeaderPath, setting.HeaderLogoUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileNameHeader = await model.HeaderLogo.GenerateFile(Constants.HeaderPath);
                setting.HeaderLogoUrl = unicalFileNameHeader;
            }
            if (model.FooterLogo != null)
            {

                if (!model.FooterLogo.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(viewModel);
                }

                if (!model.FooterLogo.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(viewModel);
                }
                var path = Path.Combine(Constants.FooterPath, setting.FooterLogoUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileNameFooter = await model.HeaderLogo.GenerateFile(Constants.FooterPath);
                setting.HeaderLogoUrl = unicalFileNameFooter;
            }
            setting.Facebook = model.Facebook;
            setting.Twitter = model.Twitter;
            setting.Viber= model.Viber;
            setting.Twitter= model.Twitter;
            setting.PhoneNumber = model.PhoneNumber;
            setting.PhoneNumberSecond= model.PhoneNumberSecond;
            setting.Adress= model.Adress;
            setting.Email= model.Email;
            setting.EmailSecond=model.EmailSecond;
            setting.FooterDescription=model.FooterDescription;
      
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var setting = await _dbContext.Settings.FindAsync(id);

            if (setting == null) return NotFound();

            if (setting.Id != id) BadRequest();

            var headerPath = Path.Combine(Constants.HeaderPath, setting.HeaderLogoUrl);
            var footerPath = Path.Combine(Constants.FooterPath, setting.FooterLogoUrl);


            if (System.IO.File.Exists(headerPath))
                System.IO.File.Delete(headerPath);
            if (System.IO.File.Exists(footerPath))
                System.IO.File.Delete(footerPath);


            _dbContext.Settings.Remove(setting);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

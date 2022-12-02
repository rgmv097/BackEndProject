using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogList = await _dbContext.Blogs.OrderByDescending(x => x.Id).ToListAsync();
            return View(blogList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
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
            var unicalFileName = await model.Image.GenerateFile(Constants.BlogPath);
            await _dbContext.Blogs.AddAsync(new Blog
            {
                Title= model.Title,
                Content= model.Content,
                ImageUrl=unicalFileName,
                Author=model.Author,
                DateTime=DateTime.Now,
                
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            if (blog.Id != id) BadRequest();

            var path = Path.Combine(Constants.BlogPath, blog.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            return View(new BlogUpdateViewModel
            {
               Title= blog.Title,
               Content= blog.Content,
               ImageUrl= blog.ImageUrl,
               Author= blog.Author,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var blog = await _dbContext.Blogs.FindAsync(id);
            if (blog == null) return NotFound();
            if (blog.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new BlogUpdateViewModel
                {
                    ImageUrl = blog.ImageUrl,
                });
            }
            if (model.Image != null)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(new TeacherUpdateViewModel
                    {
                        ImageUrl = blog.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(new TeacherUpdateViewModel
                    {
                        ImageUrl = blog.ImageUrl,
                    });
                }
                var path = Path.Combine(Constants.BlogPath, blog.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Image.GenerateFile(Constants.BlogPath);
                blog.ImageUrl = unicalFileName;
            }
            blog.Content=model.Content;
            blog.Title  = model.Title;
            blog.Author= model.Author;  
            blog.DateTime= DateTime.Now;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}

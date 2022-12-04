using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.DependencyResolver;
using Project.Areas.Admin.Data;
using Project.Areas.Admin.Models;
using Project.DAL;
using Project.DAL.Entities;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    public class EventController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public EventController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(s => s.Speaker)
                .Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).ToListAsync();
            return View(events);
        }
        public async Task<IActionResult> Create()
        {
            var speakers = await _dbContext.Speakers
                .Where(x => !x.IsDeleted)
                .ToListAsync();
            var eventSpeakersList = new List<SelectListItem>
            {
                new SelectListItem("choose","0")
            };

            speakers.ForEach(x => eventSpeakersList.Add(new SelectListItem(x.FullName, x.Id.ToString())));
            var model = new EventCreateViewModel
            {
                Speakers = eventSpeakersList,
            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var eventSpeakersSelectList = new List<SelectListItem>();
            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.FullName, c.Id.ToString())));
            var viewModel = new EventCreateViewModel
            {
                Speakers = eventSpeakersSelectList
            };
            if (!ModelState.IsValid) return View(viewModel);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Choose a image format");
                return View(viewModel);
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");
                return View(viewModel);
            }
            var unicalFileName = await model.Image.GenerateFile(Constants.EventPath);

            var events = new Event
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = unicalFileName,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Venue = model.Venue,
            };

            if (model.SpeakerId.Count > 0)
            {
                foreach (int speakerId in model.SpeakerId)
                {
                    if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                    {
                        ModelState.AddModelError("SpeakerId", "Incorrect Speaker");
                        return View(model);
                    }

                    eventSpeakers.Add(new EventSpeaker
                    {
                        SpeakerId = speakerId,
                    });

                }

                events.EventSpeakers = eventSpeakers;
            }
            else
            {
                ModelState.AddModelError("", "Pls Select MIN one Speaker");
                return View(new EventCreateViewModel
                {
                    Speakers = eventSpeakersSelectList,
                });

            }
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartTime) >= 0)
            {
                ModelState.AddModelError("StartTime", "The start time must be later than the current time");
                return View(viewModel);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndTime) >= 0)
            {
                ModelState.AddModelError("EndTime", "The end time must be later than the current time");
                return View(viewModel);
            }

            if (DateTime.Compare(model.StartTime, model.EndTime) >= 0)
            {
                ModelState.AddModelError("", "The start time must be earlier than the end time");
                return View(viewModel);
            }         

            await _dbContext.Events.AddAsync(events);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var existEvent = await _dbContext.Events
                  .FirstOrDefaultAsync(e => e.Id == id);
            if (existEvent is null) return NotFound();
            if (existEvent.Id != id) return BadRequest();
            var eventImagePath = Path.Combine(Constants.EventPath, existEvent.ImageUrl);
            if (System.IO.File.Exists(eventImagePath))
                System.IO.File.Delete(eventImagePath);
            _dbContext.Events.Remove(existEvent);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var existEvent = await _dbContext.Events
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(es => es.EventSpeakers)
                .ThenInclude(s => s.Speaker).FirstOrDefaultAsync();
            if (existEvent is null) return NotFound();
            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var eventSpeakersSelectList = new List<SelectListItem>();
            speakers.ForEach(e => eventSpeakersSelectList
            .Add(new SelectListItem(e.FullName, e.Id.ToString())));
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            foreach (EventSpeaker eventSpeaker in existEvent.EventSpeakers)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == eventSpeaker.SpeakerId))
                {
                    ModelState.AddModelError("", "Incorect Speaker Id");
                    return View();
                }
                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = eventSpeaker.SpeakerId
                });
            }
            var eventViewModel = new EventUpdateViewModel
            {
                Title = existEvent.Title,
                Description = existEvent.Description,
                StartTime = existEvent.StartTime,
                EndTime = existEvent.EndTime,
                ImageUrl = existEvent.ImageUrl,
                Venue = existEvent.Venue,
                Speakers = eventSpeakersSelectList,
                SpeakerId = eventSpeakers.Select(s => s.SpeakerId).ToList()
            };
            return View(eventViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EventUpdateViewModel model, int? id)
        {
            if (id is null) return BadRequest();
            var existEvent = await _dbContext.Events
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(es => es.EventSpeakers)
                .ThenInclude(s => s.Speaker)
                .FirstOrDefaultAsync();
            if (existEvent == null) return NotFound();
            var speakers = await _dbContext.Speakers
                .Where(s => !s.IsDeleted)
                .ToListAsync();
            
            var eventSpeakersSelectList = new List<SelectListItem>();
            var viewModel = new EventUpdateViewModel
            {
                ImageUrl = existEvent.ImageUrl,
                Speakers = eventSpeakersSelectList,
            };
            speakers.ForEach(e => eventSpeakersSelectList
            .Add(new SelectListItem(e.FullName, e.Id.ToString())));
            if (model.SpeakerId.Count > 0)
            {
                foreach (int speakerId in model.SpeakerId)
                {
                    if (!await _dbContext.Speakers.AnyAsync(c => c.Id == speakerId))
                    {
                        ModelState.AddModelError("", "Has been selected incorrect speaker.");
                        return View(viewModel);
                    }
                }
                List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
                foreach (var spekarId in model.SpeakerId)
                {
                    EventSpeaker eventSpeaker = new EventSpeaker
                    {
                        SpeakerId = spekarId
                    };
                    eventSpeakers.Add(eventSpeaker);
                }
                existEvent.EventSpeakers = eventSpeakers;
            }
            else
            {
                ModelState.AddModelError("", "Pls Select MIN one Speaker");
                return View(viewModel);
            }
            if (!ModelState.IsValid) return View(viewModel);
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartTime) >= 0)
            {
                ModelState.AddModelError("StartTime", "The start time must be later than the current time");
                return View(viewModel);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndTime) >= 0)
            {
                ModelState.AddModelError("EndTime", "The end time must be later than the current time");
                return View(viewModel);
            }

            if (DateTime.Compare(model.StartTime, model.EndTime) >= 0)
            {
                ModelState.AddModelError("", "The start time must be earlier than the end time");
                return View(viewModel);
            }
            if (model.Image != null)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Choose a image format");

                    return View(viewModel);
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "The size of the image can be maximum 10 MB");

                    return View(viewModel);
                }
                var path = Path.Combine(Constants.EventPath, existEvent.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                var unicalFileName = await model.Image.GenerateFile(Constants.EventPath);

                existEvent.ImageUrl = unicalFileName;
            }
            existEvent.Title = model.Title;
            existEvent.Description = model.Description;
            existEvent.StartTime = model.StartTime;
            existEvent.EndTime = model.EndTime;
            existEvent.Venue = model.Venue;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}

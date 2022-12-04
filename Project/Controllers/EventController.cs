using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;

namespace Project.Controllers
{
    public class EventController : Controller
    {

        private readonly AppDbContext _dbContext;

        public EventController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events.Where(e => !e.IsDeleted).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var exsitEvent = await _dbContext.Events
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.EventSpeakers).ThenInclude(e => e.Speaker)
                .FirstOrDefaultAsync();
            if (exsitEvent is null) return NotFound();
            return View(exsitEvent);

        }
    }
}

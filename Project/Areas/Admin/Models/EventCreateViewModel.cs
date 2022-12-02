using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class EventCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }      
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; }
        public IFormFile Image { get; set; }
        public List<SelectListItem> Speakers { get; set; } = new();
        public List<int> SpeakerId { get; set; }= new();
    }
}

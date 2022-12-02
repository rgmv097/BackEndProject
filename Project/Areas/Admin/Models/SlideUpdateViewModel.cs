using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class SlideUpdateViewModel
    {
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        [MinLength(15)]
        public string Title { get; set; }
        [MinLength(30)]
        public string Subtitle { get; set; }
    }
}

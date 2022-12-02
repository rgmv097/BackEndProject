using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class SlideCreateViewModel
    {
        public IFormFile Image { get; set; }
        [MinLength(15), MaxLength(30)]
        public string Title { get; set; }
        [MinLength(30), MaxLength(50)]
        public string Subtitle { get; set; }

    }
}

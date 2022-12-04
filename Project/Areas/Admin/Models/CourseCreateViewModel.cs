using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class CourseCreateViewModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certifiaction { get; set; }
        public DateTime StartTime { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        [Range(15, 25, ErrorMessage = "Can only be between 0 .. 25")]
        public int Student { get; set; }
        public string Assesments { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Fee { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();
        public int CategoryId { get; set; }
    }
}

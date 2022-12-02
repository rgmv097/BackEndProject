using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class TeacherUpdateViewModel
    {
        [MaxLength(20)]
        public string Fullname { get; set; }
        public string Profession { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        [Range(0, 100)]
        public byte Language { get; set; }
        [Range(0, 100)]
        public byte TeamLeader { get; set; }
        [Range(0, 100)]
        public byte Development { get; set; }
        [Range(0, 100)]
        public byte Design { get; set; }
        [Range(0, 100)]
        public byte Innavation { get; set; }
        [Range(0, 100)]
        public byte Communication { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}

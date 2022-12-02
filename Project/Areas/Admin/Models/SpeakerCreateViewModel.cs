namespace Project.Areas.Admin.Models
{
    public class SpeakerCreateViewModel
    {
        public IFormFile Image { get; set; }

        public string FullName { get; set; }
        public string Profession { get; set; }
    }
}

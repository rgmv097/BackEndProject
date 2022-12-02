namespace Project.Areas.Admin.Models
{
    public class BlogUpdateViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string Author { get; set; }
    }
}

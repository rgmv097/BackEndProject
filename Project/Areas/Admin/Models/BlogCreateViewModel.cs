namespace Project.Areas.Admin.Models
{
    public class BlogCreateViewModel
    {

        public string Title { get; set; }
        public string Content { get; set; }              
        public IFormFile Image { get; set; }
        public string Author { get; set; }
    }
}

using Project.DAL.Entities;

namespace Project.Models
{
    public class BlogSideBarViewModel
    { 
        public List<Category> Categories { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}

namespace Project.DAL.Entities
{
    public class Blog:Entity
    {
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }

    }
}

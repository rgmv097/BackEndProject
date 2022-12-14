namespace Project.DAL.Entities
{
    public class Course:Entity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certifiaction { get; set; }
        public DateTime StartTime { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int Student { get; set; }
        public string Assesments { get; set; }
        public decimal Fee { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }




    }
}

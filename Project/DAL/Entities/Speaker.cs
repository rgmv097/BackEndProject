namespace Project.DAL.Entities
{
    public class Speaker:Entity
    {
        public string FullName { get; set; }
        public string Profession { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<EventSpeaker> EventSpeakers { get; set; }

    }
}

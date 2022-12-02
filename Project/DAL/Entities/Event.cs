namespace Project.DAL.Entities
{
    public class Event:Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; }
        public string ImageUrl { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }

    }
}

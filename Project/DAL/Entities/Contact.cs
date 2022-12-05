namespace Project.DAL.Entities
{
    public class Contact:Entity
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string WebSite { get; set; }
        public string? Message { get; set; }
       
    }
}

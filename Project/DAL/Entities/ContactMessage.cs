using System.ComponentModel.DataAnnotations;

namespace Project.DAL.Entities
{
    public class ContactMessage:Entity
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool isRead { get; set; }
    }
}

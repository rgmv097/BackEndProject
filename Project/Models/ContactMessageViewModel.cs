using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ContactMessageViewModel
    { 
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}

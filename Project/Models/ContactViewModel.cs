
using Project.DAL.Entities;

namespace Project.Models
{
    public class ContactViewModel
    {
        public Contact Contact { get; set; } = new();
        public ContactMessageViewModel ContactMessage { get; set; }
    }
}

using Project.DAL.Entities;

namespace Project.Areas.Admin.Models
{
    public class ContactMessageReadViewModel
    {
        public List<ContactMessage> contactMessages= new List<ContactMessage>();
        public bool isReadAllMessage { get; set; }
    }
}

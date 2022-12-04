using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class ContactCreateViewModel
    {
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string WebSite { get; set; }
    }
}

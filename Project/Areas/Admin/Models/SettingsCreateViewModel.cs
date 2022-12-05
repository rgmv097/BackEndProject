using System.ComponentModel.DataAnnotations;

namespace Project.Areas.Admin.Models
{
    public class SettingsCreateViewModel
    {
        public IFormFile HeaderLogo { get; set; }
        public IFormFile FooterLogo { get; set; }
        public string? FooterDescription { get; set; }
        public string? Facebook { get; set; }
        public string? Pinterest { get; set; }
        public string? Viber { get; set; }
        public string? Twitter { get; set; }
        public string Adress { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Phone]
        public string? PhoneNumberSecond { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EmailAddress]
        public string? EmailSecond { get; set; }
    }
}


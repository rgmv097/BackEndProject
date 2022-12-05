namespace Project.DAL.Entities
{
    public class Settings:Entity
    {
       public string HeaderLogoUrl { get; set; }
        public string FooterLogoUrl { get; set; }      
        public string? FooterDescription { get; set; }
        public string? Facebook { get; set; }
        public string? Pinterest { get; set; }
        public string? Viber { get; set; }
        public string? Twitter { get; set; }      
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhoneNumberSecond { get; set;}
        public string Email { get; set; }
        public string? EmailSecond { get; set; }







    }
}

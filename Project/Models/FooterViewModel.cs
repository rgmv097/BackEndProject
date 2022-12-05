using Project.DAL.Entities;

namespace Project.Models
{
    public class FooterViewModel
    { 
        
        public Settings Settings { get; set; }
        public List<FooterInformation> FooterInformations { get; set; }
        public List<FooterUseful> FooterUsefuls { get; set; }

    }
}

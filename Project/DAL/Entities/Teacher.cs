namespace Project.DAL.Entities
{
    public class Teacher:Entity
    {
        public string Fullname { get; set; }
        public string Profession { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public byte Language { get; set; }
        public byte TeamLeader { get; set; }
        public byte Development { get; set; }
        public byte Design { get; set; }
        public byte Innavation { get; set; }
        public byte Communication { get; set;}
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public string ImageUrl { get; set; }



    }
}

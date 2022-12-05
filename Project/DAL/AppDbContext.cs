using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;
using System.Collections.Generic;

namespace Project.DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<EventSpeaker> EventsSpeakers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<FooterInformation> FooterInformations { get; set; }
        public DbSet<FooterUseful> FooterUsefuls { get; set; }

    }
}

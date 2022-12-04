using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Project.DAL;
using Project.Data;
using Constants = Project.Data.Constants;

namespace Project
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder =>
                    {
                        builder.MigrationsAssembly(nameof(Project));
                    });
            });
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 2;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));
            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.SliderPath = Path.Combine(Constants.RootPath, "img", "slider");
            Constants.TeacherPath = Path.Combine(Constants.RootPath, "img", "teacher");
            Constants.BlogPath = Path.Combine(Constants.RootPath, "img", "blog");
            Constants.SpeakerPath = Path.Combine(Constants.RootPath, "img", "speaker");
            Constants.EventPath = Path.Combine(Constants.RootPath, "img", "event");
            Constants.CoursePath = Path.Combine(Constants.RootPath, "img", "course");


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var dataInitalizer = new DataInitializer(serviceProvider);

                await dataInitalizer.SeedData();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );
                app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

            });



            await app.RunAsync();
        }
    }
}
using CourseManagment.BLL.Services;
using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Interfaces;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CourseManagment.DAL.Repositories.GenaricRepo;

namespace CourseManagmentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<CourseDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));
            builder.Services.AddScoped(typeof(IGenaricRepo<>), typeof(GenaricRepository<>));
            builder.Services.AddScoped(typeof(ICourseRepo), typeof(CourseRepository));
            builder.Services.AddScoped(typeof(IEnrollmentRepo), typeof(EnrollmentRepo));
            builder.Services.AddScoped(typeof(IDashboardRepo), typeof(DashboardRepo));
            builder.Services.AddScoped(typeof(ICategoryRepo), typeof(CategoryRepo));
            builder.Services.AddScoped(typeof(IInstructorRepo), typeof(InstructorRepo));
            builder.Services.AddScoped(typeof(IHomeRepo), typeof(HomeRepo));
            builder.Services.AddScoped<IProfileRepo, ProfileRepo>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped(typeof(ICourseService), typeof(CourseService));
            builder.Services.AddScoped(typeof(IInstructorService), typeof(InstructorService));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            builder.Services.AddScoped(typeof(IEnrollmentService), typeof(EnrollmentService));
            builder.Services.AddScoped(typeof(IDashboardService), typeof(DashboardService));
            builder.Services.AddScoped(typeof(IHomeService), typeof(HomeService));
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            // ------------------ الداتابيز (نفس الداتابيز اللي كريتها صاحبك بالـ EF) ------------------
            builder.Services.AddDbContext<CourseDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));

            // ------------------ الريبوزيتوري العام (Generic Repository) ------------------
            builder.Services.AddScoped(typeof(IGenaricRepo<>), typeof(GenaricRepo.GenaricRepository<>));

            // ------------------ هاشينج الباسورد ------------------
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            // ------------------ سيرفيس توكن الـ Reset Password ------------------
            builder.Services.AddDataProtection();
            builder.Services.AddScoped<AccountTokenService>();

            // ------------------ MVC ------------------
            builder.Services.AddControllersWithViews();

            // ------------------ Cookie Authentication (اللوجين/اللوج آوت) ------------------
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                });




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

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using POS_System_DotNet.Data;
using POS_System_DotNet.Services;


namespace POS_System_DotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(connectionString));

            // Add session services
            builder.Services.AddSession();

            builder.Services.AddScoped<IEmailService, EmailService>();

        

            
            // Add logging services
            builder.Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();  // Add Console logging
                    loggingBuilder.AddDebug();    // Add Debug logging
                });

               

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseStatusCodePagesWithRedirects("ErrorPage");
				app.UseDeveloperExceptionPage();
				
			}
            else
            {
				app.UseStatusCodePagesWithRedirects("ErrorPage");
				app.UseExceptionHandler("/ErrorPage");
			}

			app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Add session middleware
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

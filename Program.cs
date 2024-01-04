using Microsoft.EntityFrameworkCore;
using WebAplicacionTurnos.Data;

namespace WebAplicacionTurnos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Establece la conexión a la base de datos
            builder.Services.AddDbContext<ApplicationDbContext>(
                    options =>
                            options.UseSqlServer(
                                builder.Configuration.GetConnectionString("AgendaServiciosDbLocal")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
                pattern: "{controller=Turnos}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
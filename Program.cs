using eShopCKC.Infrastructure;
using eShopCKC.Services;
using Microsoft.EntityFrameworkCore;

namespace eShopCKC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<ICatalogService, CatalogService>();

            builder.Services.AddDbContext
                <CatalogContext>(
                    c =>
                    {
                        try
                        {
                            //c.UseSqlServer("Server=yourservername;Integrated Security=true;Initial Catalog=CatalogDbCKC;Encrypt=False;");
                            //c.UseNpgsql("Host=localhost;Port=5432;Database=CatalogDbCKC;Username=Postgres;Password=123456");
                            //c.UseNpgsql("Host=localhost;Port=5432;Database=CatalogDbCKC;Username=postgres;Password=123456;");
                            c.UseNpgsql("Host=crossover.proxy.rlwy.net;Port=32134;Database=CatalogDbCKC;Username=postgres;Password=XdgBCekZwQrXxmwHgnclOpZsdExzEWeP;");

                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            //app.MapStaticAssets();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalog}/{action=Index}/{id?}");
                //.WithStaticAssets();

            CatalogContextSeed.SeedAsync(app,
                app.Services.GetRequiredService<ILoggerFactory>())
                .Wait();

            app.Run();
        }
    }
}

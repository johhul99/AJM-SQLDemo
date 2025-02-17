
using Microsoft.EntityFrameworkCore;
using SQL_Demo.Models;

namespace SQL_Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SnusDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Add services to the container.
            builder.Services.AddAuthorization();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapPost("/snus", async (SnusDBContext db, Snus snus) =>
            {
                db.Snus.Add(snus);
                await db.SaveChangesAsync();

                return Results.Ok(snus);
            });

            app.MapGet("/snus", async (SnusDBContext db) =>
            {
                return Results.Ok(await db.Snus.ToListAsync());
            });

            app.MapGet("/snus/{id}", async (SnusDBContext db, int id) =>
            {
                var snus = await db.Snus.FindAsync(id);

                if (snus == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(snus);
            });

            app.MapPut("/snus/{id}", async (SnusDBContext db, Snus oldSnus) =>
            {
                var snus = await db.Snus.FindAsync(oldSnus.Id);

                if (snus == null)
                {
                    return Results.NotFound();
                }

                snus.Name = oldSnus.Name;
                snus.Brand = oldSnus.Brand;
                snus.Pris = oldSnus.Pris;
                snus.Quantity = oldSnus.Quantity;

                await db.SaveChangesAsync();

                return Results.Ok(snus);
            });


            app.MapDelete("/snus/{id}", async (SnusDBContext db, int id) =>
            {
                var snus = await db.Snus.FindAsync(id);

                if (snus == null)
                {
                    return Results.NotFound();
                }

                db.Snus.Remove(snus);
                await db.SaveChangesAsync();

                return Results.Ok("Snus deleted!");
            });

            app.Run();
        }
    }
}

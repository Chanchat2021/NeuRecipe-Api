using Microsoft.EntityFrameworkCore;
using NeuRecipe.API;
using NeuRecipe.Application.Services;
using NeuRecipe.Application.Services.Interfaces;
using NeuRecipe.Domain.Entity;
using NeuRecipe.Infrastructure;
using NeuRecipe.Infrastructure.Repositories;
using NeuRecipe.Infrastructure.Repositories.Interfaces;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = configuration.GetConnectionString("RecipeDBContext");
        builder.Services.AddDbContextPool<RecipeDBContext>(c =>
        {
            c.UseSqlServer(configuration["ConnectionStrings:RecipeDBContext"]);
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "_myAllowSpecificOrigins",
                              policy =>
                              {
                                  policy.AllowAnyMethod();
                                  policy.AllowAnyHeader();
                                  policy.WithOrigins("http://localhost:4200");
                              });
        });
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRecipeService, RecipeService>();
        builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
        builder.Services.AddScoped<IGenericRepository<Recipe>, GenericRepository<Recipe>>();
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseMiddleware<GlobalExceptionHandler>();
        app.UseAuthorization();
        app.UseCors(MyAllowSpecificOrigins);
        app.MapControllers();
        app.Run();
    }
}

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Warehouse.Middlewares;
using Warehouse.Repositories;
using Warehouse.Services;

namespace Warehouse;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddScoped<IProductService, ProductService>(); //register dependency
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Warehouse",
                Version = "v1",
                Description = "Rest API for managing Warehouse",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@example.com",
                    Url = new Uri("https://example/suppert")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseGlobalExceptionHandling(); 

        app.UseSwagger();

        //Enable middleware to serve swagger-ui
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel Agency API v1");

            //Basic UI Customization
            c.DocExpansion(DocExpansion.List);
            c.DefaultModelsExpandDepth(0); //Hide schemas section by default
            c.DisplayRequestDuration(); //Show request duration
            c.EnableFilter(); //Enable filtering operration
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        

        app.Run();
    }
}
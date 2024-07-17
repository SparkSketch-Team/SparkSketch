using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;

// add class Program and add Main funct
class Program
{

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        builder.Configuration.AddEnvironmentVariables();



        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        
        builder.Services.AddCors(options => {
            options.AddPolicy("AllowReactApp", builder => {
                builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
            });
        });

        var app = builder.Build();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowReactApp");
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors("AllowReactApp");
        app.UseRouting();


        app.UseAuthorization();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllers();
        app.MapControllers();

        app.Run();
    }
    
}


        app.Run();
    }
    
}



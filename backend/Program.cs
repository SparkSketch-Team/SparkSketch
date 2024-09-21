using System.Security.Cryptography.Xml;
using System.Text;
using backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

class Program
{

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(5227); // This makes Kestrel listen on all IP addresses (0.0.0.0)
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        builder.Configuration.AddEnvironmentVariables();

        // Register Services
        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

        // Use Dependency Injection for AccountEmailSender
        builder.Services.AddTransient<IAccountEmailSender, AccountEmailSender>();

        var jwtKey = builder.Configuration["Jwt:Key"];
        Console.WriteLine($"JWT Key: {(string.IsNullOrEmpty(jwtKey) ? "Empty or Null" : jwtKey)}");


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                    return Task.CompletedTask;
                }
            };
        });


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();




        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", builder =>
            {
                builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
            });
        });

        // Register Repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAiRepository, AiRepository>();
        builder.Services.AddScoped<ISketchRepository, SketchRepository>();
        builder.Services.AddScoped<ILikeRepository, LikeRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseCors("AllowReactApp");
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllers();

        app.Run();
    }

}



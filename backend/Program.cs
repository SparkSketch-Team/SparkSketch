using Microsoft.EntityFrameworkCore;

// add class Program and add Main funct

var builder = WebApplication.CreateBuilder(args);

/*
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets<Program>();

        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        try
        {
            // Don't break build if no keyvault access
            var keyVaultClient = new KeyVaultClient(
            new KeyVaultClient.AuthenticationCallback(
                azureServiceTokenProvider.KeyVaultTokenCallback));

            builder.Configuration.AddAzureKeyVault(
                $"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/",
                keyVaultClient,
                new DefaultKeyVaultSecretManager());
        }
        catch
        { }
*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//TODO: Security Risk, will change
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", builder => {
        builder.WithOrigins("https://lemon-ground-0ee28a610.5.azurestaticapps.net")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
    });
});

var app = builder.Build();

/*
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseRouting();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();

/*

    static void AddConnectionStringProvider(WebApplicationBuilder builder)
    {
        var databaseConfiguration = builder.Configuration
            .GetSection("DatabaseConfiguration")
            .Get<DatabaseConfiguration>();
        builder.Services.AddSingleton(c =>
            new ConnectionStringProvider(
                databaseConfiguration ?? new DatabaseConfiguration()));
    }

*/

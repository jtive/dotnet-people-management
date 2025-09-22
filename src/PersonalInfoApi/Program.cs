using Microsoft.EntityFrameworkCore;
using PersonalInfoShared.Data;
using PersonalInfoShared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Entity Framework with PostgreSQL
var connectionString = BuildConnectionString(builder.Configuration);

builder.Services.AddDbContext<PersonalInfoDbContext>(options =>
    options.UseNpgsql(connectionString));

static string BuildConnectionString(IConfiguration configuration)
{
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "show-tell.czgcu8i084mj.us-east-2.rds.amazonaws.com";
    var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "personalinfo";
    var username = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "postgres";
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "CqPkGGdn3HN2e0pQks8F";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
    
    return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;";
}

// Register services
builder.Services.AddScoped<IDataMaskingService, DataMaskingService>();
builder.Services.AddScoped<IMappingService, MappingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorApp");
app.UseAuthorization();
app.MapControllers();

// Ensure database is created and up to date
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PersonalInfoDbContext>();
    
    // For development, we'll recreate the database if there are schema issues
    if (app.Environment.IsDevelopment())
    {
        try
        {
            context.Database.EnsureCreated();
        }
        catch (Exception)
        {
            // If there's a schema conflict, drop and recreate
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
    else
    {
        context.Database.EnsureCreated();
    }
}

app.Run();

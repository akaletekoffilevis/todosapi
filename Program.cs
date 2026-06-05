using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodosApi.Data;
using TodosApi.Services;
using TodosApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
var logger = loggerFactory.CreateLogger("Startup");

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDbConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todos API - Secure Task Management",
        Version = "v1",
        Description = "A secure API for managing tasks with JWT authentication",
        Contact = new OpenApiContact
        {
            Name = "Todos Api Support",
            Email = "koffilevis21@gmail.com"
        }
    });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer <token>'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    opt.AddSecurityDefinition("Bearer", scheme);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        opt.IncludeXmlComments(xmlPath);
});

builder.Services.AddJwtAuthentication(builder.Configuration, logger);

var app = builder.Build();

// Apply migrations
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
    logger.LogInformation("Database migrated successfully");
}
catch (Exception ex)
{
    logger.LogError(ex, "Error migrating database");
    throw;
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Todos API";
        options.RoutePrefix = "swagger";
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

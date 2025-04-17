using DotNetEnv;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using NotesBE.Data;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging; // Add this using statement

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);


// Configure logging
builder.Logging.ClearProviders();
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddConsole(); // only in Development
}


// Build the SQL Server connection string dynamically using environment variables
string connectionString = $"Server={Env.GetString("SQL_SERVER")},{Env.GetString("SQL_PORT")};" +
                         $"Database={Env.GetString("SQL_DATABASE")};" +
                         $"User Id={Env.GetString("SQL_USER")};" +
                         $"Password={Env.GetString("SQL_PASSWORD")};" +
                         "TrustServerCertificate=True;";

// Load JWT configuration from environment variables
string jwtSecret = Env.GetString("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET is missing.");
string jwtIssuer = Env.GetString("JWT_ISSUER") ?? "NotesBE";
string jwtAudience = Env.GetString("JWT_AUDIENCE") ?? "NotesBEUsers";

builder.Services.AddControllers();

// Configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NotesBE",
        Version = "v1",
        Description = "API documentation for NotesApp backend"
    });

    // Add JWT bearer authentication to Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like 'Bearer {your token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Dependency Injection setup for repositories and database connection
builder.Services.AddScoped<NoteRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Exception, "Authentication failed");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Token validated");
                return Task.CompletedTask;
            }
        };

    });

// Configure CORS to allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Log incoming HTTP requests for debugging
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

    if (context.Request.Headers.ContainsKey("Authorization"))
    {
        logger.LogInformation($"Authorization Header: {context.Request.Headers["Authorization"]}");
    }

    await next();
});

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Test SQL Server connection during application startup
try
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Connected to SQL Server successfully!");
    }
}
catch (SqlException ex)
{
    Console.WriteLine($"Error connecting to SQL Server: {ex.Message}");
}

app.Run();
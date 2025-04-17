using DotNetEnv;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using NotesBE.Data;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace NotesBE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load environment variables
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddConsole();
            }

            // Build connection string from environment
            var sqlServer = Env.GetString("SQL_SERVER") ?? throw new Exception("SQL_SERVER missing");
            var sqlPort = Env.GetString("SQL_PORT") ?? "1433";
            var sqlDb = Env.GetString("SQL_DATABASE") ?? throw new Exception("SQL_DATABASE missing");
            var sqlUser = Env.GetString("SQL_USER") ?? throw new Exception("SQL_USER missing");
            var sqlPass = Env.GetString("SQL_PASSWORD") ?? throw new Exception("SQL_PASSWORD missing");

            var connectionString = $"Server={sqlServer},{sqlPort};Database={sqlDb};User Id={sqlUser};Password={sqlPass};TrustServerCertificate=True;";

            var jwtSecret = Env.GetString("JWT_SECRET") ?? throw new Exception("JWT_SECRET missing");
            var jwtIssuer = Env.GetString("JWT_ISSUER") ?? "NotesBE";
            var jwtAudience = Env.GetString("JWT_AUDIENCE") ?? "NotesBEUsers";

            // Register services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NotesBE",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}'"
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddScoped<NoteRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

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
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Middleware for logging
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    logger.LogInformation($"Auth Header: {context.Request.Headers["Authorization"]}");
                }

                await next();
            });

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

            // Try connection
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("✅ Connected to SQL Server.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ SQL Server connection failed: {ex.Message}");
            }

            app.Run();
        }
    }
}

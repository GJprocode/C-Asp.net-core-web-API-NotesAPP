// Program.cs

using DotNetEnv;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using NotesBE.Data;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

// load .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// logging
builder.Logging.ClearProviders();
if (builder.Environment.IsDevelopment())
    builder.Logging.AddConsole();

// build connection string
var cs = new SqlConnectionStringBuilder {
    DataSource           = $"{Env.GetString("SQL_SERVER")},{Env.GetString("SQL_PORT")}",
    InitialCatalog       = Env.GetString("SQL_DATABASE"),
    UserID               = Env.GetString("SQL_USER"),
    Password             = Env.GetString("SQL_PASSWORD"),
    TrustServerCertificate = true
}.ConnectionString;

// DEBUG: Log connection string without password
Console.WriteLine("🔍 Connecting with:");
Console.WriteLine($"  Server   : {Env.GetString("SQL_SERVER")}:{Env.GetString("SQL_PORT")}");
Console.WriteLine($"  DB       : {Env.GetString("SQL_DATABASE")}");
Console.WriteLine($"  User     : {Env.GetString("SQL_USER")}");


// JWT
string key      = Env.GetString("JWT_SECRET")  ?? throw new InvalidOperationException("Missing JWT_SECRET");
string issuer   = Env.GetString("JWT_ISSUER")  ?? "NotesBE";
string audience = Env.GetString("JWT_AUDIENCE") ?? "NotesBEUsers";

// services
builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(cs));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<NoteRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => {
  opts.SwaggerDoc("v1", new OpenApiInfo { Title = "NotesBE", Version = "v1" });
  opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    Type         = SecuritySchemeType.Http,
    Scheme       = "bearer",
    BearerFormat = "JWT",
    In           = ParameterLocation.Header,
    Description  = "JWT auth header"
  });
  opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
    [ new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
          Type = ReferenceType.SecurityScheme,
          Id   = "Bearer"
        }
      }
    ] = new string[]{}
  });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(o => {
         o.TokenValidationParameters = new TokenValidationParameters {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
           ValidateIssuer           = true,
           ValidIssuer              = issuer,
           ValidateAudience         = true,
           ValidAudience            = audience,
           ValidateLifetime         = true
         };
         o.Events = new JwtBearerEvents {
           OnAuthenticationFailed = ctx => {
             ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>()
               .LogError(ctx.Exception, "Auth failed");
             return Task.CompletedTask;
           },
           OnTokenValidated = ctx => {
             ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>()
               .LogInformation("Token valid");
             return Task.CompletedTask;
           }
         };
       });

builder.Services.AddCors(c => c.AddPolicy("AllowAll", p =>
  p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
));

var app = builder.Build();

// request logging
app.Use(async (ctx, next) => {
  var log = ctx.RequestServices.GetRequiredService<ILogger<Program>>();
  log.LogInformation($"{ctx.Request.Method} {ctx.Request.Path}");
  if (ctx.Request.Headers.ContainsKey("Authorization"))
    log.LogInformation($"Auth: {ctx.Request.Headers["Authorization"]}");
  await next();
});

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// smoke‑test the DB on startup
try {
  using var conn = new SqlConnection(cs);
  conn.Open();
  Console.WriteLine("✅ SQL Connected!");
}
catch (Exception ex) {
  Console.WriteLine($"❌ SQL failed: {ex.Message}");
}

app.Run();

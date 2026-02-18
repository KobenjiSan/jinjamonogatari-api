using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Persistence.Seeding;

using MediatR;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Users;
using Application.Features.Users.Services;
using Application.Common.Interfaces;
using Application.Features.Etiquette.Services;
using Infrastructure.Services.Etiquette;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services
// --------------------

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR (scan Application assembly for handlers)
builder.Services.AddMediatR(typeof(Application.Common.Interfaces.IAppDbContext).Assembly);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// CORS (dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true)); // dev-only
});

// JWT Settings
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);
var jwtSettings = jwtSection.Get<JwtSettings>()!;

// AuthN/AuthZ
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(1),

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

// Infrastructure services
// Auth
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
// Users
builder.Services.AddScoped<IUserReadService, UserReadService>();
builder.Services.AddScoped<IUserWriteService, UserWriteService>();
// Etiquette
builder.Services.AddScoped<IEtiquetteReadService, EtiquetteReadService>();


var app = builder.Build();

// --------------------
// Middleware pipeline
// --------------------

app.UseMiddleware<Api.Middleware.ExceptionHandlingMiddleware>();

app.UseCors("DevCors");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed (dev only)
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DevDataSeeder.SeedAsync(db);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

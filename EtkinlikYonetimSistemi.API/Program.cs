using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Application.Services;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using EtkinlikYonetimSistemi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // CORS politikası ekleniyor
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        // EventDbContext servise ekleniyor
        builder.Services.AddDbContext<EventDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // JWT Authentication
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key değeri eksik!")
                        )
                    )
                };
            });

        builder.Services.AddAuthorization();

        // Servisler
        builder.Services.AddScoped<IEtkinlikService, EtkinlikService>();
        builder.Services.AddScoped<IEtkinlikRepository, EtkinlikRepository>();
        builder.Services.AddScoped<IHavaDurumuService, HavaDurumuService>();
        builder.Services.AddScoped<IIlgiAlaniRepository, IlgiAlaniRepository>();
        builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
        builder.Services.AddScoped<IKullaniciService, KullaniciService>();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.UseHttpsRedirection();

        // CORS middleware'i ekleniyor
        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

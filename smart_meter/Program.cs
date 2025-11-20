using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using smart_meter.Data.Context;
using smart_meter.Services;
using smart_meter.Worker;
using System.Text;

namespace smart_meter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            // Add custom jwt and add db context
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            
            // This enables proper mapping for DateOnly and TimeOnly types
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ConsumerService>();
            builder.Services.AddScoped<TariffService>();
            builder.Services.AddScoped<OrgunitService>();
            builder.Services.AddScoped<MeterService>();
            builder.Services.AddScoped<HistoricalConsumptionService>();
            builder.Services.AddScoped<UserServices>();
            builder.Services.AddScoped<MeterReadingServices>();
            builder.Services.AddScoped<BillService>();
            builder.Services.AddScoped<DatabaseService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure jwt authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });

            // Add RabbitMQ Connection
            builder.Services.AddSingleton(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(config["RabbitMq:Connection"])
                };
                return factory.CreateConnectionAsync();
            });

            // Add the new BackgroundService
            // This will start the listener
            builder.Services.AddHostedService<ReadingListenerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

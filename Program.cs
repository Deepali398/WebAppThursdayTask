using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAppThursdayTask.Data;
using WebAppThursdayTask.Repositories;
using WebAppThursdayTask.Services;

namespace WebAppThursdayTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
           


            //Swagger settings to authorise api
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DataItemAPI", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT" // Optional
                };

                var securityRequirement = new OpenApiSecurityRequirement
                {
                  {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    new string[] {}
                  }
                };

                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

            });

            //add rsponce caching for every reponce from server
            builder.Services.AddResponseCaching();

            //Adding Database to services (Dependency Injection for database service)
            builder.Services.AddDbContext<DataItemDbContext>(options => options.UseSqlServer
                        (builder.Configuration.GetConnectionString("DefaultConnection")));


            //Scope is available for every request made.
            builder.Services.AddScoped<IDataItem, DataItemRepo>();
            builder.Services.AddScoped<IUser, UserRepo>();
            builder.Services.AddScoped<IAuthenticate, AuthenticateRepo>();


            //JWT Bearer Token Service settings
             builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication : Issuer"],
                    ValidAudience = builder.Configuration["Authentication : Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("kjdkjsddfnsnsdnfsnsdlfjsdfsdlnfsdnndsn")),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

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
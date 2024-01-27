
using Assignment_2.Contexts;
using Assignment_2.Entities;
using Assignment_2.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Assignment_2
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
            builder.Services.AddSwaggerGen();

            #region Custom Services 
            // Context
            builder.Services.AddDbContext<ITIDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
                // this line to solve this error =>Error In Update Record (The instance of entity type 'x' cannot be tracked) 
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ITIDbContext>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // CORS Settings
            var policy = "myPolicy";
            builder.Services.AddCors(corsOptions =>
            {
                //corsOptions.AddPolicy("myPolicy", corsPolicyBuilder =>
                corsOptions.AddPolicy(name : policy , corsPolicyBuilder => 
                    //corsPolicyBuilder.WithOrigins("http://example.com",
                                    //          "http://www.contoso.com");
                corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // [Authorize] Settings To Make Authentication Middleware Use JWT Token For Checking
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // valid or return unauthorized
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // valid or redirect to login
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>       
            {
                options.SaveToken = true; // time (not expired )
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()  // custom validation
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = 
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            });

                #region Swagger Authorization Button
            builder.Services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
            });
            #endregion

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(); // This Middelware Allow Request To Access Static Files (any with extention) => in wwwroot

            // CORS Policy Setting
            app.UseCors("myPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication(); // by default check cookies 

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
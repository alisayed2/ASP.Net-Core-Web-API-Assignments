
using Assignment_2.Contexts;
using Assignment_2.Repositories;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddDbContext<ITIDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
                // this line to solve this error =>Error In Update Record (The instance of entity type 'x' cannot be tracked) 
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            var policy = "myPolicy";
            builder.Services.AddCors(corsOptions =>
            {
                //corsOptions.AddPolicy("myPolicy", corsPolicyBuilder =>
                corsOptions.AddPolicy(name : policy , corsPolicyBuilder => 
                    //corsPolicyBuilder.WithOrigins("http://example.com",
                                    //          "http://www.contoso.com");
                corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
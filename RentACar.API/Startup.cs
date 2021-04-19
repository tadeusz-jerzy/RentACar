using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentACar.Infrastructure;
using Swashbuckle.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RentACar.API;

namespace RentACar
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            services.AddMvcCore()
                .AddDataAnnotations();

            services.AddMyServices(Configuration);

            /*optionsBuilder.UseInMemoryDatabase("new12").LogTo(
                Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name//,
                                                 //DbLoggerCategory.Database.Transaction.Name
                                               },
                LogLevel.Information)
                .EnableSensitiveDataLogging(); */
            services.AddMyDbContexts(Configuration);
            
            services.AddDbContext<MyAppContext>(o => o.UseInMemoryDatabase("RentACar_Main3"));

            services.AddTransient<ISeed, Seed>();

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            try
            {
                seed.Create();
            }
            catch (Exception e) { }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

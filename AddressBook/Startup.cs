using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.Data;
using AddressBook.Data.Repositories;
using AddressBook.Services.Automapper;
using AddressBook.Services.Extensions;
using AddressBook.Services.Services;
using AddressBook.Services.Services.Custom;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace AddressBook.Api
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
            services.AddDbContext<Context>(ServiceLifetime.Scoped);
            services.AddScoped(typeof(Repository<>));
            services.AddScoped<ContactService>();
            services.AddScoped<ColumnMappingService>();

            services.AddHttpContextAccessor();
            services.AddMvc(config =>
                {
                    config.Filters.Add(typeof(Filters.GenericExceptionFilterAttribute));
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //added as an example if we have loop reference situations, so our JSON don't go crazy
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // I like adding Swagger to any API I'm making. I know it was not asked in the description but I'm guessing it wont hurt.
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new Info { Title = "AddressBook API", Version = "v1" });
              
            });
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {

                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("DefaultPolicy");
            AutomapperConfig.RegisterMappings();
            app.UseSwagger();
            app.ConfigureExceptionHandler();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressBook API V1");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{id?}");

            });
        }
    }
}

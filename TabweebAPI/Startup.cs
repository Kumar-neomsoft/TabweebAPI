using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace TabweebAPI
{
    public class Startup
    {
        private static readonly string MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();

        }

        public IConfiguration Configuration { get; set; }

        public static string ConnectionString { get; private set; }
        public static string SysConnectionString { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TabweebAPI", Version = "v1" });
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            ConnectionString = Configuration.GetConnectionString("DBConnection");
            SysConnectionString = Configuration.GetConnectionString("DBSysConnection");
            if (env.IsDevelopment() || env.IsProduction() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TabweebAPI v1"));
            }
            //app.UseCors(MyAllowSpecificOrigins);
            //app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseRouting();
            // app.UseCors("CorsApi");

            app.UseAuthorization();

            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

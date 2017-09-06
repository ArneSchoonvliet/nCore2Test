using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Digipolis.Web;
using Digipolis.DataAccess;
using nCore2Test.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;

namespace nCore2Test
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
            services.AddDataAccess<MyEntityContext>();

            var connectionStr = GetConnectionString();

            services.AddDbContext<MyEntityContext>(options =>
            {
                options.UseNpgsql(connectionStr, opt => opt.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "main"));
                options.ConfigureWarnings(config => config.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });

            services.AddMvc();

            //var pathToDoc = Configuration["Swagger:FileName"];
            services.AddSwaggerGen((a) => { });
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Geo Search API",
                        Version = "v1",
                        Description = "A simple api to search using geo location in Elasticsearch",
                        TermsOfService = "None"
                    }
                 );

                //var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, pathToDoc);
                //options.IncludeXmlComments(filePath);
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });

            //Redirect to swagger UI.
            app.UseSwaggerUiRedirect();
        }

        private string GetConnectionString()
        {
            var configSection = Configuration.GetSection("DataAccess").GetSection("ConnectionString");

            var host = configSection.GetValue<string>("Host");
            var dbname = configSection.GetValue<string>("DbName");
            var user = configSection.GetValue<string>("User");
            var password = configSection.GetValue<string>("Password");

            ushort port = 0;
            try
            {
                port = configSection.GetValue<ushort>("Port");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Database port must be a number from 0 to 65536.", ex.InnerException ?? ex);
            }

            var connectionString = new ConnectionString(host, port, dbname, user, password);
            return connectionString.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using prs2server.Models;

namespace prs2server {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            services.AddDbContext<Prs2DbContext>(option => {
                option.UseLazyLoadingProxies();
                option.UseSqlServer(Configuration.GetConnectionString("Prs2DbContext"));
                //option.UseSqlServer(Configuration.GetConnectionString("Winhost"));
            });

            services.AddCors();

            //var secret = Configuration["Application:codeword"];
            //Configuration["Test"];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            using var scope = app.ApplicationServices
                                    .GetRequiredService<IServiceScopeFactory>()
                                    .CreateScope();
            scope.ServiceProvider.GetService<Prs2DbContext>()
                                    .Database.Migrate();
            

        }
    }
}

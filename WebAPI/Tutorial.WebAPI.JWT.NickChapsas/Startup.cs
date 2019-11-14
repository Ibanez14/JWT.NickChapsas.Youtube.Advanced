using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Tutorial.WebAPI.JWT.NickChapsas.Options;
using Tutorial.WebAPI.JWT.NickChapsas.Service_Installer;
using MySwaggerOptions = Tutorial.WebAPI.JWT.NickChapsas.Options.MySwaggerOptions;

namespace Tutorial.WebAPI.JWT.NickChapsas
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
            services.InstallServicesAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseAuthentication();

            #region Configure Swagger   

            var swaggerOptions = Configuration.GetSection("SwaggerOptions")
                                                           .Get<MySwaggerOptions>();

            app.UseSwagger();
            app.UseSwaggerUI(ops =>
                             ops.SwaggerEndpoint(url: swaggerOptions.UIEndpoint, 
                                                name: swaggerOptions.Description));
            #endregion

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

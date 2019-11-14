using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer
{
    public class SwaggerInstaller : IInstaller // Interface has only one method InstallService
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new Info { Title = "Swagger Reference Book" });


                #region Jwt Authenttication
                var securityDictionary = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Array.Empty<string>()}
                };

                ops.AddSecurityDefinition(name: "Bearer", // Must be BEARER! nothing else
                               securityScheme: new ApiKeyScheme
                               {
                                   Description = "JWT Authorization Header using bearer scheme",
                                   Name = "Authorization",
                                   In = "header",
                                   Type = "apiKey" // Must be apiKey, not ApiKey, apikey or other
                               });

                ops.AddSecurityRequirement(securityDictionary);

                #endregion
            });




        }
    }

}

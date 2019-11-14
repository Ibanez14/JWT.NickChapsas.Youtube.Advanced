using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorial.WebAPI.JWT.NickChapsas.Authorization;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer.Concrete
{
    public class PolicyInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration config)
        {
            services.AddAuthorization(authorizationOps =>
            {
                authorizationOps.AddPolicy("View",         policy => policy.RequireClaim("tags.view", "true"));
                authorizationOps.AddPolicy("ViewAndWrite", policy => policy.RequireClaim("Tag", "ViewWrite"));

                // I havent applied thie policy at any controller
                authorizationOps.AddPolicy("WorkForMicrosoft",

                                policy => policy.Requirements.Add(new WorForCompanyRequirement("microsoft.com")));
            });

            services.AddSingleton<IAuthorizationHandler, WorkForCompanyHandler>();
        }
    }
}

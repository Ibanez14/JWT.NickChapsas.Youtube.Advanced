using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tutorial.WebAPI.JWT.NickChapsas.Controllers;
using Tutorial.WebAPI.JWT.NickChapsas.Options;
using Tutorial.WebAPI.JWT.NickChapsas.Services;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer
{
    public class MvcandOtherServicesInstaller : IInstaller // Interface has only one method InstallService
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tutorial.WebAPI.JWT.NickChapsas.Options;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer.Concrete
{
    public class JwtAuthenticationInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration config)
        {
            var jwtOptions = config.GetSection("JwtOptions").Get<JwtOptions>();
           

            var authenticationBuilder =
            services.AddAuthentication(ops =>
            {
                ops.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                ops.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            var tokenvalidationPArameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };

            services.AddSingleton(tokenvalidationPArameters);
            services.AddSingleton(jwtOptions);

            authenticationBuilder.AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenvalidationPArameters;
            });

        }
    }
}

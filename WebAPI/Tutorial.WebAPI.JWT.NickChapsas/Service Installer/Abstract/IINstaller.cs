using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer
{
    public interface IInstaller
    {
        void InstallService(IServiceCollection services, IConfiguration config);
    }
}

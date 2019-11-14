using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer
{
    public static class InstallerExtensions
    {
        public static void InstallServicesAssembly(this IServiceCollection services, IConfiguration Configuration)
        {
            // Types that implement IInstaller
            IEnumerable<Type> types =
                typeof(Startup).Assembly.GetExportedTypes().Where(type => typeof(IInstaller).IsAssignableFrom(type)
                                                                          && !type.IsInterface  // is not interface
                                                                          && !type.IsAbstract); // is not abstract

            // Creating instances of these types
            IEnumerable<object> objects =
                types.Select(type => Activator.CreateInstance(type));

            // Casting objects to IInstaller
            IEnumerable<IInstaller> installers = objects.Cast<IInstaller>();

            // Install the service
            installers.ToList().ForEach(installer => installer.InstallService(services, Configuration));
        }
    }
}

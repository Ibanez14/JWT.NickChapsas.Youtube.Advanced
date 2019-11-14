using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorial.WebAPI.JWT.NickChapsas.Data;

namespace Tutorial.WebAPI.JWT.NickChapsas.Service_Installer.Concrete
{
    public class DataService : IInstaller
    {
        public void InstallService(IServiceCollection servicecollection, IConfiguration config)
        {
            servicecollection.AddDbContext<DataContext>(ops =>
            {
                ops.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NickPchpasas;Integrated Security=True;");
            });

            servicecollection.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}

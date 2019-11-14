using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.WebAPI.JWT.NickChapsas.Domain;
using Tutorial.WebAPI.JWT.NickChapsas.Models;

namespace Tutorial.WebAPI.JWT.NickChapsas.Data
{
    public class DataContext:IdentityDbContext
    { 
        public DataContext(DbContextOptions<DataContext> ops):base(ops)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Book> Books{ get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

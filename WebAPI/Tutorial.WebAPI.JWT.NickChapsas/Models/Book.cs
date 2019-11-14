using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Tutorial.WebAPI.JWT.NickChapsas.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
    }
}

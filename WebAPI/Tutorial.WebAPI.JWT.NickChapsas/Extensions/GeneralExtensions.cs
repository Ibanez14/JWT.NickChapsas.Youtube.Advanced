using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tutorial.WebAPI.JWT.NickChapsas.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            if (context.User == null) return string.Empty;

            

            // we instantiate id claim when generating jwt (see IdentitySerivce)
            return context.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
 }

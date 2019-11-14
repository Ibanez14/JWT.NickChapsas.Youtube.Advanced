using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.WebAPI.JWT.NickChapsas.Contract.V1.Request
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

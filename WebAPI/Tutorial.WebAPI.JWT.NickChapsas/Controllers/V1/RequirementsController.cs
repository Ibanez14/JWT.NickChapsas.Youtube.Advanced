using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tutorial.WebAPI.JWT.NickChapsas.Controllers.V1
{
    /// <summary>
    /// Thic controller is created for checking authroziationHAndlers
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequirementsController : Controller
    {

        [HttpGet]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Ok("Handler Words");
        }
    }
}

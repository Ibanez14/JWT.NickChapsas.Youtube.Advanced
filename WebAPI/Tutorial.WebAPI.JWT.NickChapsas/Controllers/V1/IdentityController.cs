using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tutorial.WebAPI.JWT.NickChapsas.Contract;
using Tutorial.WebAPI.JWT.NickChapsas.Contract.V1.Request;
using Tutorial.WebAPI.JWT.NickChapsas.Contract.V1.Responses;
using Tutorial.WebAPI.JWT.NickChapsas.Domain;
using Tutorial.WebAPI.JWT.NickChapsas.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tutorial.WebAPI.JWT.NickChapsas.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }


        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] RegistrationModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany
                                                    (x => x.Errors.Select(xx=>xx.ErrorMessage)));

            AuthenticationResult result = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!result.Success)
                return BadRequest(new AuthFailedResponse(result.Errors));
            else
                return Ok(new AuthSuccessResponse(result.Token, result.RefreshToken));
        }



        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany
                                                    (x => x.Errors.Select(xx => xx.ErrorMessage)));

            AuthenticationResult result = await _identityService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
                return BadRequest(new AuthFailedResponse(result.Errors));
            else
                return Ok(new AuthSuccessResponse(result.Token, result.RefreshToken));
        }

        
        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany
                                                    (x => x.Errors.Select(xx => xx.ErrorMessage)));

            AuthenticationResult result = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!result.Success)
                return BadRequest(new AuthFailedResponse(result.Errors));
            else
                return Ok(new AuthSuccessResponse(result.Token, result.RefreshToken));
        }
    }
}
 
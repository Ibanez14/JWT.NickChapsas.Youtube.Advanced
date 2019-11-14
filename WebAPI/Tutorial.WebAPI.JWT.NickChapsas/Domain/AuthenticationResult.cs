using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.WebAPI.JWT.NickChapsas.Domain
{
    public class AuthenticationResult
    {

        // CTORS
        public AuthenticationResult()
        {

        }
        public AuthenticationResult(IEnumerable<string> vs )
        {
            this.Errors = vs;
        }
        public AuthenticationResult(string token, bool success, IEnumerable<string> errors) =>
            (Token, Success, Errors) = (token, success, errors);

        public AuthenticationResult(string token, string refreshToken, bool success, IEnumerable<string> errors)
                                 : this(token, success, errors) 
            => RefreshToken = refreshToken;
        

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success  { get; set; }
        public IEnumerable<string> Errors { get; set; }


        public static AuthenticationResult FailResult(params string [] errors)
            => new AuthenticationResult(errors);

        public static AuthenticationResult FailResult(IEnumerable<string> errors)
            => new AuthenticationResult(errors);

        public static AuthenticationResult SuccessResult(string jwt)
            => new AuthenticationResult(jwt, true, null);

        public static AuthenticationResult SuccessResult(string jwt, string refreshtoken)
            => new AuthenticationResult(jwt, refreshtoken, true, null);


    }
}

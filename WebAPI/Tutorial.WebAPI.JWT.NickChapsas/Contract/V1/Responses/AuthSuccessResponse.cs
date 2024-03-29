﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.WebAPI.JWT.NickChapsas.Contract.V1.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        // CTOR
        public AuthSuccessResponse(string token, string refreshToken)
        => (Token, RefreshToken) = (token, refreshToken);
    }

    public class AuthFailedResponse
    {
        // CTOR
        public AuthFailedResponse(IEnumerable<string> errors) => Errors = errors;
        
        public IEnumerable<string> Errors { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tutorial.WebAPI.JWT.NickChapsas.Data;
using Tutorial.WebAPI.JWT.NickChapsas.Domain;
using Tutorial.WebAPI.JWT.NickChapsas.Options;

namespace Tutorial.WebAPI.JWT.NickChapsas.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }




    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtOptions jwtOptions;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly DataContext context;

        public IdentityService(UserManager<IdentityUser> userManager,
                                JwtOptions jwtOptions,
                                TokenValidationParameters tokenValidationParameters,
                                DataContext context)
        {
            this.userManager = userManager;
            this.jwtOptions = jwtOptions;
            this.tokenValidationParameters = tokenValidationParameters;
            this.context = context;
        }


        // LOGIN
        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return AuthenticationResult.FailResult("User doesnt exist");

            bool isPasswordValid = await userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
                return AuthenticationResult.FailResult("User/Password combination is wrong");

            return await GenerateAuthenticationResultForUser(user);
        }


        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            ClaimsPrincipal principal = GetPrincipalFromToken(token);

            if (principal == null)
                return AuthenticationResult.FailResult("Invalid Token");

            var unix_expiration_dateOfToken = long.Parse(principal.Claims.Single
                                                  (x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var utc_expiration_date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                                   .AddSeconds(unix_expiration_dateOfToken)
                                                   .Subtract(jwtOptions.TokenLifeTime);

            if (utc_expiration_date > DateTime.UtcNow)
                return AuthenticationResult.FailResult("This token hasn't expired yet");

            var jti = principal.Claims.Single
                     (x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            RefreshToken stored_refresh_token = await
                context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (stored_refresh_token == null)
                return AuthenticationResult.FailResult("Hey, Refresh Token doesnt exixst");

            // if refresh token expired
            if (DateTime.UtcNow > stored_refresh_token.ExpirationDate)
                return AuthenticationResult.FailResult("This refresh token has expired");

            if (stored_refresh_token.Invalidated)
                return AuthenticationResult.FailResult("This refresh token has been invalidated");

            if (stored_refresh_token.IsUsed)
                return AuthenticationResult.FailResult("This refresh token has been used");

            if (stored_refresh_token.JwtId != jti)
                return AuthenticationResult.FailResult("This refresh token doesn't match JTI");

            stored_refresh_token.IsUsed = true;
            context.RefreshTokens.Update(stored_refresh_token);
            await context.SaveChangesAsync();

            var user = await
            userManager.FindByIdAsync(principal.Claims.Single(x => x.Type == "id").Value);

            return await GenerateAuthenticationResultForUser(user);


            #region Local Methods

            // local methods
            ClaimsPrincipal GetPrincipalFromToken(string _token)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                try
                {
                    ClaimsPrincipal _principal =
                        jwtHandler.ValidateToken(_token, tokenValidationParameters, out SecurityToken validatedToken);

                    if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                        return null;

                    return _principal;
                }
                catch (Exception)
                {
                    return null;
                }

                // local methods
                bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
                {
                    return (validatedToken is JwtSecurityToken jwt)
                        && jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture);
                }
            }
            #endregion
        }
         
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var newUser = await userManager.FindByEmailAsync(email);
            if (newUser != null) // If user exist
                return AuthenticationResult.FailResult("User alredy exist");

            var newUserId = Guid.NewGuid().ToString();
            newUser = new IdentityUser() { Id = newUserId, Email = email, UserName = email };
            var creationResult = await userManager.CreateAsync
                                                (user: newUser,
                                                password: password);

            // Add this claim into the database
            await userManager.AddClaimAsync(newUser, new Claim("tags.view", "true"));

            if (!creationResult.Succeeded)
                return AuthenticationResult.FailResult(creationResult.Errors.Select(r => r.Description));

            return await GenerateAuthenticationResultForUser(newUser);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUser(IdentityUser user)
        {

            var jwtClaims = new List<Claim>()
            {

                                             new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                             new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                             new Claim("id", user.Id)
            };
            var userClaims = await userManager.GetClaimsAsync(user);
            // Merge jwtClaims and userClaims to add them all in token
            jwtClaims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(jwtClaims),
                Expires = DateTime.UtcNow.Add(jwtOptions.TokenLifeTime),
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                                                      algorithm: SecurityAlgorithms.HmacSha256),
                Audience = "Barratson",
                Issuer = "ChapsasAPI",
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id, 
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(6)
            };

            await context.RefreshTokens.AddAsync(refreshToken);
            // !!!!
            // Or you can use JwtSecurityToken with all claims and SignInCredentials in it and pass it in below method
            string jwt = handler.WriteToken(token);
            return AuthenticationResult.SuccessResult(jwt, refreshToken.Token);
        }
    }
}

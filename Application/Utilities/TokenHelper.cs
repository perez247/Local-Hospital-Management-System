using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class TokenHelper
    {
        public static string generateUserToken(AppUser userTokenData, bool rememberMe = false)
        {

            // Create token an sent;
            var claims = defaultClaim(userTokenData);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(EnvironmentFunctions.AUTHORIZATION_TOKEN));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = rememberMe ? RememberMeDescriptor(claims, creds) : defaultDescriptor(claims, creds);

            if (EnvironmentFunctions.isEnv("Development"))
                tokenDescriptor.Expires = DateTime.Now.AddDays(365);

            var tokenhandler = new JwtSecurityTokenHandler() { };

            var token = tokenhandler.CreateToken(tokenDescriptor);

            // var data = new {token = tokenhandler.WriteToken(token)};

            return tokenhandler.WriteToken(token);
        }

        /// <summary>
        /// Default token with a short timeframe because no remember me was enabled
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="creds"></param>
        /// <returns></returns>
        public static SecurityTokenDescriptor defaultDescriptor(List<Claim> claims, SigningCredentials creds)
        {
            // Set date of nbf to now
            return new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                NotBefore = DateTime.Now.AddMinutes(-5),
                IssuedAt = DateTime.Now,
                SigningCredentials = creds,
            };
        }

        /// <summary>
        /// Longer timeframe token because remember me was enabled
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="creds"></param>
        /// <returns></returns>
        public static SecurityTokenDescriptor RememberMeDescriptor(List<Claim> claims, SigningCredentials creds)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(90),
                NotBefore = DateTime.Now.AddMinutes(-5),
                IssuedAt = DateTime.Now,
                SigningCredentials = creds,
            };
        }

        /// <summary>
        /// Get the user of the instance/request
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static string GetUserId(ClaimsPrincipal User)
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private static List<Claim> defaultClaim(AppUser userTokenData)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userTokenData.Id.ToString()),
                // new Claim(AppClaimTypes.profilePicture.ToString(), userTokenData.ProfilePicture),
                // new Claim(AppClaimTypes.emojiPicture.ToString(), userTokenData.EmojiPicture)
            };



            foreach (var item in userTokenData.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
            }

            return claims;
        }


    }
}

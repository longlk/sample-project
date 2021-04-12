using AdventureWorks_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AdventureWorks_API
{
    public static class JWTHelper
    {
        private static IEnumerable<Claim> GetAdminClaims(AuthenticationResponse model)
        {
            IEnumerable<Claim> claims = new Claim[]
                    {
                new Claim(ClaimDataPayload.UserId, model.Id.ToString()),
                new Claim(ClaimTypes.Name, model.UserName),
                //new Claim(ClaimDataPayload.DisplayName, model.FullName),
                new Claim(ClaimDataPayload.EmailAddress, model.Email),
               // new Claim(ClaimDataPayload.GuidId, model.GuidId),
                new Claim(ClaimDataPayload.ExpiredTime, model.ExpiredTime?.ToString("dd/MM/yyyy HH:mm:ss.fff"))
                    };
            return claims;
        }
        public static DateTime AsDateTimeExac(this object obj, DateTime defaultValue = default(DateTime))
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return defaultValue;

            DateTime result;
            if (!DateTime.TryParseExact(obj.ToString(), "dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return defaultValue;

            return result;
        }
        public static string GenTokenkey(AuthenticationResponse model, int? expiredTimeInminutes = 0)
        {
            try
            {
                if (model == null) return null;

                // Get secret key
                var key = Encoding.ASCII.GetBytes("9135176d-94830-456b-aff8-3dsf7b85b05f26");

                // Get expires time
                expiredTimeInminutes = !expiredTimeInminutes.HasValue || expiredTimeInminutes == 0 ? 2: expiredTimeInminutes; // 120 = 2h
                DateTime expireTime = DateTime.Now.AddMinutes(expiredTimeInminutes.Value);
                model.ExpiredTime = expireTime;

                // Generate new guid string help determine user login
                if (string.IsNullOrEmpty(model.GuidId)) model.GuidId = System.Guid.NewGuid().ToString();

                //Generate Token for user 
                var JWToken = new JwtSecurityToken(
                    issuer: "https://quizdeveloper.com",
                    audience: "https://quizdeveloper.com",
                    claims: GetAdminClaims(model),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expireTime).DateTime,
                    //Using HS256 Algorithm to encrypt Token  
                    signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static AuthenticationResponse ExtracToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return null;
                var key = Encoding.ASCII.GetBytes("9135176d-94830-456b-aff8-3dsf7b85b05f26");
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "https://quizdeveloper.com",
                    ValidateAudience = true,
                    ValidAudience = "https://quizdeveloper.com",
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string adminId = (jwtToken.Claims.First(x => x.Type.Contains("userId")).Value);
                //string userName = jwtToken.Claims.First(x => x.Type.Contains("name")).Value;
                string displayName = jwtToken.Claims.First(x => x.Type.Contains("displayName")).Value;
                string emailaddress = jwtToken.Claims.First(x => x.Type.Contains("emailAddress")).Value;
                string guidId = jwtToken.Claims.First(x => x.Type.Contains("guidId")).Value;
                DateTime expiredTime = (jwtToken.Claims.First(x => x.Type.Contains("expiredTime")).Value).AsDateTimeExac(DateTime.Now);

                return new AuthenticationResponse()
                {
                    Id = adminId,
                    //UserName = userName,
                    FullName = displayName,
                    Email = emailaddress,
                    ExpiredTime = expiredTime,
                    Token = token,
                    GuidId = guidId,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

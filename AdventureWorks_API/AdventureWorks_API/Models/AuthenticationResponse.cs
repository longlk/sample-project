using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks_API.Models
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse()
        {

        }
        public AuthenticationResponse(string id, string userName, string email, DateTime? expriedTime, string token, string refreshToken)
        {
            Id = id;
            UserName = userName;
          //  FullName = fullName;
            Token = token;
            Email = email;
            ExpiredTime = expriedTime;
            RefreshToken = refreshToken;
            GuidId = System.Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string GuidId { get; set; }
        public DateTime? ExpiredTime { get; set; }
    }
}

using AdventureWorks_API.Areas.Identity.Data;
using AdventureWorks_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdventureWorks_API.Admin
{
    public class AdminBO : IAdminBO
    {
        private readonly AdventureWorksContext _context;
     
        public AdminBO(AdventureWorksContext context
            )
        {
            _context = context;
     
        }

        AuthenticationResponse IAdminBO.Authentication(AdventureWorks_APIUser userLogin)
        {
            try
            {
                var responAdmin = new AuthenticationResponse()
                {
                    Id = userLogin.Id.ToString(),
                    Email =userLogin.Email,
                    UserName =userLogin.UserName
                };

                string token = JWTHelper.GenTokenkey(responAdmin);

                responAdmin.GuidId = System.Guid.NewGuid().ToString();
                string newRefreshToken = JWTHelper.GenTokenkey(responAdmin, 10800);

                return new AuthenticationResponse(userLogin.Id, userLogin.UserName,userLogin.Email ,responAdmin.ExpiredTime, token, newRefreshToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        ResponseAPI IAdminBO.RefeshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}

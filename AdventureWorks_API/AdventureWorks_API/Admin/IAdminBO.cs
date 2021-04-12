using AdventureWorks_API.Areas.Identity.Data;
using AdventureWorks_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks_API.Admin
{
    public interface IAdminBO
    {
        // Help check authentication
        AuthenticationResponse Authentication(AdventureWorks_APIUser user);

        // Help refresh token
        ResponseAPI RefeshToken(string refreshToken);
    }
}

using AdventureWorks_API.Admin;
using AdventureWorks_API.Areas.Identity.Data;
using AdventureWorks_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks_API.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AdventureWorks_APIUser> _userManager;
        private readonly SignInManager<AdventureWorks_APIUser> _signInManager;
        private readonly IAdminBO _adminBO;

        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(UserManager<AdventureWorks_APIUser> userManager,
                                        SignInManager<AdventureWorks_APIUser> signInManager,
                                        IAdminBO adminBO, ILogger<AuthenticationController> logger
            
                                       )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _adminBO = adminBO;
            _logger = logger;

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] LoginModel myLoginModel)
        {

            var userRegister = new AdventureWorks_APIUser()
            {
                Email = myLoginModel.Email,
                UserName = myLoginModel.Email,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(userRegister, myLoginModel.Password);
            if (result.Succeeded)
            {
                return Ok(new { Result = "Register sucessfully" });
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    stringBuilder.Append(error.Description);
                    stringBuilder.Append("/n/r");
                }
                return Ok(new { Result = $"Register fail: {stringBuilder.ToString()}" });
            }
        }

        /// <summary>
        /// check account existing.
        /// then generate token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody] LoginModel authenticationAccount)
        {
            try
            {


                if (authenticationAccount == null || string.IsNullOrEmpty(authenticationAccount.Email) || string.IsNullOrEmpty(authenticationAccount.Password))
                {
                    return Ok(new ResponseAPI() { IsError = false, Message = "Email and Password are required." });
                }

                //only check this user need to sign in first

                var isSignIn = _signInManager.IsSignedIn(this.User);
                if (isSignIn)
                {
                    await _signInManager.SignOutAsync();
                }
                var result = await _signInManager.PasswordSignInAsync(authenticationAccount.Email, authenticationAccount.Password, true, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    return Ok(new ResponseAPI() { IsError = false, Message = "We need to login first." });
                }
                //checking User is existing


                //AdventureWorks_APIUser userLogin = await _userManager.GetUserAsync(HttpContext.User);
                //after _signInManager.PasswordSignInAsync , it still not sign in so that why userLogin is null.See below.
                //we need to do the steps in the IdentityByExamples project to see.
                //https://stackoverflow.com/questions/33951881/user-identity-getuserid-returns-null-after-successful-login

                // var userId = _userManager.FindByNameAsync(authenticationAccount.Email)?.Id;

                // var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); //this is to get UserId of current user login
                // var userLogin = await _userManager.FindByIdAsync(userId);

                var userLogin = await _userManager.FindByNameAsync(authenticationAccount.Email);

                if (userLogin == null)
                {
                    return Ok(new ResponseAPI() { IsError = false, Message = "Account does not exist." });
                }
               var adminObj = _adminBO.Authentication(userLogin);
                //ResponseResult.IsError = false;
                //ResponseResult.Message = string.Empty;
                return Ok(adminObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return Ok(new ResponseAPI() { IsError = true, Message = ex.Message });
            }

        }
    }
}

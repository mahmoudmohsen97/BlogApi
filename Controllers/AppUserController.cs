using BlogApi.DTOs;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly UserManager<AppUser> UserManager;

        public AppUserController(UserManager<AppUser> _userManager)
        {
            UserManager = _userManager;
        }


        [HttpPost("Register")]
        public async Task<IActionResult>RegisterUser(RegisterDto UserInBody)
        {
            if (ModelState.IsValid)
            {
                AppUser UserToCreate = new AppUser();

                UserToCreate.UserName = UserInBody.UserName;
                UserToCreate.Email = UserInBody.Email;
                
                

                IdentityResult result
                       = await UserManager.CreateAsync(UserToCreate, UserInBody.Password);

                if (result.Succeeded)
                {
                    return Ok("Account Created");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("password", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        //...........................................................
        [HttpPost("Login")]
        public async Task<IActionResult>LoginUser(LoginDto UserInBody)
        {
            if (ModelState.IsValid)
            {
                AppUser UserFromDb = 
                    await UserManager.FindByNameAsync(UserInBody.UserName);

                bool  CorrectPassword = 
                    await UserManager.CheckPasswordAsync(UserFromDb, UserInBody.password);
                if (CorrectPassword) 
                {
                    //claims 

                    List<Claim> UserClaims = new List<Claim>();
                    UserClaims.Add(new Claim(ClaimTypes.NameIdentifier,UserFromDb.Id));
                    UserClaims.Add(new Claim(ClaimTypes.Name,UserFromDb.UserName));
                    UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var UserRoles = await UserManager.GetRolesAsync(UserFromDb);
                    foreach (var RoleName in UserRoles)
                    {
                        UserClaims.Add(new Claim(ClaimTypes.Role, RoleName));
                    }
                    //signinCredintial

                    var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kfddllsldmoci82yhnfc9r0h2nfm20332"));
                    var signingCredential = new SigningCredentials(SignInKey, SecurityAlgorithms.Aes128CbcHmacSha256);

                    //token design
                    JwtSecurityToken SecurityToken = new JwtSecurityToken(
                       issuer: "http://localhost:5047",
                       expires:DateTime.Now.AddHours(1),
                       claims:UserClaims,
                       signingCredentials:signingCredential
                        );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                        expiration = SecurityToken.ValidTo
                    }
                        );
                }
                else
                    ModelState.AddModelError("Paassword", "Username or password is invalid");
                
            }
            return BadRequest(ModelState);
        }
    }
}

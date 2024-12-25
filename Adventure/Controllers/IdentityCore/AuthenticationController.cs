using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adventure.DTO;
using Adventure.Model.Authentcation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.IdentityCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }



        [HttpPost("SingUp")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
             if(ModelState.IsValid) {
                if(await _userManager.FindByEmailAsync(register.Email) == null){
                    var user = new IdentityUser
                    {
                        Email = register.Email,
                        UserName = register.Email
                    };

                    var result = await _userManager.CreateAsync(user, register.Password);
                    if (result.Succeeded)
                    {
                        return new OkObjectResult(new ResponseDetailsStatus()
                            {
                                Success = true,
                                Description = "Registerd succesfully",
                                Data = "registerd successfully"
                            }
                        );
                    }else{
                            return new BadRequestObjectResult(new ResponseDetailsStatus()
                            {
                                Success = false,
                                Description = "Registerd Failed",
                                Data = ""
                            }
                        );
                    }
                }else{
                    return new BadRequestObjectResult(new ResponseDetailsStatus()
                    {
                        Success = false,
                        Description = "Registerd succesfully",
                        Data = "user has an account already"
                    });
                }
             }else {
                return new BadRequestObjectResult(new ResponseDetailsStatus()
                            {
                                Success = false,
                                Description = "Registerd succesfully",
                                Data = ModelState
                            });
            }
        
        }


        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Login login){
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);
                if (result.Succeeded)
                {
                    return new OkObjectResult(new ResponseDetailsStatus()
                            {
                                Success = true,
                                Description = "Login succesfully",
                                Data = "Login successfully"
                            }
                    );
                }
                else
                {
                     return new BadRequestObjectResult(new ResponseDetailsStatus()
                            {
                                Success = true,
                                Description = "Login failed",
                                Data = ""
                            }
                    );
                }
            }

            return new BadRequestObjectResult(new ResponseDetailsStatus()
                                {
                                    Success = true,
                                    Description = "Login failed",
                                    Data = ModelState
                                }
                            );
        }
    }
}
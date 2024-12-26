using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Adventure.DTO;
using Adventure.Model.Authentcation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Controllers.IdentityCore
{
    [Route("[controller]")]
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



        [HttpPost("SignUp")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            //Code improvements checking failure case and stop it earlier is good way of coding
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDetailsStatus
                {
                    Success = false,
                    Description = "Validation failed",
                    Data = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    )
                });
            }

            // Create a new user
            var user = new IdentityUser
            {
                Email = register.Email,
                UserName = register.Email
            };

            try
            {
                // Check if the user already exists
                var existingUser = await _userManager.FindByEmailAsync(register.Email);
                if (existingUser != null)
                {
                    return BadRequest(new ResponseDetailsStatus
                    {
                        Success = false,
                        Description = "User already exists",
                        Data = new List<string> { "An account with this email already exists." }
                    });
                }

               

                var createUserResult = await _userManager.CreateAsync(user, register.Password);
                if (!createUserResult.Succeeded)
                {
                    return BadRequest(new ResponseDetailsStatus
                    {
                        Success = false,
                        Description = "Registration failed",
                        Data = createUserResult.Errors.Select(error => error.Description).ToList()
                    });
                }

                //// Assign the role to the user
                var addToRoleResult = await _userManager.AddToRoleAsync(user, register.Role);
                if (!addToRoleResult.Succeeded)
                {
                    // Optionally, handle cleanup if role assignment fails
                    await _userManager.DeleteAsync(user); // Undo user creation if role assignment fails
                    return BadRequest(new ResponseDetailsStatus
                    {
                        Success = false,
                        Description = "Failed to assign role",
                        Data = addToRoleResult.Errors.Select(error => error.Description).ToList()
                    });
                }

                //// Add claim to the user
                var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("CustomClaim", register.Claim));
                if (!addClaimResult.Succeeded)
                {
                    // Optionally, handle cleanup if claim assignment fails
                    await _userManager.DeleteAsync(user);
                    return BadRequest(new ResponseDetailsStatus
                    {
                        Success = false,
                        Description = "Failed to add claim",
                        Data = addClaimResult.Errors.Select(error => error.Description).ToList()
                    });
                }
            }
            catch(Exception e)
            {
                //if any things fails to create an user like role or claims we will delete the user
                if(await _userManager.FindByEmailAsync(register.Email) != null)
                    await _userManager.DeleteAsync(user);

                return BadRequest(new ResponseDetailsStatus
                {
                    Success = false,
                    Description = "Failed to Register",
                    Data = e.Message
                });
            }
            

            return Ok(new ResponseDetailsStatus
            {
                Success = true,
                Description = "Registration successful",
                Data = "User registered successfully."
            });
        }



        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            //If the validation failed we are stopping the further execution
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new ResponseDetailsStatus()
                {
                    Success = false,
                    Description = "Validation failed",
                    Data = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    )
                });
            }

            var signInUser = await _signInManager.
                  PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);

            if (!signInUser.Succeeded)
            {
                return new BadRequestObjectResult(new ResponseDetailsStatus()
                {
                    Success = true,
                    Description = "Login failed",
                    Data = new List<string> { "userName or Passwrod is Incorrect" }
                });     
            }

            return new OkObjectResult(new ResponseDetailsStatus()
            {
                Success = true,
                Description = "Login succesfully",
                Data = "Login successfully"
            });


        }
    }
}
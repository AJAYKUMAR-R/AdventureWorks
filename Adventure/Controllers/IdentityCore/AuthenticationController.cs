using System;
using System.Security.Claims;
using Adventure.Model.Authentcation;
using Adventure.Models.Authentcation;
using Adventure.Models.Identity;
using Adventure.Utlis.Response;
using Adventure.Utlis.TokenGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Adventure.Controllers.IdentityCore
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
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
                return ResponseHelper.CreateBadRequest("Validation failed", ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    ));
            }

            // Create a new user
            var user = new ApplicationUser
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
                    return ResponseHelper.CreateBadRequest("User already exists", new List<string> { "An account with this email already exists." });
                }

                var createUserResult = await _userManager.CreateAsync(user, register.Password);
                if (!createUserResult.Succeeded)
                {
                    return ResponseHelper.CreateBadRequest("Registration failed", createUserResult.Errors.Select(error => error.Description).ToList());
                }

                
                //// Assign the role to the user
                var addToRoleResult = await _userManager.AddToRoleAsync(user, register.Role.ToString());
                if (!addToRoleResult.Succeeded)
                {
                    // Optionally, handle cleanup if role assignment fails
                    await _userManager.DeleteAsync(user); // Undo user creation if role assignment fails

                    return ResponseHelper.CreateBadRequest("Failed to assign role", addToRoleResult.Errors.Select(error => error.Description).ToList());
                }

                //// Add claim to the user
                var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("Permission", register.Claim.ToString()));
                if (!addClaimResult.Succeeded)
                {
                    // Optionally, handle cleanup if claim assignment fails
                    await _userManager.DeleteAsync(user);
                    return ResponseHelper.CreateBadRequest("Failed to add claim", addToRoleResult.Errors.Select(error => error.Description).ToList());
                }
            }
            catch(Exception e)
            {
                //if any things fails to create an user like role or claims we will delete the user
                if(await _userManager.FindByEmailAsync(register.Email) != null)
                    await _userManager.DeleteAsync(user);
                return ResponseHelper.CreateBadRequest("Failed to Register", e.Message);
            }

            //Finally generating token
            return ResponseHelper.CreateOkRequest("Registerd succesfully", "Registerd successfully");
        }



        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            //If the validation failed we are stopping the further execution
            if (!ModelState.IsValid)
            {
                return ResponseHelper.CreateBadRequest("Validation failed",  ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                ));
            }

            var signInUser = await _signInManager.
                  PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, true);

            //if the user enter the correct username then only the user got blocked for 5 min
            if (signInUser.IsLockedOut)
            {
                return ResponseHelper.CreateBadRequest("Login failed", new List<string> { "Too Many attempts the account has been locked for 5 min" });
            }

            if(!signInUser.Succeeded)
            {
               return ResponseHelper.CreateBadRequest("Login failed", new List<string> { "userName or Passwrod is Incorrect" });  
            }

            try
            {
                // Create a new user
                var user = await _userManager.FindByEmailAsync(login.UserName);

                var roles = await _userManager.GetRolesAsync(user);
                var claims = await _userManager.GetClaimsAsync(user);
                var accessToken = TokenHelper.GenerateJwtToken(user, roles, claims);
                var refreshToken = TokenHelper.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddSeconds(15);

                await _userManager.UpdateAsync(user);

                //Finally generating token
                return ResponseHelper.CreateOkRequest("Login succesfully", new
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch(Exception e)
            {
                return ResponseHelper.CreateBadRequest("Login failed", new List<string> { "issue in generating token" });
            }

            
        }


        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return ResponseHelper.CreateBadRequest("Validation failed", ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                ));
            }

            try
            {
                ClaimsPrincipal? principal = TokenHelper.GetPrincipalFromExpiredToken(tokenRequest.Token);

                if (principal == null)
                    return Unauthorized();

                var username = principal.Identity?.Name;
                var user = await _userManager.FindByNameAsync(username);
                if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                    return Unauthorized();

                var roles = await _userManager.GetRolesAsync(user);
                var claims = await _userManager.GetClaimsAsync(user);
                var newAccessToken = TokenHelper.GenerateJwtToken(user, roles, claims);
                var newRefreshToken = TokenHelper.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken
                });

            }
            catch(Exception e)
            {
                return ResponseHelper.CreateBadRequest("Refresh token failed to generate", new List<string> { e.Message });
            }
        }
    }
}
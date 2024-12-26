using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Adventure.DTO;
using Adventure.Enums;
using Adventure.Model.Authentcation;
using Adventure.Utlis.Response;
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
                return ResponseHelper.CreateBadRequest("Validation failed", ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    ));
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
                var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("CustomClaim", register.Claim.ToString()));
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

            return ResponseHelper.CreateOkRequest("Registration successful", "User registered successfully.");
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

            if (signInUser.IsLockedOut)
            {
                return ResponseHelper.CreateBadRequest("Login failed", new List<string> { "Too Many attempts the account has been locked for 5 min" });
            }

            if(!signInUser.Succeeded)
            {
               return ResponseHelper.CreateBadRequest("Login failed", new List<string> { "userName or Passwrod is Incorrect" });  
            }

            return ResponseHelper.CreateOkRequest("Login succesfully", "Login successfully");
        }
    }
}
using Adventure.Models.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Adventure.Utlis.TokenGenerator
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(ApplicationUser user, IList<string> roles, IList<Claim> claims)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), //username of the user
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //Identityfier for the claims
            };

            // Add roles to claims
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Add roles to claims
            authClaims.AddRange(claims);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"));

            //This expire time will evaluated in the middleware level - look program.cs
            //using authorize attribute above the api level
            var token = new JwtSecurityToken(
                issuer: "http://localhost:5289",
                audience: "http://localhost:5289",
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            //This will also validate the access token life span
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, // You might need to adjust this
                ValidateIssuer = true,   // You might need to adjust this
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere")),
                ValidateLifetime = true // We want to validate expired tokens here
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return principal;

            throw new SecurityTokenException("Invalid token");
        }

    }
}

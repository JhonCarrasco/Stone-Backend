using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Stone.Dto.Helper
{
    public static class JwtTokenHelper
    {
        public static string GetClaimValueFromToken(string tokenString, string claimType)
        {
            if (string.IsNullOrEmpty(tokenString))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Read the token (does not validate signature or expiration)
                var securityToken = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;

                if (securityToken == null)
                {
                    return null;
                }

                // Find the first claim with the specified type and return its value
                var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

                return claimValue;
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., if the token string is not a valid JWT)
                return null;
            }
        }
    }
}

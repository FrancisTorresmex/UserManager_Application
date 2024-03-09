using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace userManagerApplication.Indentity
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSecretKey()
        {
            return _configuration["KeyJWT"];
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = GetSecretKey();
                var key = Encoding.UTF8.GetBytes(tokenKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        //Validate that the token is valid
        public bool ValidateTokenAccess(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                // Verifica y decodifica el token
                var tokenClaims = ValidateToken(token);

                if (tokenClaims == null)
                {
                    return false;
                }

                // Verifica si el token contiene el reclamo de rol "Admin"
                //if (!tokenClaims.Claims.Any(claim => claim.Type == "Admin" /*&& claim.Value == "true"*/))
                //{

                //    // Si el usuario no tiene el rol "Admin", redirige al usuario a una página de acceso denegado u otra acción apropiada.
                //    return false;
                //}
                return true;
            }
            else
                return false;


        }

        //public bool ValidateTokenAccess(string token)
        //{
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        // Verifica y decodifica el token
        //        var tokenHelper = new TokenHelper(_configuration);
        //        var tokenClaims = tokenHelper.ValidateToken(token);

        //        if (tokenClaims == null)
        //        {
        //            return false;
        //        }

        //        // Verifica si el token contiene el reclamo de rol "Admin"
        //        if (!tokenClaims.Claims.Any(claim => claim.Type == "Admin" && claim.Value == "true"))
        //        {
        //            // Si el usuario no tiene el rol "Admin", redirige al usuario a una página de acceso denegado u otra acción apropiada.
        //            return false;
        //        }
        //        return true;
        //    }
        //    else
        //        return false;


        //}

    }
}

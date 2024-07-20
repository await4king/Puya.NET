using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Puya.Jwt
{
    // source: https://medium.com/@mmoshikoo/jwt-authentication-using-c-54e0c71f21b0
    public class JwtService: IJwtService
    {
        private JwtConfig _config;
        public JwtConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new JwtConfig();
                }

                return _config;
            }
            set { _config = value; }
        }
        public JwtService(JwtConfig config)
        {
            this.Config = config;
        }
        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(Config.SecretKey);

            return new SymmetricSecurityKey(symmetricKey);
        }
        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
        public string GenerateToken()
        {
            if (Config.Claims == null || Config.Claims.Length == 0)
                throw new ArgumentException("Arguments to create token are not valid");

            var symmetricKey = GetSymmetricSecurityKey();
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Config.Claims),
                Expires = Config.ExpireAt,
                SigningCredentials = new SigningCredentials(symmetricKey, Config.SecurityAlgorithm),
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty");

            var tokenValidationParameters = GetTokenValidationParameters();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                return tokenValid.Claims;
            }
            catch
            {
                throw;
            }
        }

        public bool IsTokenValid(string token, out Exception e)
        {
            var result = false;

            if (string.IsNullOrEmpty(token))
            {
                e = new ArgumentException("Given token is null or empty");
            }
            else
            {
                var tokenValidationParameters = GetTokenValidationParameters();
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                    e = null;
                    result = true;
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            }

            return result;
        }
    }
}

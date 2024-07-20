using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Puya.Jwt
{
    public interface IJwtService
    {
        JwtConfig Config { get; set; }
        bool IsTokenValid(string token, out Exception e);
        string GenerateToken();
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}

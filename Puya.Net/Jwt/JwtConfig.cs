using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Puya.Jwt
{
    public class JwtConfig
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; }
        public DateTime ExpireAt { get; set; }
        public Claim[] Claims { get; set; }
    }
}

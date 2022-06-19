using System;

namespace Services.DTOs.Sessions
{
    public class JwtDto
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
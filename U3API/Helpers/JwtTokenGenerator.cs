using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using U3API.Models.DTOs;

namespace U3API.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        //public string GetToken(LogInDto dto)
        //{

        //    List<Claim> Claims = new();

        //    Claims.Add(new Claim(ClaimTypes.Role, dto.NombreDept));
        //    Claims.Add(new Claim(ClaimTypes.Email, dto.Username));
        //    Claims.Add(new Claim(JwtRegisteredClaimNames.Iss, "itesrcU3"));
        //    Claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "pruebaU3"));
        //    Claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
        //    Claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(5).ToString())); //aumentar

        //    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        //    var token = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(Claims),
        //        Issuer = "itesrcU3",
        //        Audience = "pruebaU3",
        //        IssuedAt = DateTime.Now,
        //        Expires = DateTime.UtcNow.AddMinutes(5),
        //        NotBefore = DateTime.UtcNow.AddMinutes(-1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("oWgVSB6azKciOUdwnRSMxKPyccYXiVp0qP0svFWemCQRK45kkbf3rqHbykHHYntKYyMxjKFJia9n7ZbKiC380uFSBuSuhzRd8IhY")), SecurityAlgorithms.HmacSha512)
        //    };

        //    return handler.CreateEncodedJwt(token);
        //}

        public string GetToken(string username, string role, List<Claim> claims)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var issuer = _config.GetSection("Jwt").GetValue<string>("Issuer") ?? "";
            var audience = _config.GetSection("Jwt").GetValue<string>("Audience") ?? "";
            var secret = _config.GetSection("Jwt").GetValue<string>("Secret");

            List<Claim> basicas = new()
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, username),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience)
            };

            basicas.AddRange(claims);

            JwtSecurityToken jwtSecurity = new(issuer, audience, basicas, DateTime.Now, DateTime.Now.AddMinutes(20),
              new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
              SecurityAlgorithms.HmacSha512));

            return handler.WriteToken(jwtSecurity);
        }
    }
}

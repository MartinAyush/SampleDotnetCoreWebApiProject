using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.Api.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string CreateToken(IdentityUser user, List<string> roles)
        {
            // Create claims: JWT claims are the core information that JWTs transmit (kinda like the letter inside a sealed envelope).
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach(var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // Jwt Key stores in the appsettings.json file
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Encrypt jwtKey using security algorithm
            var credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Zcy.MES.Controllers
{
    public class WeatherForecastController : BaseNormalController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("get-admin")]
        [Authorize(Roles = "admin")]
        public string GetAdmin()
        {
            return "admin";
        }

        [HttpGet("get-root")]
        [Authorize(Roles = "root")]
        public string GetRoot()
        {
            return "root";
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "userName"),
                new Claim(JwtClaimTypes.Role, "admin"),
                new Claim(JwtClaimTypes.Role, "root"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                //audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = encodedToken
            })
            .ToArray();
        }
    }
}

using BusinessLayer.IServices;
using DataLayer.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ModelLayer.Configuration; // <-- Doğru


namespace BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwt;

        public AuthService(IUnitOfWork uow, IOptions<JwtSettings> jwtOptions)
        {
            _uow = uow;
            _jwt = jwtOptions.Value;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {

            string role = null;
            string fullName = null;
            int subjectId = 0;

            var user = (await _uow.User.GetWhereAsync(u => u.Email == dto.Email && u.Password == dto.Password)).FirstOrDefault();
            if (user != null)
            {
                role = "User";
                fullName = user.FullName;
                subjectId = user.UserId;
            }
            else
            {
                var dietitian = (await _uow.Dietitian.GetWhereAsync(d => d.Email == dto.Email && d.Password == dto.Password)).FirstOrDefault();
                if (dietitian != null)
                {
                    role = "Dietitian";
                    fullName = dietitian.FullName;
                    subjectId = dietitian.DietitianId;
                }
                else
                {
                    var sec = (await _uow.Secretary.GetWhereAsync(s => s.Email == dto.Email && s.Password == dto.Password)).FirstOrDefault();
                    if (sec != null)
                    {
                        role = "Secretary";
                        fullName = sec.FullName;
                        subjectId = sec.SecretaryId;
                    }
                }
            }

            if (role == null)
            {
                return null;
            }

            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, subjectId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, dto.Email),
                new Claim("name", fullName ?? ""),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenObj);

            return new AuthResponseDto
            {
                Token = tokenString,
                ExpiresAt = expires,
                Role = role,
                SubjectId = subjectId,
                FullName = fullName
            };
        }
    }
}

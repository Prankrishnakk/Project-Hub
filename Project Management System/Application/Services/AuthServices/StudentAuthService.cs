using Application.Dto;
using Application.Interface.AuthInterface;
using AutoMapper;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthServices
{
    public class StudentAuthService : IStudentAuthService
    {
        private readonly IStudentAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public StudentAuthService(IStudentAuthRepository repository, IConfiguration config, IMapper mapper)
        {
            _repository = repository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<string> RegisterAsync(StudentRegDto dto)
        {
            // Trim inputs
            dto.Name = dto.Name?.Trim();
            dto.Email = dto.Email?.Trim();
            dto.Department = dto.Department?.Trim();
            dto.Password = dto.Password?.Trim();
            dto.ConfirmPassword = dto.ConfirmPassword?.Trim();


            if (dto.Password != dto.ConfirmPassword)
                throw new ArgumentException("Passwords do not match.");

            if (await _repository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException($"An account with email '{dto.Email}' already exists.");

            var student = _mapper.Map<Student>(dto);
            student.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _repository.AddAsync(student);
            return "Registration successful.";
        }

        public async Task<AuthResponseDto> LoginAsync(StudentLoginDto dto)
        {
            var student = await _repository.GetByEmailAsync(dto.Email);

            if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.Password))
                throw new AuthenticationException("Invalid email or password.");

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new Claim(ClaimTypes.Name, student.Name),
            new Claim(ClaimTypes.Role, student.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = student.Role,
                Name = student.Name,
                Email = student.Email
            };
        }
    }

}

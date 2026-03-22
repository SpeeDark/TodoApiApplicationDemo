using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoApi.API.DTOs;
using TodoApi.Models;
using TodoApi.Services;
using BCryptImpl = BCrypt.Net.BCrypt;

namespace TodoApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(TodoContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> PostRegister(RegisterDto registerDto)
        {
            var existingUser = await _context.Users.AnyAsync(
                user => user.Email == registerDto.Email);

            if (existingUser) return BadRequest("User already exists");

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = BCryptImpl.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            var response = _mapper.Map<AuthResponseDto>(user);
            response.Token = token;
            
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> PostLogin(LoginDto loginDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(
                user => user.Email == loginDto.Email);

            if (existingUser == null || !VerifyPassword(loginDto.Password, existingUser.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _tokenService.GenerateToken(existingUser);

            var response = _mapper.Map<AuthResponseDto>(existingUser);
            response.Token = token;

            return Ok(response);
        }

        private bool VerifyPassword(string inputPassword, string storedHash) =>
            BCryptImpl.Verify(inputPassword, storedHash);
    }
}

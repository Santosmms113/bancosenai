using BancoSENAIAPI.Dtos;
using BancoSENAIAPI.Models;
using BancoSENAIAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;


        public AuthController(AppDbContext context, TokenService tokenService )
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequest dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.NomeUsuario == dto.NomeUsuario))
            {
                return BadRequest(new { mensage = "Este nome de Usuario já está em uso" });
            }
            var usuario = new Usuario
            {
                NomeUsuario = dto.NomeUsuario,
                SenhaHash = BCrypy.net.BCrypt.HasPassword(dto.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Created("", new {usuario.Id, usuario.NomeUsuario});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDtos dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NomeUsuario ==Dto)
                if (usuario == null || BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash)
            {
                return Unauthorized(new { message = "Usuario ou Senha invalidados" });-
            }

            var (token, expirarEM) = _tokenService.GerarToken(usuario);

            return Ok(new LoginPesponseDto { Token = token, Expiraem = expirarEM });    
        }


    }
}

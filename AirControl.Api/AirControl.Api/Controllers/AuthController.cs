using AirControl.Api.Data;
using AirControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace AirControl.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // ====== LOGIN ======
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model == null)
                return BadRequest("Dados de login inválidos.");

            // 🔒 BLOQUEIO POR PERÍODO DE TESTES
            // appsettings.json:
            // "Licensing": { "TrialEnd": "2025-12-31" }
            var trialEndStr = _config["Licensing:TrialEnd"];
            if (DateTime.TryParse(trialEndStr, out var trialEnd) &&
                DateTime.UtcNow.Date > trialEnd.Date)
            {
                return StatusCode(403, new
                {
                    message = "Período de testes encerrado. Entre em contato para liberar o acesso."
                });
            }

            var email = (model.Email ?? string.Empty).Trim().ToLowerInvariant();
            var senha = model.Senha ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                return BadRequest("Informe e-mail e senha.");

            // garante comparação de e-mail sem problema de maiúscula/minúscula
            var user = await _db.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Ativo &&
                                           u.Email.ToLower() == email);

            if (user == null)
                return Unauthorized("Usuário não encontrado.");

            if (!VerificarSenha(senha, user.SenhaHash))
                return Unauthorized("Senha incorreta.");

            // ⬇️ IMPORTANTE: devolve o cargo
            return Ok(new
            {
                id = user.Id,
                nome = user.Nome,
                email = user.Email,
                cargo = user.Cargo ?? string.Empty
            });
        }

        // ====== CADASTRAR USUÁRIO ======
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarRequest model)
        {
            if (model == null)
                return BadRequest("Dados de registro inválidos.");

            var nome = (model.Nome ?? string.Empty).Trim();
            var email = (model.Email ?? string.Empty).Trim().ToLowerInvariant();
            var senha = model.Senha ?? string.Empty;
            var cargo = string.IsNullOrWhiteSpace(model.Cargo)
                ? "Tecnico"
                : model.Cargo.Trim();
            var telefone = (model.Telefone ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("E-mail é obrigatório.");

            if (string.IsNullOrWhiteSpace(senha))
                return BadRequest("Senha é obrigatória.");

            // validação simples de e-mail
            if (!email.Contains("@") || !email.Contains("."))
                return BadRequest("E-mail em formato inválido.");

            // Já existe esse e-mail?
            if (await _db.Usuarios.AnyAsync(u => u.Email == email))
                return Conflict("E-mail já cadastrado.");

            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                Cargo = cargo,
                Telefone = telefone,
                SenhaHash = GerarHash(senha),
                Ativo = true
            };

            _db.Usuarios.Add(usuario);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMsg = ex.InnerException?.Message ?? string.Empty;
                if (innerMsg.Contains("IX_Usuarios_Email", StringComparison.OrdinalIgnoreCase))
                {
                    return Conflict("E-mail já cadastrado.");
                }

                return StatusCode(500, "Erro ao salvar usuário.");
            }

            return Ok(new
            {
                mensagem = "Usuário registrado com sucesso",
                usuario = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    cargo = usuario.Cargo,
                    telefone = usuario.Telefone
                }
            });
        }

        // ===== UTILS: HASH DE SENHA =====

        private string GerarHash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return GerarHash(senha) == hash;
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class RegistrarRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}


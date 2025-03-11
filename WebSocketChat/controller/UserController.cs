
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace WebSocketChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUseCase;
        private readonly IMediator _mediatR;

        private readonly ILogger<UserController> _logger;

        private readonly IConfiguration _configuration;

        public UserController(ICreateUserUseCase createUseCase,
        IMediator mediatR,
        ILogger<UserController> logger,
        IConfiguration configuration)
        {
            _createUseCase = createUseCase;
            _mediatR = mediatR;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _createUseCase.Execute(userDto, cancellationToken);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return CreatedAtAction(nameof(CreateUser), new { id = result.Value }, result.Value);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro inesperado ao criar user.");
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _mediatR.Send(new GetUserRequest(), cancellationToken);

                if (users == null || users.Count == 0)
                    return NotFound("Nenhum usuário encontrado.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado ao buscar usuários: {ex.Message}");
            }
        }


        [HttpPost("Login")]
        public async Task<IResult> Login(GetLoginRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"User: {request.Email}, Password: {request.Password}");
            bool result = await _mediatR.Send(request, cancellationToken);
            // Verifique as credenciais do usuário (exemplo simples)
            if (result)
            {
                var key = _configuration["Key"];

                var token = new JwtSecurityToken(
                    issuer: "ChatBackend",
                    audience: "ChatFrontend",
                    claims: new[] { new Claim(ClaimTypes.Name, request.Email) },
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), SecurityAlgorithms.HmacSha256)
                );

                return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Results.Unauthorized();
        }

    }
}
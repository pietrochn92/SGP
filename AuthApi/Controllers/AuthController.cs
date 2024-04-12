using Application.DTOs.Authentication;
using Application.Repository.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        protected ResponseDto _response;
        public AuthController(
            IAuthRepository authRepository, 
            IConfiguration configuration, 
            ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _logger = logger;
            _response = new();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            _logger.LogInformation("Inizio metodo Register");

            var errorMessage = await _authRepository.Register(registrationDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;

                _logger.LogWarning($"BadRequest - {_response.Message}");

                return BadRequest(_response);
            }

            _logger.LogInformation("Fine metodo Register");

            return Ok(_response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            _logger.LogInformation("Inizio metodo Login");

            var loginResponse = await _authRepository.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username o password non corretti.";

                _logger.LogWarning($"BadRequest - {_response.Message}");

                return BadRequest(_response);
            }

            _response.Result = loginResponse;

            _logger.LogInformation("Fine metodo Login");

            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationDto registrationDto)
        {
            _logger.LogInformation("Inizio metodo AssignRole");

            var assignRoleSuccessful = await _authRepository.AssignRole(registrationDto.Email, registrationDto.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Errore nell'assegnazione del ruolo.";

                _logger.LogWarning($"BadRequest - {_response.Message}");

                return BadRequest(_response);
            }

            _logger.LogInformation("Fine metodo AssignRole");

            return Ok(_response);
        }
    }
}

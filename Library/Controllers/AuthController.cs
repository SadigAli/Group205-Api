using FluentValidation;
using Library.Data.DTOs.Auth;
using Library.Data.Entities;
using Library.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IAuthManager _authManager;
        public AuthController(
            IValidator<RegisterDTO> registerValidator,
            IValidator<LoginDTO> loginValidator,
            IAuthManager authManager)
        {
            _registerValidator = registerValidator;
            _authManager = authManager;
            _loginValidator = loginValidator;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO model)
        {
            var validateResult = _registerValidator.Validate(model);
            if (!validateResult.IsValid) return BadRequest(validateResult.Errors.Select(x=>x.ErrorMessage));
            var (status, message) = await _authManager.Register(model);
            if(status == 0) return BadRequest(message);
            return Ok(message);
        }

        [HttpPost("/login")] // api/auth/login

        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await _loginValidator.ValidateAsync(model);
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToList());
            try
            {
                var (status, message) = await _authManager.Login(model);
                if (status == 0) return BadRequest(message);
                return Ok(new
                {
                    Message = "Successfully logged in",
                    Token = message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

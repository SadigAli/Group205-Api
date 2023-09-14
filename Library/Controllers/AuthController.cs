using FluentValidation;
using Library.Data.DTOs.Auth;
using Library.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<RegisterDTO> _validator;
        private readonly UserManager<AppUser> _userManager;
        public AuthController(IValidator<RegisterDTO> validator,UserManager<AppUser> userManager)
        {
            _validator = validator;
            _userManager = userManager;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO model)
        {
            var validateResult = _validator.Validate(model);
            if (!validateResult.IsValid) return BadRequest(validateResult.Errors.Select(x=>x.ErrorMessage));
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null) return BadRequest(new { Message = "This user is already exists" });
            user = await _userManager.FindByNameAsync(model.Username);
            if (user != null) return BadRequest(new { Message = "This user is already exists" });
            user = new AppUser
            {
                FirstName = model.Firstname,
                LastName = model.Lastname,
                UserName = model.Username,
                Email = model.Email,
            };

            var identityResult = await _userManager.CreateAsync(user, model.Password);
            if(!identityResult.Succeeded) return BadRequest(identityResult.Errors);
            return Ok(new { Message = "User registered successfully" });
        }
    }
}

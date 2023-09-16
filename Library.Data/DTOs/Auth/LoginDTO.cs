using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DTOs.Auth
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please, enter your email")
                .EmailAddress().WithMessage("Your email is not correct format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please, enter your password");
        }
    }
}

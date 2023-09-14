using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DTOs.Auth
{
    public class RegisterDTO : LoginDTO
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class RegisterValidation : AbstractValidator<RegisterDTO>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please, enter your mail")
                .EmailAddress().WithMessage("Email address is not correct format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please, enter your password");

            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("Please, enter your firstname");

            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Please, enter your lastname");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Please, enter your username");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x=>x.Password).WithMessage("Please, confirm password");
        }
    }
}

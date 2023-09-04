namespace Infrastructure.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(1, 20).WithMessage("Invalid length.");
    }
}

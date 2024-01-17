namespace Infrastructure.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotEmpty().WithMessage("UserName is required.");
        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(1, 20).WithMessage("Invalid length.");
    }
}

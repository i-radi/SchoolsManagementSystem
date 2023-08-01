namespace Infrastructure.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(s => s.Email).NotNull().NotEmpty().Length(1, 50);
        RuleFor(s => s.Password).NotNull().NotEmpty().Length(1, 20);
    }
}

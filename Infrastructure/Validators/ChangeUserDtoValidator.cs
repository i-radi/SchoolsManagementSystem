namespace Infrastructure.Validators;

public class ChangeUserDtoValidator : AbstractValidator<ChangeUserDto>
{
    public ChangeUserDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(s => s.Password)
            .Length(1, 20).WithMessage("Invalid password length.");
    }
}

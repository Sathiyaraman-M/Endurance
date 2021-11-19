namespace Quark.Core.Validators.Requests.Identity;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Current Password is required!");
        RuleFor(request => request.NewPassword)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Password is required!")
            .MinimumLength(8).WithMessage("Password must be at least of length 8")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one capital letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit");
        RuleFor(request => request.ConfirmNewPassword)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Password Confirmation is required!")
            .Equal(request => request.NewPassword).WithMessage(x => "Passwords don't match");
    }
}
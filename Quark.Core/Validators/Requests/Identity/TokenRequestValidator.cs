namespace Quark.Core.Validators.Requests.Identity;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "UserName is required");
        RuleFor(request => request.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Password is required!");
    }
}
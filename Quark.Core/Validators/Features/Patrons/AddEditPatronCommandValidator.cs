using Quark.Core.Features.Patrons.Commands;

namespace Quark.Core.Validators.Features.Patrons;

public class AddEditPatronCommandValidator : AbstractValidator<AddEditPatronCommand>
{
    public AddEditPatronCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage(x => "First Name cannot be empty");
        RuleFor(x => x.LastName).NotEmpty().WithMessage(x => "Last Name cannot be empty");
        RuleFor(x => x.Address).NotEmpty().WithMessage(x => "Address cannot be empty");
        RuleFor(x => x.Email).NotEmpty().WithMessage(x => "Email cannot be empty");
        RuleFor(x => x.Email).EmailAddress().WithMessage(x => "Invalid Email address");
        RuleFor(x => x.Mobile).Matches("[0-9]{10}$").WithMessage(x => "Enter 10-digit phone number");
        RuleFor(x => x.Mobile).NotEmpty().WithMessage(x => "Phone Number cannot be empty");
        RuleFor(x => x.DateOfBirth).NotNull().WithMessage(x => "Date of birth cannot be empty");
        RuleFor(x => x.RegisterId).NotEmpty().WithMessage(x => "Library Card Register Number cannot be empty");
        RuleFor(x => x.Issued).NotNull().WithMessage(x => "Date of card issue cannot be empty");
        RuleFor(x => x.MultipleCheckoutLimit).ExclusiveBetween(0, 11).WithMessage(x => "Limit must be between 1 to 10");
    }
}
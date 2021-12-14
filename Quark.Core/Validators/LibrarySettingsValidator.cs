using Quark.Core.Configurations;

namespace Quark.Core.Validators;

public class LibrarySettingsValidator : AbstractValidator<LibrarySettings>
{
    public LibrarySettingsValidator()
    {
        RuleFor(x => x.CheckInDelayFinePerDay).GreaterThanOrEqualTo(0M).WithMessage(x => "Fine amount cannot be negative");
        RuleFor(x => x.DefaultMultipleCheckoutLimit).GreaterThanOrEqualTo(1).WithMessage(x => "Simultaneous multiple checkout must be atleast 1");
        RuleFor(x => x.DefaultExpectedCheckInInterval).GreaterThanOrEqualTo(1).WithMessage(x => "Default Expected Check In Interval must atleast 1 day");
    }
}
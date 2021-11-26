using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Core.Validators.Features.Checkouts;

public class CheckInBookCommandValidator : AbstractValidator<CheckInBookCommand>
{
    public CheckInBookCommandValidator()
    {
        RuleFor(x => x.BookBarcode).NotEmpty().WithMessage(x => "Book barcode is required");
        RuleFor(x => x.CheckInDate).NotNull().WithMessage(x => "Check In date is required");
    }
}
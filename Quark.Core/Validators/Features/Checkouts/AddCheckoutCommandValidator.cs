using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Core.Validators.Features.Checkouts;

public class AddCheckoutCommandValidator : AbstractValidator<AddCheckoutCommand>
{
    public AddCheckoutCommandValidator()
    {
        RuleFor(x => x.BookBarcode).NotEmpty().WithMessage(x => "Book Barcode is required");
        RuleFor(x => x.PatronRegisterId).NotEmpty().WithMessage(x => "Patron ID is required");
        RuleFor(x => x.CheckedOutSince).NotNull().WithMessage(x => "Checkout Date is required");
        RuleFor(x => x.ExpectedCheckInDate).NotNull().WithMessage(x => "Expected Check In Date is required");
    }
}
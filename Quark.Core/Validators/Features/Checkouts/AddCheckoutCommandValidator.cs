using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Core.Validators.Features.Checkouts;

public class AddCheckoutCommandValidator : AbstractValidator<AddCheckoutCommand>
{
    public AddCheckoutCommandValidator()
    {
        RuleFor(x => x.BookBarcode).NotEmpty().WithMessage(x => "Book Barcode is required");
        RuleFor(x => x.PatronRegisterId).NotEmpty().WithMessage(x => "Patron ID is required");
    }
}
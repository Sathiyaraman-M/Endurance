using Quark.Core.Features.Books.Commands;

namespace Quark.Core.Validators.Features.Books;

public class AddEditBookHeaderCommandValidator : AbstractValidator<AddEditBookHeaderCommand>
{
    public AddEditBookHeaderCommandValidator()
    {
        RuleFor(x => x.Barcode).NotEmpty().WithMessage(x => "Book barcode cannot be empty");
    }
}
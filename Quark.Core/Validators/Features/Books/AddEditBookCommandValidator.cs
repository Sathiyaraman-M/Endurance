using Quark.Core.Features.Books.Commands;

namespace Quark.Core.Validators.Features.Books;

public class AddEditBookCommandValidator : AbstractValidator<AddEditBookCommand>
{
    public AddEditBookCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(x => "Book name is required");
        RuleFor(x => x.ISBN).MinimumLength(10).MaximumLength(13).WithMessage(x => "ISBN is required");
        RuleFor(x => x.Author).NotEmpty().WithMessage(x => "Author name is required");
        RuleFor(x => x.DeweyIndex).NotEmpty().WithMessage(x => "Dewey Index is required");
        RuleFor(x => x.PublicationYear).GreaterThanOrEqualTo(1000).LessThanOrEqualTo(DateTime.Today.Year).WithMessage(x => "Publication Year is not valid");
        RuleFor(x => x.Barcode).NotEmpty().WithMessage(x => "Barcode cannot be empty");
        RuleFor(x => x.Publisher).NotEmpty().WithMessage("Publisher Name is required");
        RuleFor(x => x.Edition).NotEmpty().WithMessage("Edition is required");
        RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative");
    }
}
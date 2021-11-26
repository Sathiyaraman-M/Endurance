using Quark.Core.Features.Designations.Commands;

namespace Quark.Core.Validators.Features.Designations;

public class AddEditDesignationCommandValidator : AbstractValidator<AddEditDesignationCommand>
{
    public AddEditDesignationCommandValidator()
    {
        RuleFor(x => x.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Designation name is required!");
    }
}
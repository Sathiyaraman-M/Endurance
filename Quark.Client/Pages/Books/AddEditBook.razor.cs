using Quark.Core.Features.Books.Commands;

namespace Quark.Client.Pages.Books;

public partial class AddEditBook
{
    [Parameter]
    public Guid? Id { get; set; }

    private AddEditBookCommand Model = new();

    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        Id ??= Guid.Empty;
    }

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty && Id.HasValue)
        {
            var response = await _bookHttpClient.GetByIdAsync(Id.Value);
            if (response.Succeeded)
            {
                var book = response.Data;
                Model = new()
                {
                    Id = book.Id,
                    Name = book.Name,
                    ISBN = book.ISBN,
                    Author = book.Author,
                    DeweyIndex = book.DeweyIndex,
                    Description = book.Description,
                    Publisher = book.Publisher,
                    PublicationYear = book.PublicationYear,
                    Edition = book.Edition,
                    Cost = book.Cost,
                    ImageUrl = book.ImageUrl,
                    Headers = book.BookHeaders
                };
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    snackbar.Add(message, Severity.Error);
                }
            }
        }
        await base.OnInitializedAsync();
    }

    private void AddBookItem()
    {
        Model.Headers.Add(new BookHeaderResponse
        {
            Condition = AssetStatusConstants.GoodCondition
        });
    }

    private void DeleteBookItem(BookHeaderResponse bookItem)
    {
        if (bookItem.Id != Guid.Empty)
        {
            Model.DeleteBookHeaders.Add(bookItem.Id);
        }
        Model.Headers.Remove(bookItem);
    }

    private async Task SaveAsync()
    {
        var response = await _bookHttpClient.SaveAsync(Model);
        foreach (var message in response.Messages)
        {
            snackbar.Add(message, response.Succeeded ? Severity.Normal : Severity.Error);
        }
    }
}
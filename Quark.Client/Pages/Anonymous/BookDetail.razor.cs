namespace Quark.Client.Pages.Anonymous;

public partial class BookDetail
{
    [Parameter]
    public Guid Id { get; set; }

    private BookResponse Book { get; set; }
    private bool _loaded = false;

    protected override async Task OnParametersSetAsync()
    {
        var response = await _bookHttpClient.GetByIdAsync(Id);
        if (response.Succeeded)
        {
            Book = response.Data;
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
        _loaded = true;
    }
}
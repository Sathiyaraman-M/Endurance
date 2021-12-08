namespace Quark.Client.Pages.Identity;

public partial class ForgotPassword
{
    private ForgotPasswordRequest requestModel = new();

    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => options.IncludeAllRuleSets());

    private async Task SubmitAsync()
    {
        var response = await userHttpClient.ForgotPasswordAsync(requestModel);
        if (response.Succeeded)
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Success);
            }
            navigationManager.NavigateTo("/");
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
            requestModel.Email = "";
        }
    }
}
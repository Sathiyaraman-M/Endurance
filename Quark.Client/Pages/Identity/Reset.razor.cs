using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Quark.Client.Pages.Identity;

public partial class Reset
{
    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
    private readonly ResetPasswordRequest _resetPasswordModel = new();

    protected override void OnInitialized()
    {
        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out var param))
        {
            var queryToken = param.First();
            _resetPasswordModel.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(queryToken));
        }
    }

    private async Task SubmitAsync()
    {
        if (!string.IsNullOrEmpty(_resetPasswordModel.Token))
        {
            var result = await userHttpClient.ResetPasswordAsync(_resetPasswordModel);
            if (result.Succeeded)
            {
                snackbar.Add(result.Messages[0], Severity.Success);
                navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    snackbar.Add(message, Severity.Error);
                }
            }
        }
        else
        {
            snackbar.Add("Token Not Found!", Severity.Error);
        }
    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}
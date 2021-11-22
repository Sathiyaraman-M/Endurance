using Blazored.FluentValidation;
using MudBlazor;
using Quark.Core.Requests.Identity;

namespace Quark.Client.Pages.Authentication;

public partial class Account
{
    private FluentValidationValidator fluentValidationValidator;
    private bool Validated => fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
    private readonly ChangePasswordRequest passwordModel = new();

    private async Task ChangePasswordAsync()
    {
        var response = await accountHttpClient.ChangePasswordAsync(passwordModel);
        if (response.Succeeded)
        {
            snackbar.Add("Password Changed!", Severity.Success);
            passwordModel.Password = string.Empty;
            passwordModel.NewPassword = string.Empty;
            passwordModel.ConfirmNewPassword = string.Empty;
        }
        else
        {
            foreach (var error in response.Messages)
            {
                snackbar.Add(error, Severity.Error);
            }
        }
    }

    private bool currentPasswordVisibility;
    private InputType currentPasswordInput = InputType.Password;
    private string currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool newPasswordVisibility;
    private InputType newPasswordInput = InputType.Password;
    private string newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    private void TogglePasswordVisibility(bool newPassword)
    {
        if (newPassword)
        {
            if (newPasswordVisibility)
            {
                newPasswordVisibility = false;
                newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                newPasswordInput = InputType.Password;
            }
            else
            {
                newPasswordVisibility = true;
                newPasswordInputIcon = Icons.Material.Filled.Visibility;
                newPasswordInput = InputType.Text;
            }
        }
        else
        {
            if (currentPasswordVisibility)
            {
                currentPasswordVisibility = false;
                currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                currentPasswordInput = InputType.Password;
            }
            else
            {
                currentPasswordVisibility = true;
                currentPasswordInputIcon = Icons.Material.Filled.Visibility;
                currentPasswordInput = InputType.Text;
            }
        }
    }
}
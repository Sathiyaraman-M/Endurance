namespace Quark.Client.Pages.Identity;

public partial class RegisterUserModal
{
    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
    private readonly RegisterRequest _registerUserModel = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    private async Task SubmitAsync()
    {
        var response = await userHttpClient.RegisterUserAsync(_registerUserModel);
        if (response.Succeeded)
        {
            snackbar.Add(response.Messages[0], Severity.Success);
            MudDialog.Close();
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
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
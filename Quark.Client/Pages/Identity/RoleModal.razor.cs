namespace Quark.Client.Pages.Identity;

public partial class RoleModal
{
    [Parameter] public RoleRequest RoleModel { get; set; } = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    private async Task SaveAsync()
    {
        var response = await _roleHttpClient.SaveAsync(RoleModel);
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
}
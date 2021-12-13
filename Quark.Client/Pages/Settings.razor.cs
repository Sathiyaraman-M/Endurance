using Quark.Core.Configurations;

namespace Quark.Client.Pages;

public partial class Settings
{
    private LibrarySettings Model { get; set; } = new();
    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    protected override async Task OnInitializedAsync()
    {
        var response = await _settingsHttpClient.GetSettings();
        if (response.Succeeded)
        {
            Model = response.Data;
        }
        await base.OnInitializedAsync();
    }

    private async Task SaveSettingsAsync()
    {
        var response = await _settingsHttpClient.UpdateSettings(Model);
        if(response.Succeeded && Navigation.CanNavigateBack)
        {
            Navigation.NavigateBack();
            snackbar.Add(response.Messages[0], Severity.Success);
        }
    }
}
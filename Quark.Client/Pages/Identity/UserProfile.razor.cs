namespace Quark.Client.Pages.Identity;

public partial class UserProfile
{
    [Parameter]
    public string Id { get; set; }

    [Parameter] public string Title { get; set; }
    [Parameter] public string Description { get; set; }

    private bool _active;
    private char _firstLetterOfName;
    private string _fullName;
    private string _phoneNumber;
    private string _email;

    private bool _loaded;

    private async Task ToggleUserStatus()
    {
        var request = new ToggleUserStatusRequest { ActivateUser = _active, UserId = Id };
        var result = await userHttpClient.ToggleUserStatusAsync(request);
        if (result.Succeeded)
        {
            snackbar.Add("Updated User Status.", Severity.Success);
            navigationManager.NavigateTo("/identity/users");
        }
        else
        {
            foreach (var error in result.Messages)
            {
                snackbar.Add(error, Severity.Error);
            }
        }
    }

    [Parameter] public string ImageDataUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var userId = Id;
        var result = await userHttpClient.GetAsync(userId);
        if (result.Succeeded)
        {
            var user = result.Data;
            if (user != null)
            {
                _fullName = user.FullName;
                _email = user.Email;
                _phoneNumber = user.PhoneNumber;
                _active = user.IsActive;
                var data = await accountHttpClient.GetProfilePictureAsync(userId);
                if (data.Succeeded)
                {
                    ImageDataUrl = data.Data;
                }
            }
            Title = $"{_fullName}'s {"Profile"}";
            Description = _email;
            if (_fullName.Length > 0)
            {
                _firstLetterOfName = _fullName[0];
            }
        }

        _loaded = true;
    }
}
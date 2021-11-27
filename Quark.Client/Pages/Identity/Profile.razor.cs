using Microsoft.AspNetCore.Components.Forms;
using Quark.Core.Domain.Enums;

namespace Quark.Client.Pages.Authentication;

public partial class Profile
{
    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
    private char _firstLetterOfName;
    private readonly UpdateProfileRequest _profileModel = new();

    public string UserId { get; set; }

    private async Task UpdateProfileAsync()
    {
        var response = await accountHttpClient.UpdateProfileAsync(_profileModel);
        if (response.Succeeded)
        {
            await authenticationHttpClient.Logout();
            snackbar.Add("Your Profile has been updated. Please Login to Continue.", Severity.Success);
            navigationManager.NavigateTo("/");
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var state = await authStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        _profileModel.Email = user.GetEmail();
        _profileModel.UserName = user.GetUserName();
        _profileModel.FullName = user.GetFullName();
        _profileModel.PhoneNumber = user.GetPhoneNumber();
        UserId = user.GetUserId();
        var data = await accountHttpClient.GetProfilePictureAsync(UserId);
        if (data.Succeeded)
        {
            ImageDataUrl = data.Data;
        }
        if (_profileModel.FullName.Length > 0)
        {
            _firstLetterOfName = _profileModel.FullName[0];
        }
    }

    private IBrowserFile _file;

    [Parameter]
    public string ImageDataUrl { get; set; }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        _file = e.File;
        if (_file != null)
        {
            var extension = Path.GetExtension(_file.Name);
            var fileName = $"{UserId}-{Guid.NewGuid()}{extension}";
            var format = "image/png";
            var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
            var buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream().ReadAsync(buffer);
            var request = new UpdateProfilePictureRequest { Data = buffer, FileName = fileName, Extension = extension, UploadType = UploadType.ProfilePicture };
            var result = await accountHttpClient.UpdateProfilePictureAsync(request, UserId);
            if (result.Succeeded)
            {
                await localStorageService.SetItemAsync(StorageConstants.UserImageURL, result.Data);
                snackbar.Add("Profile picture added.", Severity.Success);
                navigationManager.NavigateTo("/account", true);
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    snackbar.Add(error, Severity.Error);
                }
            }
        }
    }

    private async Task DeleteAsync()
    {
        if ((await dialogService.ShowMessageBox("Confirm Delete?", String.Format("Are you sure want to delete the profile picture of {0}?", _profileModel.Email), yesText: "Yes", cancelText: "No")) == true)
        {
            var request = new UpdateProfilePictureRequest { Data = null, FileName = string.Empty, UploadType = UploadType.ProfilePicture };
            var data = await accountHttpClient.UpdateProfilePictureAsync(request, UserId);
            if (data.Succeeded)
            {
                await localStorageService.RemoveItemAsync(StorageConstants.UserImageURL);
                ImageDataUrl = string.Empty;
                snackbar.Add("Profile picture deleted.", Severity.Success);
                navigationManager.NavigateTo("/account", true);
            }
            else
            {
                foreach (var error in data.Messages)
                {
                    snackbar.Add(error, Severity.Error);
                }
            }
        }
    }
}
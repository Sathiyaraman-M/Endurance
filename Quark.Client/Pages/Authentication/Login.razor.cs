using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Quark.Core.Requests.Identity;
using System.Security.Claims;

namespace Quark.Client.Pages.Authentication;

public partial class Login
{
    private TokenRequest requestModel = new();

    protected override async Task OnInitializedAsync()
    {
        var state = await authStateProvider.GetAuthenticationStateAsync();
        if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            navigationManager.NavigateTo("/");
        }
    }

    private async Task SubmitAsync()
    {
        var result = await authenticationManager.Login(requestModel);
        if (result.Succeeded)
        {
            snackbar.Add(string.Format("Welcome {0}", requestModel.UserName), Severity.Success);
            navigationManager.NavigateTo("/", true);
        }
        else
        {
            foreach (var message in result.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    void TogglePasswordVisibility()
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
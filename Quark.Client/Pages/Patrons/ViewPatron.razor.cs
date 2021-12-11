using Quark.Core.Features.Patrons.Commands;

namespace Quark.Client.Pages.Patrons;

public partial class ViewPatron
{
    [Parameter]
    public Guid Id { get; set; }
    private PatronResponse Patron;

    private ClaimsPrincipal _currentUser;
    private bool _loaded = false;
    private bool _canEdit;
    private bool _canDelete;

    protected override async Task OnParametersSetAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canEdit = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.Edit)).Succeeded;
        _canDelete = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.Delete)).Succeeded;
        var response = await _patronHttpClient.GetByIdAsync(Id);
        if (response.Succeeded)
        {
            Patron = response.Data;
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

    private async Task InvokeModal()
    {
        var parameters = new DialogParameters();
        parameters.Add(nameof(AddEditPatronModal.Model), new AddEditPatronCommand
        {
            Id = Patron.Id,
            RegisterId = Patron.RegisterId,
            FirstName = Patron.FirstName,
            LastName = Patron.LastName,
            Address = Patron.Address,
            DateOfBirth = Patron.DateOfBirth,
            Email = Patron.Email,
            Mobile = Patron.Mobile,
            Issued = Patron.Issued,
            MultipleCheckoutLimit = Patron.MultipleCheckoutLimit
        });
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, Position = DialogPosition.TopCenter };
        await dialogService.Show<AddEditPatronModal>("Update", parameters, options).Result;
    }

    private async Task Delete()
    {
        if ((await dialogService.ShowMessageBox("Confirm Delete?", "Are you sure want to delete this patron?", yesText: "Delete", cancelText: "Cancel")) == true)
        {
            var response = await _patronHttpClient.DeleteAsync(Id);
            if(response.Succeeded)
            {
                snackbar.Add(response.Messages[0], Severity.Success);
                navigationManager.NavigateTo("administration/patrons");
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
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Interfaces.Services;
using Quark.Core.Interfaces.Services.Identity;
using Quark.Core.Requests.Identity;
using Quark.Infrastructure.Models.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Infrastructure.Services.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUploadService _uploadService;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUploadService uploadService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _uploadService = uploadService;
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return await Result.FailAsync("User Not Found.");
        }
        var identityResult = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
        var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
        return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
    }

    public async Task<IResult<string>> GetProfilePictureAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return await Result<string>.FailAsync("User Not Found");
        }
        return await Result<string>.SuccessAsync(data: user.ProfilePictureDataUrl);
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId)
    {
        if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return await Result.FailAsync(string.Format("Phone number {0} is already used.", model.PhoneNumber));
            }
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
        if (userWithSameEmail == null || userWithSameEmail.Id == userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result.FailAsync("User Not Found.");
            }
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            }
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
            await _signInManager.RefreshSignInAsync(user);
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }
        else
        {
            return await Result.FailAsync(string.Format("Email {0} is already used.", model.Email));
        }
    }

    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return await Result<string>.FailAsync(message: "User Not Found");
        var filePath = _uploadService.UploadAsync(request);
        user.ProfilePictureDataUrl = filePath;
        var identityResult = await _userManager.UpdateAsync(user);
        var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
        return identityResult.Succeeded ? await Result<string>.SuccessAsync(data: filePath) : await Result<string>.FailAsync(errors);
    }
}
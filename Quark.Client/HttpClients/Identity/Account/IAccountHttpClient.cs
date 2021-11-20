using Quark.Core.Requests.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Client.HttpClients.Identity.Account;

public interface IAccountHttpClient
{
    Task<IResult> ChangePasswordAsync(ChangePasswordRequest model);
    Task<IResult<string>> GetProfilePictureAsync(string userId);
    Task<IResult> UpdateProfileAsync(UpdateProfileRequest model);
    Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
}
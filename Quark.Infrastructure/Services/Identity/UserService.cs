using Hangfire;
using Microsoft.AspNetCore.WebUtilities;
using Quark.Core.Extensions;
using Quark.Core.Requests.Mail;
using Quark.Infrastructure.Specifications;
using Quark.Shared;
using System.Text;
using System.Text.Encodings.Web;

namespace Quark.Infrastructure.Services.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMailService _mailService;
    private readonly IExcelService _excelService;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService, IMailService mailService, IExcelService excelService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
        _mailService = mailService;
        _excelService = excelService;
    }

    public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        return result.Succeeded
            ? await Result<string>.SuccessAsync(user.Id, string.Format("Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT.", user.Email))
            : throw new ApiException(string.Format("An error occurred while confirming {0}", user.Email));
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            // Don't reveal that the user does not exist or is not confirmed
            return await Result.FailAsync("An Error has occurred!");
        }
        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest
        {
            Body = string.Format("Please reset your password by <a href='{0}'>clicking here</a>.", HtmlEncoder.Default.Encode(passwordResetURL)),
            Subject = "Reset Password",
            To = request.Email
        };
        BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest, origin));
        return await Result.SuccessAsync("Password Reset Mail has been sent to your authorized Email.");
    }

    public async Task<Result<List<UserResponse>>> GetAllAsync()
    {
        var result = _mapper.Map<List<UserResponse>>(await _userManager.Users.ToListAsync());
        return await Result<List<UserResponse>>.SuccessAsync(result);
    }

    public async Task<IResult<UserResponse>> GetAsync(string userId)
    {
        return await Result<UserResponse>.SuccessAsync(_mapper.Map<UserResponse>(await _userManager.FindByIdAsync(userId)));
    }

    public async Task<int> GetCountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string id)
    {
        var viewModel = new List<UserRoleModel>();
        var user = await _userManager.FindByIdAsync(id);
        var roles = await _roleManager.Roles.ToListAsync();

        foreach (var role in roles)
        {
            var userRolesViewModel = new UserRoleModel
            {
                RoleName = role.Name,
                RoleDescription = role.Description
            };
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            viewModel.Add(userRolesViewModel);
        }
        var result = new UserRolesResponse { UserRoles = viewModel };
        return await Result<UserRolesResponse>.SuccessAsync(result);
    }

    public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            return await Result.FailAsync(string.Format("Username {0} is already taken.", request.UserName));
        }
        var user = new ApplicationUser
        {
            Email = request.Email,
            FullName = request.FullName,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.ActivateUser,
            EmailConfirmed = request.AutoConfirmEmail
        };

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return await Result.FailAsync(string.Format("Phone number {0} is already registered.", request.PhoneNumber));
            }
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.BasicRole);
                if (!request.AutoConfirmEmail)
                {
                    var verificationUri = await SendVerificationEmail(user, origin);
                    var mailRequest = new MailRequest
                    {
                        From = "mail@wayne-enterprises.com",
                        To = user.Email,
                        Body = string.Format("Please confirm your account by <a href='{0}'>clicking here</a>.", verificationUri),
                        Subject = "Confirm Registration"
                    };
                    BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest, origin));
                    return await Result<string>.SuccessAsync(user.Id, string.Format("User {0} Registered. Please check your Mailbox to verify!", user.UserName));
                }
                return await Result<string>.SuccessAsync(user.Id, string.Format("User {0} Registered.", user.UserName));
            }
            else
            {
                return await Result.FailAsync(result.Errors.Select(a => a.Description.ToString()).ToList());
            }
        }
        else
        {
            return await Result.FailAsync(string.Format("Email {0} is already registered.", request.Email));
        }
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/identity/user/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        return verificationUri;
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return await Result.FailAsync("An Error has occured!");
        }
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        return result.Succeeded ? await Result.SuccessAsync("Password Reset Successful!") : await Result.FailAsync("An Error has occured!");
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
        var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
        if (isAdmin)
        {
            return await Result.FailAsync("Administrators Profile's Status cannot be toggled");
        }
        if (user is not null)
        {
            user.IsActive = request.ActivateUser;
            var identityResult = await _userManager.UpdateAsync(user);
        }
        return await Result.SuccessAsync();
    }

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user.Email == "admin@wayne-enterprises.com")
        {
            return await Result.FailAsync("Not Allowed.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
        {
            var tryToAddAdministratorRole = selectedRoles
                .Any(x => x.RoleName == RoleConstants.AdministratorRole);
            var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
            if (tryToAddAdministratorRole && !userHasAdministratorRole || !tryToAddAdministratorRole && userHasAdministratorRole)
            {
                return await Result.FailAsync("Not Allowed to add or delete Administrator Role if you have not this role.");
            }
        }

        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
        return await Result.SuccessAsync("Roles Updated");
    }

    public async Task<string> ExportToExcelAsync(string searchString = "")
    {
        var userSpec = new UserFilterSpecification(searchString);
        var users = await _userManager.Users
            .Specify(userSpec)
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync();
        var result = await _excelService.ExportAsync(users,
            new Dictionary<string, Func<ApplicationUser, object>>
            {
                    { "Id", item => item.Id },
                    { "UserName", item => item.UserName },
                    { "FirstName", item => item.FullName },
                    { "Email", item => item.Email },
                    { "EmailConfirmed", item => item.EmailConfirmed },
                    { "PhoneNumber", item => item.PhoneNumber },
                    { "PhoneNumberConfirmed", item => item.PhoneNumberConfirmed },
                    { "IsActive", item => item.IsActive },
                    { "CreatedOn", item => item.CreatedOn.ToString("g") },
                    { "ProfilePictureDataUrl", item => item.ProfilePictureDataUrl },
            }, "Users");

        return result;
    }
}
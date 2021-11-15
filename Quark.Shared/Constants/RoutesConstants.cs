namespace Quark.Shared.Constants;

public static class Routes
{
    public const string DashboardRoute = "api/dashboard";

    public static class BookEndpoints
    {
        public const string BaseRoute = "api/books";

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
            => Routes.GetAllPaged(BaseRoute, pageNumber, pageSize, searchString, orderBy);
    }

    public static class PatronEndpoints
    {
        public const string BaseRoute = "api/patrons";

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
            => Routes.GetAllPaged(BaseRoute, pageNumber, pageSize, searchString, orderBy);
    }

    public static class DesignationEndpoints
    {
        public const string BaseRoute = "api/designations";

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
            => Routes.GetAllPaged(BaseRoute, pageNumber, pageSize, searchString, orderBy);
    }

    public static class CheckoutEndpoints
    {
        public const string BaseRoute = "api/checkouts";
        public const string ExtendCheckoutRoute = "api/checkouts/extend";
        public const string CheckInRoute = "api/checkouts/close";

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
            => Routes.GetAllPaged(BaseRoute, pageNumber, pageSize, searchString, orderBy);
    }

    public static string GetAllPaged(string baseRoute, int pageNumber, int pageSize, string searchString, string[] orderBy)
    {
        var url = $"{baseRoute}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
        if (orderBy?.Any() == true)
        {
            foreach (var orderByPart in orderBy)
            {
                url += $"{orderByPart},";
            }
            url = url[..^1];
        }
        return url;
    }

    public static class UserEndpoints
    {
        public const string BaseRoute = "api/identity/user";
        public const string GetById = "api/identity/user/{id}";
        public const string Roles = "api/identity/user/roles/{id}";
        public const string ConfirmEmail = "api/identity/user/confirm-email";
        public const string ToggleActivation = "api/identity/user/toggle-activate";
        public const string ForgotPassword = "api/identity/user/forgot-password";
        public const string ResetPassword = "api/identity/user/reset-password";
    }

    public static class RolesEndpoints
    {
        public const string Delete = "api/identity/role";
        public const string GetAll = "api/identity/role";
        public const string Save = "api/identity/role";
        public const string GetPermissions = "api/identity/role/permissions/";
        public const string UpdatePermissions = "api/identity/role/permissions/update";
    }

    public static class TokenEndpoints
    {
        public const string Get = "api/identity/token";
        public const string Refresh = "api/identity/token/refresh";
    }

    public static class AuditTrailEndpoints
    {
        public const string GetCurrentUserTrails = "api/audit/current";
        public const string GetAllTrails = "api/audit";
    }

    public static class AccountEndpoints
    {
        public const string UpdateProfile = "api/identity/account/update-profile";
        public const string ChangePassword = "api/identity/account/change-password";
        public const string ProfilePicture = "api/identity/account/profile-picture/{userId}";
        public const string ProfilePictureShort = "api/identity/account/profile-picture";
    }
}
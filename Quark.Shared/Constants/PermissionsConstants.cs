using System.Reflection;

namespace Quark.Shared.Constants.Permission;

public static class ApplicationClaimTypes
{
    public const string Permission = "Permission";
}

public static class Permissions
{
    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Edit = "Permissions.Users.Edit";
        public const string Delete = "Permissions.Users.Delete";
        public const string Export = "Permissions.Users.Export";
    }

    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string Create = "Permissions.Roles.Create";
        public const string Edit = "Permissions.Roles.Edit";
        public const string Delete = "Permissions.Roles.Delete";
    }

    public static class RoleClaims
    {
        public const string View = "Permissions.RoleClaims.View";
        public const string Create = "Permissions.RoleClaims.Create";
        public const string Edit = "Permissions.RoleClaims.Edit";
        public const string Delete = "Permissions.RoleClaims.Delete";
    }

    public static class AuditTrails
    {
        public const string View = "Permissions.AuditTrails.View";
        public const string ViewAll = "Permissions.AuditTrails.ViewAll";
        public const string Export = "Permissions.AuditTrails.Export";
    }

    public static class Designations
    {
        public const string View = "Permissions.Designations.View";
        public const string Create = "Permissions.Designations.Create";
        public const string Edit = "Permissions.Designations.Edit";
        public const string Delete = "Permissions.Designations.Delete";
        public const string Export = "Permissions.Designations.Export";
    }

    public static class Patrons
    {
        public const string View = "Permissions.Patrons.View";
        public const string Create = "Permissions.Patrons.Create";
        public const string Edit = "Permissions.Patrons.Edit";
        public const string Delete = "Permissions.Patrons.Delete";
        public const string Export = "Permissions.Patrons.Export";
    }

    public static class Books
    {
        public const string View = "Permissions.Books.View";
        public const string Create = "Permissions.Books.Create";
        public const string Edit = "Permissions.Books.Edit";
        public const string Delete = "Permissions.Books.Delete";
        public const string Export = "Permissions.Books.Export";
    }

    public static class Checkouts
    {
        public const string View = "Permissions.Checkouts.View";
        public const string Create = "Permissions.Checkouts.Create";
        public const string Edit = "Permissions.Checkouts.Edit";
        public const string Delete = "Permissions.Checkouts.Delete";
        public const string Export = "Permissions.Checkouts.Export";
    }

    public static class Hangfire
    {
        public const string View = "Permissions.Hangfire.View";
    }

    public static class Dashboard
    {
        public const string View = "Permissions.Dashboard.View";
    }

    public static class Settings
    {
        public const string View = "Permissions.Settings.View";
        public const string Update = "Permissions.Settings.Update";
    }

    public static IEnumerable<string> GetRegisteredPermissions()
    {
        //var permssions = new List<string>();
        foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
        {
            var propertyValue = prop.GetValue(null);
            if (propertyValue is not null)
                yield return propertyValue.ToString();
            //permissions.Add(propertyValue.ToString());
        }
        //return permssions;
    }
}

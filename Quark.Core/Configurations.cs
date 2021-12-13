namespace Quark.Core.Configurations;

public class AppConfiguration
{
    public string Secret { get; set; }
}

public class MailConfiguration
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSSL { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
}

public class LibrarySettings
{
    public decimal CheckInDelayFinePerDay { get; set; }
    public int DefaultMultipleCheckoutLimit { get; set; }
}
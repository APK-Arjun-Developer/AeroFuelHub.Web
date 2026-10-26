namespace AeroFuelHub.Web.Models.Common;

public class DemoCredentialsOptions
{
    public const string SectionName = "DemoCredentials";

    public bool Enabled { get; set; }

    public List<DemoAccount> Accounts { get; set; } = new();
}

public class DemoAccount
{
    public string Role { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Icon { get; set; } = "fa-user";
}

using System.Diagnostics.CodeAnalysis;

namespace AutomationPractice.Common;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public class SessionSettings
{
    public Browsers Browser { get; set; }
    public string DriverPath { get; set; }=string.Empty;
    public bool Headless { get; set; }
    public string DownloadDirectory { get; set; } = string.Empty;
    public uint DefaultTimeoutSeconds { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? ApplicationUrl { get; set; }
    public string? StoragePath { get; set; }
}
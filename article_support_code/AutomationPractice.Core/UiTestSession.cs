using AutomationPractice.Common;
using AutomationPractice.Core.DI;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationPractice.Core;

public class UiTestSession
{
    private IServiceProvider _services = null!;

    public SessionSettings Settings => Resolve<SessionSettings>();

    public static UiTestSession Current => InstanceFactory.Value;

    private static readonly Lazy<UiTestSession> InstanceFactory = new(() => new UiTestSession());

    private UiTestSession()
    {
    }

    public void Start()
    {
        _services = ServiceRegistry.Register();
        if (!string.IsNullOrWhiteSpace(Settings.DownloadDirectory) && !Directory.Exists(Settings.DownloadDirectory))
        {
            Directory.CreateDirectory(Settings.DownloadDirectory);
        }
    }

    public void CleanUp()
    {
        if (Directory.Exists(Settings.DownloadDirectory))
        {
            Directory.Delete(Settings.DownloadDirectory, true);
        }
    }

    public T Resolve<T>() where T : notnull
    {
        if (_services == null)
        {
            throw new InvalidOperationException("The session is not started");
        }

        return _services.GetRequiredService<T>();
    }
}
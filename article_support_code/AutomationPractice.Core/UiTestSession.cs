using AutomationPractice.Common;
using AutomationPractice.Core.DI;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AutomationPractice.Core;

public class UiTestSession
{
    private IServiceProvider _services = null!;
    public TestContext TestContext => TestContext.CurrentContext;
    public TestExecutionContext ExecutionContext => TestExecutionContext.CurrentContext;

    public SessionSettings Settings { get; }

    public static UiTestSession Current => InstanceFactory.Value;
    public ILogger? Logger { get; set; }

    public string SessionId { get; internal set; }

    private static readonly Lazy<UiTestSession> InstanceFactory = new(() => new UiTestSession());

    private UiTestSession()
    {
        _services = ServiceRegistry.Register();
        SessionId = Guid.NewGuid().ToString();
        Settings = Resolve<SessionSettings>();
    }

    public void Start()
    {
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
        if( typeof(T).Equals(typeof(IServiceProvider)))
        {
            return (T)_services;
        }
        return _services.GetRequiredService<T>();
    }
}
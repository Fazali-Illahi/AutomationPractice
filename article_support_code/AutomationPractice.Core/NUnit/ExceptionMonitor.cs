namespace AutomationPractice.UiTests;

public static class ExceptionMonitor
{
    public static event EventHandler<Exception>? Handler;

    public static void OnException(object sender, Exception ex)
    {
        Handler?.Invoke(sender,ex);
    }
}

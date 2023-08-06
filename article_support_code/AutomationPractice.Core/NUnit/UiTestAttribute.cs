using AutomationPractice.Core;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AutomationPractice.UiTests;

[AttributeUsage(AttributeTargets.Method)]
public class UiTestAttribute : TestAttribute, IWrapSetUpTearDown
{
    public TestCommand Wrap(TestCommand command)
    {
        return new TestExecutionCommand(command);
    }

    public class TestExecutionCommand : AfterTestCommand
    {
        private readonly ILogger _logger = UiTestSession.Current.Logger!;
        private TestCommand _command;
        public TestExecutionCommand(TestCommand innerCommand) : base(innerCommand)
        {
            _command = innerCommand;
        }

        public override TestResult Execute(TestExecutionContext context)
        {
            _logger.LogInformation($"Test Started: {context.CurrentTest.Name}");
            context.CurrentResult.StartTime = DateTime.UtcNow;
            try
            {
                context.CurrentResult = _command.Execute(context);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not AssertionException)
                {
                    if (context.CurrentResult == null)
                        context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentResult.RecordException(ex);
                    context.CurrentResult.SetResult(ResultState.Error);
                    ExceptionMonitor.OnException(context.CurrentTest.Name,ex);
                    throw;
                }
            }
            finally
            {
                context.CurrentResult.EndTime = DateTime.UtcNow;
                context.CurrentResult.Duration = (context.CurrentResult.EndTime - context.CurrentResult.StartTime).TotalSeconds;                
                LogMessage(context.CurrentResult);
            }
            return context.CurrentResult;
        }
        private void LogMessage(TestResult result)
        {
            var message = $"Test {result.ResultState.Status}: {result.Test.Name} in {result.Duration:0.0} seconds";            
            switch (result.ResultState.Status)
            {
                case TestStatus.Passed:
                    _logger.LogInformation(message);
                    break;
                case TestStatus.Inconclusive:
                case TestStatus.Skipped:                
                case TestStatus.Warning:
                    _logger.LogWarning(message);
                    break;
                case TestStatus.Failed:
                    _logger.LogError(message);
                    break;
            }
        }
    }
}

using AutomationPractice.Core;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;

namespace AutomationPractice.UiTests
{
    [SetUpFixture]
    public class TestSessionStartup
    {
        [OneTimeSetUp]
        public void StartSession()
        {
            UiTestSession.Current.Start();
            ExceptionMonitor.Handler += OnUncaughtException; 
        }

        private void OnUncaughtException(object? sender, Exception e)
        {
            UiTestSession.Current.Logger!.LogCritical($"{sender} threw an error.{Environment.NewLine}{e.Message}");
        }

     

        [OneTimeTearDown]
        public void EndSession()
        {
            UiTestSession.Current.CleanUp();
        }
    }
}

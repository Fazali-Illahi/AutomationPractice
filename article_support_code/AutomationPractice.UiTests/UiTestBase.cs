using AutomationPractice.Core;
using AutomationPractice.Core.PageObjects;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AutomationPractice.UiTests
{
    public class UiTestBase<T> where T : UiPageBase<T>
    {
        public T Page { get; }= UiTestSession.Current.Resolve<T>();
        public UiTestBase()
        {
            UiTestSession.Current.Logger = UiTestSession.Current.Resolve<ILogger<T>>();
        }

        [OneTimeTearDown]
        public void CleanupSuite()
        {
            Page.Dispose();
        }
    }
}
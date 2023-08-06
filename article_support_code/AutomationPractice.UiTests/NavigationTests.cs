using AutomationPractice.Core.PageObjects;
using NUnit.Framework;

namespace AutomationPractice.UiTests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class NavigationTests : UiTestBase<HomePage>
{
    [UiTest]
    public void NavigateToHomePageTest()
    {
        var shopPage = Page.Open().OpenShop();
        Assert.That(shopPage, Is.Not.Null);
        Assert.That(shopPage.IsOpen, Is.True);
    }

    [UiTest]
    public void NavigateToMyAccountPageTest()
    {
        var accountPage = Page.Open().OpenMyAccountPage();
        Assert.That(accountPage, Is.Not.Null);
        Assert.That(accountPage.IsOpen, Is.True);
    }

    [UiTest]
    public void NavigateToTestCasePageTest()
    {
        var testCasePage = Page.Open().OpenTestCasePage();
        Assert.That(testCasePage, Is.Not.Null);
        Assert.That(testCasePage.IsOpen, Is.True);
    }
}

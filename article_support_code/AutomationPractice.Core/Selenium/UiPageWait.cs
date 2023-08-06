using AutomationPractice.Core.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace AutomationPractice.Core.Selenium
{
    public class UiPageWait<Page> : DefaultWait<Page> where Page : UiPageBase<Page>
    {
        public UiPageWait(Page input) : base(input)
        {
        }
        public UiPageWait(Page input, TimeSpan timeOut) : base(input)
        {
            Timeout = timeOut;
        }
    }
}

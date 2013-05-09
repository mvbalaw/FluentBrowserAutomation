using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class LinkWrapper : BasicInfoWrapper, INavigationControl
	{
		public LinkWrapper(IWebElement link, string howFound, IBrowserContext browserContext)
			: base(link, howFound, browserContext)
		{
		}
	}
}
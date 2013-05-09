using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ContainerWrapper : BasicInfoWrapper
	{
		public ContainerWrapper(IWebElement div, string howFound, IBrowserContext browserContext)
			: base(div, howFound, browserContext)
		{
		}
	}
}
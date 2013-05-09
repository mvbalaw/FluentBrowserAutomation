using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class DivWrapper : BasicInfoWrapper
	{
		public DivWrapper(IWebElement div, string howFound, IBrowserContext browserContext)
			: base(div, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
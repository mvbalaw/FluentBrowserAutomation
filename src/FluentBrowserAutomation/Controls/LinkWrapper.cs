using FluentBrowserAutomation.Accessors;
using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class LinkWrapper : BasicInfoWrapper, INavigationControl
	{
		public LinkWrapper(IWebElement link, string howFound, IBrowserContext browserContext)
			: base(link, howFound, browserContext)
		{
		}

		public string Text
		{
			get { return new ReadOnlyText("text of " + HowFound, Element.GetAttribute("text")); }
		}
	}
}
using FluentBrowserAutomation.Accessors;
using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ImageWrapper : BasicInfoWrapper, INavigationControl
	{
		public ImageWrapper(IWebElement webElement, string howFound, IBrowserContext browser)
			: base(webElement, howFound, browser)
		{
		}

		public string Text
		{
			get { return new ReadOnlyText("alt of " + HowFound, Element.GetAttribute("alt")); }
		}
	}
}
using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ImageWrapper : BasicInfoWrapper, INavigationControl
	{
		public ImageWrapper(IWebElement webElement, string howFound, IBrowserContext browser)
			: base(webElement, howFound, browser)
		{
		}
	}
}
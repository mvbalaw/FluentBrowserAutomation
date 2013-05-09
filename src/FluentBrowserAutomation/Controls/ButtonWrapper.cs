using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ButtonWrapper : BasicInfoWrapper, INavigationControl
	{
		public ButtonWrapper(IWebElement button, string howFound, IBrowserContext browser)
			: base(button, howFound, browser)
		{
		}
	}
}
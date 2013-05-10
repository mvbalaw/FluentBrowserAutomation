using FluentBrowserAutomation.Accessors;
using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ButtonWrapper : BasicInfoWrapper, INavigationControl
	{
		public ButtonWrapper(IWebElement button, string howFound, IBrowserContext browser)
			: base(button, howFound, browser)
		{
		}

		public string Text
		{
			get { return new ReadOnlyText("value of " + HowFound, Element.GetAttribute("value")); }
		}
	}
}
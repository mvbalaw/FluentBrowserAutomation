using FluentBrowserAutomation.Accessors;
using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ButtonWrapper : BasicInfoWrapper, INavigationControl
	{
		private string _text;

		public ButtonWrapper(IWebElement button, string howFound, IBrowserContext browser)
			: base(button, howFound, browser)
		{
		}

		public string Text
		{
			get
			{
				if (_text == null)
				{
					_text = Element.TagName == "button" ? Element.Text : Element.GetAttribute("value");
				}
				return new ReadOnlyText("value of " + HowFound, _text);
			}
		}
	}
}
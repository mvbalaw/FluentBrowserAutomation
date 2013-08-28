using FluentBrowserAutomation.Accessors;
using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class LinkWrapper : BasicInfoWrapper, INavigationControl
	{
		private string _text;

		public LinkWrapper(IWebElement link, string howFound, IBrowserContext browserContext)
			: base(link, howFound, browserContext)
		{
		}

		public string Text
		{
			get
			{
				if (_text == null)
				{
					_text = Element.GetAttribute("text");
				}
				return new ReadOnlyText("text of " + HowFound, _text);
			}
		}
	}
}
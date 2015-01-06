using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class ButtonWrapper : BasicInfoWrapper, INavigationControl
	{
		public ButtonWrapper(RemoteWebElementWrapper button, string howFound, IBrowserContext browser)
			: base(button, howFound, browser)
		{
		}

		private string _text;

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
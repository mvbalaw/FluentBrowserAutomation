using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class LinkWrapper : BasicInfoWrapper, INavigationControl
	{
		public LinkWrapper(RemoteWebElementWrapper link, string howFound, IBrowserContext browserContext)
			: base(link, howFound, browserContext)
		{
		}

		private string _text;

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
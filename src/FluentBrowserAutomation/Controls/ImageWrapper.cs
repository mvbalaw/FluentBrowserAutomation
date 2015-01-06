using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class ImageWrapper : BasicInfoWrapper, INavigationControl
	{
		public ImageWrapper(RemoteWebElementWrapper webElement, string howFound, IBrowserContext browser)
			: base(webElement, howFound, browser)
		{
		}

		public string Text
		{
			get { return new ReadOnlyText("alt of " + HowFound, Element.GetAttribute("alt")); }
		}
	}
}
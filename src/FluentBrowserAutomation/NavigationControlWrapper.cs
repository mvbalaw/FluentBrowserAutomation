using FluentAssert;

using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation
{
	public static class INavigationControlExtensions
	{
		public static ButtonWrapper AsButton(this INavigationControl navigationControl)
		{
			var button = navigationControl as ButtonWrapper;
			if (button != null)
			{
				return button;
			}
			var elem = navigationControl.Element.IsButton() ? navigationControl.Element : null;
			return new ButtonWrapper(elem, navigationControl.HowFound, navigationControl.BrowserContext);
		}

		public static LinkWrapper AsLink(this INavigationControl navigationControl)
		{
			var link = navigationControl as LinkWrapper;
			if (link != null)
			{
				return link;
			}
			var elem = navigationControl.Element.IsLink() ? navigationControl.Element : null;
			return new LinkWrapper(elem, navigationControl.HowFound, navigationControl.BrowserContext);
		}

		public static bool IsButton(this INavigationControl navigationControl)
		{
			return navigationControl is ButtonWrapper || navigationControl.Element.IsButton();
		}

		public static bool IsLink(this INavigationControl navigationControl)
		{
			return navigationControl is LinkWrapper || navigationControl.Element.IsLink();
		}

		public static INavigationControl ShouldContainText(this INavigationControl navigationControl, string text)
		{
			navigationControl.BrowserContext.WaitUntil(x => navigationControl.IsVisible().IsTrue, errorMessage:"wait for " + navigationControl.HowFound + " to be visible");
			navigationControl.Element.Text.Contains(text).ShouldBeTrue();
			return navigationControl;
		}
	}
}
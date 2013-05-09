using FluentAssert;

using FluentBrowserAutomation.Controls;

namespace FluentBrowserAutomation
{
	public static class INavigationControlExtensions
	{
		public static INavigationControl ShouldBeVisible(this INavigationControl navigationControl)
		{
			navigationControl.IsVisible().ShouldBeTrue();
			return navigationControl;
		}

		public static INavigationControl ShouldContainText(this INavigationControl navigationControl, string text)
		{
			navigationControl.Element.Text.Contains(text).ShouldBeTrue();
			return navigationControl;
		}

		public static INavigationControl ShouldNotBeVisible(this INavigationControl navigationControl)
		{
			navigationControl.IsVisible().ShouldBeFalse();
			return navigationControl;
		}
	}
}
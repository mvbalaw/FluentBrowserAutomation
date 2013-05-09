using System;

//// ReSharper disable CheckNamespace
namespace FluentBrowserAutomation
//// ReSharper restore CheckNamespace
{
	public interface ICanBeClicked : IAmVisualElement, IHaveBasicInfo
	{
	}

	public static class ICanBeClickedExtensions
	{
		public static void Click(this ICanBeClicked element)
		{
			element.Exists().ShouldBeTrue();
			var disableable = element as ICouldBeDisabled;
			if (disableable != null)
			{
				disableable.IsEnabled().ShouldBeTrue();
			}
			var needsFoscus = element as INeedFocus;
			if (needsFoscus != null)
			{
				element.Focus();
			}
			element.BrowserContext.WaitUntil(x => ClickIt(element));
		}

		private static bool ClickIt(ICanBeClicked element)
		{
			try
			{
				element.Element.Click();
				return true;
			}
			catch (InvalidOperationException invalidOperationException)
			{
				if (invalidOperationException.Message.Contains("Other element would receive the click"))
				{
					element.ScrollToIt();
					element.Focus();
					element.Element.Click();
					return true;
				}
			}
			return false;
		}
	}
}
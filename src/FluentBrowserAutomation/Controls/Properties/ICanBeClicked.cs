using System;

using OpenQA.Selenium.Remote;

// ReSharper disable once CheckNamespace
namespace FluentBrowserAutomation
{
// ReSharper disable once RedundantExtendsListEntry
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

// ReSharper disable once SuggestBaseTypeForParameter
		private static bool ClickIt(ICanBeClicked element)
		{
			try
			{
				element.Element.Click();
				return true;
			}
			catch (InvalidOperationException invalidOperationException)
			{
				element.Focus();
				if (invalidOperationException.Message.Contains("Other element would receive the click"))
				{
					// try scrolling down to the element
					for (var yOffset = 50; yOffset < ((RemoteWebElement)element.Element).Location.Y; yOffset += 250)
					{
						try
						{
							element.ScrollToIt(yOffset);
							element.Element.Click();
							return true;
						}
						catch (InvalidOperationException invalidOperationException2)
						{
							if (!invalidOperationException2.Message.Contains("Other element would receive the click"))
							{
								throw;
							}
						}
					}
				}
			}
			return false;
		}
	}
}
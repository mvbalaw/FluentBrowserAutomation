using OpenQA.Selenium;

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
			element.BrowserContext.WaitUntil(x => element.IsVisible().IsTrue, errorMessage:"wait for " + element.HowFound + " to be visible in Click");

			var couldBeDisabled = element as ICouldBeDisabled;
			if (couldBeDisabled != null)
			{
				element.BrowserContext.WaitUntil(x => couldBeDisabled.IsEnabled().IsTrue, errorMessage:"wait for " + element.HowFound + " to be enabled in Click");
			}
			var needsFocus = element as INeedFocus;
			if (needsFocus != null)
			{
				element.Focus();
			}
			element.BrowserContext.WaitUntil(x => ClickIt(element));
		}

// ReSharper disable once SuggestBaseTypeForParameter
		private static bool ClickIt(ICanBeClicked element)
		{
			var webElement = element.Element;
			try
			{
				webElement.Click();
				element.BrowserContext.WaitForPendingRequests();
				return true;
			}
			catch (WebDriverException exception)
			{
				element.Focus();
                if (exception.Message.Contains("Other element would receive the click") || exception.Message.Contains("Element is not clickable at point"))
				{
					// try scrolling down to the element
					var yLocation = webElement.RemoteWebElement.Location.Y + 100;
					for (var yOffset = 50; yOffset < yLocation; yOffset += 250)
					{
						try
						{
							element.ScrollToIt(yOffset);
							webElement.Click();
							element.BrowserContext.WaitForPendingRequests();
							return true;
						}
						catch (WebDriverException webDriverException)
						{
                            if (!webDriverException.Message.Contains("Other element would receive the click") && !exception.Message.Contains("Element is not clickable at point"))
							{
								throw new AssertionException(exception);
							}
						}
					}
				}
			}
			return false;
		}
	}
}
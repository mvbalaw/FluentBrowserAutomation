using System;

//// ReSharper disable CheckNamespace
// ReSharper disable CheckNamespace

namespace FluentBrowserAutomation
// ReSharper restore CheckNamespace
//// ReSharper restore CheckNamespace
{
// ReSharper disable RedundantExtendsListEntry
	public interface ICanBeClicked : IAmVisualElement, IHaveBasicInfo
// ReSharper restore RedundantExtendsListEntry
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

// ReSharper disable SuggestBaseTypeForParameter
		private static bool ClickIt(ICanBeClicked element)
// ReSharper restore SuggestBaseTypeForParameter
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
					// try scrolling up and down relative to the element
					for (var yOffset = 0; yOffset < 300; yOffset += 50)
					{
						try
						{
							element.ScrollToIt(yOffset);
							element.Focus();
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
					for (var yOffset = 50; yOffset < 300; yOffset += 50)
					{
						try
						{
							element.ScrollToIt(-yOffset);
							element.Focus();
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
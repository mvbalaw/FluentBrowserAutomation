using System;
using System.Collections.Generic;

using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

using OpenQA.Selenium;

//// ReSharper disable CheckNamespace

namespace FluentBrowserAutomation
//// ReSharper restore CheckNamespace
{
	public interface IHaveBasicInfo
	{
		IBrowserContext BrowserContext { get; }
		IWebElement Element { get; }
		string HowFound { get; }
		string Id { get; }
	}

	public static class IBasicInfoExtensions
	{
		public static Children Children(this IHaveBasicInfo element)
		{
			return new Children(element.Element, element.HowFound + " .Children", element.BrowserContext);
		}

		public static IReadOnlyBooleanState Exists(this IHaveBasicInfo element)
		{
			const string unexpectedlyFalse = "{0} does not exist but should.";
			const string unexpectedlyTrue = "{0} exists but should not.";
			var unexpectedlyTrueMessage = String.Format(unexpectedlyTrue, element.HowFound);
			var unexpectedlyFalseMessage = String.Format(unexpectedlyFalse, element.HowFound);
			var result = new BooleanState(
				unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() => element.Element != null);
			return result;
		}

		private static KeyValuePair<bool, string> IsDisplayed(IWebElement element)
		{
			// adapted from http://blog.coditate.com/2009/07/determining-html-element-visibility.html
			if (!element.Displayed)
			{
				return new KeyValuePair<bool, string>(false, element.TagName);
			}
			if (string.Equals(element.TagName, "form", StringComparison.InvariantCultureIgnoreCase))
			{
				return new KeyValuePair<bool, string>(true, null);
			}
			if (element.GetParent() != null)
			{
				var result = IsDisplayed(element.GetParent());
				if (result.Key)
				{
					return result;
				}
				return new KeyValuePair<bool, string>(false, element.TagName + "." + result.Value);
			}
			return new KeyValuePair<bool, string>(true, null);
		}

		public static IReadOnlyBooleanState IsVisible(this IHaveBasicInfo element)
		{
			element.Exists().ShouldBeTrue();
			const string unexpectedlyFalse = "{0} should be visible but is not because {1} is marked display: none .";
			const string unexpectedlyTrue = "{0} should not be visible but is.";
			var visibility = IsDisplayed(element.Element);
			string parent;
			if (visibility.Value != null)
			{
				parent = "its parent tag " + visibility.Value;
			}
			else
			{
				parent = "it";
			}
			var unexpectedlyTrueMessage = String.Format(unexpectedlyTrue, element.HowFound);
			var unexpectedlyFalseMessage = String.Format(unexpectedlyFalse, element.HowFound, parent);

			var result = new BooleanState(unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() => visibility.Key);
			return result;
		}
	}
}
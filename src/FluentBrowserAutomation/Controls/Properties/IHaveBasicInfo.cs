using System;
using System.Collections.Generic;

using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

//// ReSharper disable CheckNamespace
// ReSharper disable CheckNamespace

namespace FluentBrowserAutomation
// ReSharper restore CheckNamespace
//// ReSharper restore CheckNamespace
{
	public interface IHaveBasicInfo
	{
		IBrowserContext BrowserContext { get; }
		RemoteWebElementWrapper Element { get; }
		string HowFound { get; }
		string Id { get; }
		string Name { get; }
	}

	public static class IHaveBasicInfoExtensions
	{
		public static CheckBoxWrapper AsCheckBox(this IHaveBasicInfo element, string howFound = null)
		{
			if (element is CheckBoxWrapper)
			{
				return element as CheckBoxWrapper;
			}
			var elem = element.Element.IsCheckBox() ? element.Element : null;
			return new CheckBoxWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static DropDownListWrapper AsDropDownList(this IHaveBasicInfo element, string howFound = null)
		{
			if (element is DropDownListWrapper)
			{
				return element as DropDownListWrapper;
			}
			var elem = element.Element.IsDropDownList() ? element.Element : null;
			return new DropDownListWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static FileUploadWrapper AsFileUpload(this IHaveBasicInfo element)
		{
			if (element is FileUploadWrapper)
			{
				return element as FileUploadWrapper;
			}
			var elem = element.Element.IsFileUpload() ? element.Element : null;
			return new FileUploadWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static RadioOptionWrapper AsRadioOption(this IHaveBasicInfo element, string howFound = null)
		{
			if (element is RadioOptionWrapper)
			{
				return element as RadioOptionWrapper;
			}
			var elem = element.Element.IsRadioOption() ? element.Element : null;
			return new RadioOptionWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static TextBoxWrapper AsTextBox(this IHaveBasicInfo element)
		{
			if (element is TextBoxWrapper)
			{
				return element as TextBoxWrapper;
			}
			var elem = element.Element.IsTextBox() ? element.Element : null;
			return new TextBoxWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static void AssertIsCheckBox(this IHaveBasicInfo input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist");
			if (!input.IsCheckBox())
			{
				throw new AssertionException(input.HowFound + " is not a checkbox.");
			}
		}

		public static void AssertIsDropDownList(this IHaveBasicInfo input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist");
			if (!IsDropDownList(input))
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
		}

		public static void AssertIsRadioOption(this IHaveBasicInfo input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist");
			if (!input.IsRadioOption())
			{
				throw new AssertionException(input.HowFound + " is not a radio option.");
			}
		}

		public static Children Children(this IHaveBasicInfo element)
		{
			return new Children(element.Element, element.HowFound + " .Children", element.BrowserContext);
		}

		public static IReadOnlyBooleanState Exists(this IHaveBasicInfo element)
		{
			const string unexpectedlyFalse = "{0} does not exist but should.";
			const string unexpectedlyTrue = "{0} exists but should not.";
			var unexpectedlyTrueMessage = string.Format(unexpectedlyTrue, element.HowFound);
			var unexpectedlyFalseMessage = string.Format(unexpectedlyFalse, element.HowFound);
			var result = new BooleanState(
				unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() =>
				{
					try
					{
						return element.Element.Exists;
					}
					catch (Exception)
					{
						return false;
					}
				});
			return result;
		}

		public static bool IsCheckBox(this IHaveBasicInfo element)
		{
			return element is CheckBoxWrapper || element.Element.IsCheckBox();
		}

		private static KeyValuePair<bool, string> IsDisplayed(RemoteWebElementWrapper element)
		{
			if (!element.Displayed || element.GetAttribute("class").Contains("ng-hide"))
			{
				return new KeyValuePair<bool, string>(false, element.TagName);
			}
			return new KeyValuePair<bool, string>(true, null);
		}

		public static bool IsDropDownList(this IHaveBasicInfo element)
		{
			return element is DropDownListWrapper || element.Element.IsDropDownList();
		}

		public static bool IsFileUpload(this IHaveBasicInfo element)
		{
			return element is FileUploadWrapper || element.Element.IsFileUpload();
		}

		public static bool IsNotVisible(this IHaveBasicInfo element)
		{
			return element.Exists().IsFalse || element.IsVisible().IsFalse;
		}

		public static bool IsRadioOption(this IHaveBasicInfo element)
		{
			return element is RadioOptionWrapper || element.Element.IsRadioOption();
		}

		public static bool IsTextBox(this IHaveBasicInfo element)
		{
			return element is TextBoxWrapper || element.Element.IsTextBox();
		}

		/// <summary>
		///     checks for existence and visibility
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static IReadOnlyBooleanState IsVisible(this IHaveBasicInfo element)
		{
			try
			{
				element.BrowserContext.WaitUntil(x => element.Exists().IsTrue, errorMessage:"wait for " + element.HowFound + " to exist.");
			}
			catch (ArgumentException)
			{
				return new BooleanState(element.HowFound + " should be visible but is not because it does not exist.", null, () => false);
			}
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
			var unexpectedlyTrueMessage = string.Format(unexpectedlyTrue, element.HowFound);
			var unexpectedlyFalseMessage = string.Format(unexpectedlyFalse, element.HowFound, parent);

			var result = new BooleanState(unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() => visibility.Key);
			return result;
		}
	}
}
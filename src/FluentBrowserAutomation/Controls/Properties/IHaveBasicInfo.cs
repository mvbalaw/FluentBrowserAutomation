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
			var box = element as CheckBoxWrapper;
			if (box != null)
			{
				return box;
			}
			var elem = element.Element.IsCheckBox() ? element.Element : null;
			return new CheckBoxWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static DropDownListWrapper AsDropDownList(this IHaveBasicInfo element, string howFound = null)
		{
			var list = element as DropDownListWrapper;
			if (list != null)
			{
				return list;
			}
			var elem = element.Element.IsDropDownList() ? element.Element : null;
			return new DropDownListWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static FileUploadWrapper AsFileUpload(this IHaveBasicInfo element)
		{
			var upload = element as FileUploadWrapper;
			if (upload != null)
			{
				return upload;
			}
			var elem = element.Element.IsFileUpload() ? element.Element : null;
			return new FileUploadWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static RadioOptionWrapper AsRadioOption(this IHaveBasicInfo element, string howFound = null)
		{
			var option = element as RadioOptionWrapper;
			if (option != null)
			{
				return option;
			}
			var elem = element.Element.IsRadioOption() ? element.Element : null;
			return new RadioOptionWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static TextBoxWrapper AsTextBox(this IHaveBasicInfo element)
		{
			var box = element as TextBoxWrapper;
			if (box != null)
			{
				return box;
			}
			var elem = element.Element.IsTextBox() ? element.Element : null;
			return new TextBoxWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static void AssertIsCheckBox(this IHaveBasicInfo input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist in AssertIsCheckBox");
			if (!input.IsCheckBox())
			{
				throw new AssertionException(input.HowFound + " is not a checkbox.");
			}
		}

		public static void AssertIsDropDownList(this IHaveBasicInfo input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist in AssertIsDropDownList");
			if (!IsDropDownList(input))
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
		}

		public static void AssertIsRadioOption(this IHaveBasicInfo input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist in AssertIsRadioOption");
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
                        var elementWrapper = element.Element;
                        return elementWrapper.Exists;
                    }
					catch (Exception exception)
					{
                        if (exception.Message.Contains("result.webdriverValue.value list is missing or empty"))
                        {
                            throw new Exception("Your Browser driver is out of sync with your Browser version, update one or both.", exception);
                        }
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
				element.BrowserContext.WaitUntil(x => element.Exists().IsTrue, errorMessage:"wait for " + element.HowFound + " to exist in IsVisible");
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
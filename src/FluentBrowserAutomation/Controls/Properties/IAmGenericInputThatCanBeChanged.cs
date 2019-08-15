using System.Linq;

using FluentAssert;

using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

// ReSharper disable once CheckNamespace

namespace FluentBrowserAutomation
{
	public interface IAmGenericInputThatCanBeChanged : IAmInputThatCanBeChanged
	{
	}

	public static class IAmGenericInputThatCanBeChangedExtensions
	{
// ReSharper disable once SuggestBaseTypeForParameter
		public static IAmInputThatCanBeChanged CheckIt(this IAmGenericInputThatCanBeChanged input)
		{
			input.AssertIsCheckBox();
			var checkBox = input.AsCheckBox();
			checkBox.CheckedState().SetValueTo(true);
			return checkBox;
		}

		public static bool HasOption(this IAmGenericInputThatCanBeChanged input, string expected)
		{
			input.AssertIsDropDownList();
			var dropDown = input.AsDropDownList();
			return dropDown.HasOption(expected);
		}

		public static bool HasTextValue(this IAmGenericInputThatCanBeChanged input, string expected)
		{
			input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible in HasTextValue");
			if (input.IsTextBox())
			{
				var textBox = input.AsTextBox();
				return textBox.Text() == expected;
			}
			if (input.IsDropDownList())
			{
				var dropDown = input.AsDropDownList();
				return dropDown.HasOption(expected);
			}
			if (input.IsCheckBox())
			{
				var checkBox = input.AsCheckBox();
				return checkBox.Element.ValueAttributeHasValue(expected);
			}

			throw new AssertionException("Cannot get a text value from " + input.HowFound);
		}

		public static IAmInputThatCanBeChanged SelectAnyOptionExcept(this IAmGenericInputThatCanBeChanged input, params string[] unwantedValues)
		{
			input.AssertIsDropDownList();
			var dropDown = input.AsDropDownList();
			dropDown.SelectAnyOptionExcept(unwantedValues);
			return dropDown;
		}

		public static IAmInputThatCanBeChanged SelectIt(this IAmGenericInputThatCanBeChanged input)
		{
			input.AssertIsRadioOption();
			var radioOption = input.AsRadioOption();
			radioOption.SelectedState().SetValueTo(true);
			return radioOption;
		}

		public static IAmInputThatCanBeChanged SelectedOptionShouldBeEqualTo(this IAmGenericInputThatCanBeChanged input, string text)
		{
			input.AssertIsDropDownList();
			var dropDown = input.AsDropDownList();
			dropDown.SelectedOptionShouldBeEqualTo(text);
			return dropDown;
		}

		public static IAmInputThatCanBeChanged SelectedOptionValueShouldBeEqualTo(this IAmGenericInputThatCanBeChanged input, string value)
		{
			input.AssertIsDropDownList();
			input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible in SelectedOptionValueShouldBeEqualTo");
			var dropDown = input.AsDropDownList();
			input.BrowserContext.WaitUntil(x => dropDown.Options.Any(), errorMessage:"wait for " + input.HowFound + " options to be visible in SelectedOptionValueShouldBeEqualTo");
			var selectedValues = dropDown.GetSelectedValues().ToArray();
			selectedValues.Contains(value).ShouldBeTrue("Selected value of " + dropDown.HowFound + " should be '" + value + "' but is/are '" + string.Join(", ", selectedValues) + "'");
			return dropDown;
		}

		public static IAmInputThatCanBeChanged SetTo(this IAmGenericInputThatCanBeChanged input, string text)
		{
			input.WaitUntil(x => x.Exists().IsTrue, errorMessage:"wait for " + input.HowFound + " to exist in SetTo");

			if (input.IsTextBox())
			{
				var textBox = input.AsTextBox();
				textBox.SetTo(text);
				return textBox;
			}

			if (input.IsDropDownList())
			{
				var dropDown = input.AsDropDownList();
				dropDown.Select(text);
				return dropDown;
			}

			if (input.IsCheckBox())
			{
				var checkBox = input.AsCheckBox();
				if (text == "checked")
				{
					checkBox.CheckIt();
				}
				else
				{
					checkBox.UncheckIt();
				}
				return checkBox;
			}

			if (input.IsRadioOption())
			{
				var radioOption = input.AsRadioOption();
				if (text == "checked" || text == "selected")
				{
					radioOption.SelectIt();
				}
				else
				{
					radioOption.UnselectIt();
				}
				return radioOption;
			}

			if (input.IsFileUpload())
			{
				var fileUploadWrapper = input.AsFileUpload();
				fileUploadWrapper.SetTo(text);
				return fileUploadWrapper;
			}

			return new MissingInputWrapper(input.HowFound, input.BrowserContext);
		}

		public static IAmInputThatCanBeChanged ShouldBeChecked(this IAmGenericInputThatCanBeChanged input)
		{
			input.AssertIsCheckBox();
			var checkBox = input.AsCheckBox();
			checkBox.ShouldBeChecked();
			return checkBox;
		}

		public static IAmInputThatCanBeChanged ShouldBeEmpty(this IAmGenericInputThatCanBeChanged input)
		{
			input.HasTextValue("").ShouldBeTrue();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldBeEqualTo(this IAmGenericInputThatCanBeChanged input, string expected)
		{
			var textBox = input.AsTextBox();
			if (textBox == null)
			{
				throw new AssertionException(input.HowFound + " is not a text box. It is a(n) " + input.GetType());
			}
			input.BrowserContext.WaitUntil(x => textBox.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible in ShouldBeEqualTo");
			input.BrowserContext.WaitUntil(x =>
			{
				var actual = textBox.Text().Text ?? "";
				return actual == (expected??"");
			}, errorMessage:"wait for " + input.HowFound + " to have text '" + expected + "' in ShouldBeEqualTo");
			textBox.Text().ShouldBeEqualTo(expected ?? "");
			return textBox;
		}

		public static IAmInputThatCanBeChanged ShouldHaveOption(this IAmGenericInputThatCanBeChanged input, string optionText)
		{
			input.AssertIsDropDownList();
			input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible in ShouldHaveOption");
			var dropDown = input.AsDropDownList();
			input.BrowserContext.WaitUntil(x => dropDown.Options.Any(), errorMessage:"wait for " + input.HowFound + " options to be visible in ShouldHaveOption");
			dropDown.Options.Select(x => (string)x.Text).ToList().ShouldContainAll(new[] { optionText });
			return dropDown;
		}

		public static IAmInputThatCanBeChanged ShouldNotBeChecked(this IAmGenericInputThatCanBeChanged input)
		{
			input.AssertIsCheckBox();
			var checkBox = input.AsCheckBox();
			checkBox.ShouldNotBeChecked();
			return checkBox;
		}

		public static IAmInputThatCanBeChanged ShouldNotBeEmpty(this IAmGenericInputThatCanBeChanged input)
		{
			input.HasTextValue("").ShouldBeFalse();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldNotHaveOption(this IAmGenericInputThatCanBeChanged input, string optionText)
		{
			input.AssertIsDropDownList();
			var dropDown = input.AsDropDownList();
			dropDown.ShouldNotHaveOption(optionText);
			return dropDown;
		}

		public static IAmInputThatCanBeChanged UncheckIt(this IAmGenericInputThatCanBeChanged input)
		{
			input.AssertIsCheckBox();
			var checkBox = input.AsCheckBox();
			checkBox.CheckedState().SetValueTo(false);
			return checkBox;
		}
	}
}
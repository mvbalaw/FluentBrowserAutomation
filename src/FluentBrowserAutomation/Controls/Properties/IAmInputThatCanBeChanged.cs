using System;
using System.Linq;
using System.Threading;

using FluentAssert;

using FluentBrowserAutomation.Controls;

using OpenQA.Selenium.Interactions;

//// ReSharper disable CheckNamespace
namespace FluentBrowserAutomation
//// ReSharper restore CheckNamespace
{
	public interface IAmInputThatCanBeChanged : IAmVisualElement, IHaveBasicInfo
	{
	}

	public static class IAmInputThatCanBeChangedExtensions
	{
		public static IAmInputThatCanBeChanged Blur(this IAmInputThatCanBeChanged input)
		{
			input.Element.SendKeys("\t");
			return input;
		}

		public static IAmInputThatCanBeChanged CheckIt(this IAmInputThatCanBeChanged input)
		{
			var checkBox = input as CheckBoxWrapper;
			if (checkBox == null)
			{
				throw new AssertionException(input.HowFound + " is not a checkbox.");
			}
			checkBox.CheckedState().SetValueTo(true);
			return input;
		}

		public static IAmInputThatCanBeChanged Focus(this IAmInputThatCanBeChanged input)
		{
			input.Exists().ShouldBeTrue();
			input.IsVisible().ShouldBeTrue();
			new Actions(input.BrowserContext.Browser)
				.MoveToElement(input.Element)
				.Perform();
			return input;
		}

		public static bool HasOption(this IAmInputThatCanBeChanged input, string expected)
		{
			var dropDown = input as DropDownListWrapper;
			if (dropDown == null)
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
			input.Exists().ShouldBeTrue();
			input.IsVisible().ShouldBeTrue();
			return dropDown.Options.Any(x => x.Text == expected);
		}

		public static bool HasTextValue(this IAmInputThatCanBeChanged input, string expected)
		{
			var textField = input as TextBoxWrapper;
			if (textField != null)
			{
				input.Exists().ShouldBeTrue();
				input.IsVisible().ShouldBeTrue();
				return textField.Text() == expected;
			}
			var dropDown = input as DropDownListWrapper;
			if (dropDown != null)
			{
				input.Exists().ShouldBeTrue();
				input.IsVisible().ShouldBeTrue();
				return dropDown.Options.Any(x => x.Text == expected);
			}
			var checkBox = input as CheckBoxWrapper;
			if (checkBox != null)
			{
				input.Exists().ShouldBeTrue();
				input.IsVisible().ShouldBeTrue();
				return checkBox.Element.GetAttribute("value").Equals(expected);
			}

			throw new AssertionException("Cannot get a text value from " + input.HowFound);
		}

		public static IAmInputThatCanBeChanged SelectAnyOptionExcept(this IAmInputThatCanBeChanged input, params string[] unwantedValues)
		{
			var dropDown = input as DropDownListWrapper;
			if (dropDown == null)
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
			var id = dropDown.Id;
			input.WaitUntil(x => x.Exists().IsTrue);
			input.IsVisible().ShouldBeTrue();

			var dropDownListWrapper = input.BrowserContext.DropDownListWithId(id);
			var isDropDownList = dropDownListWrapper != null;
			if (!isDropDownList)
			{
				throw new AssertionException("Can only call SelectAnyOptionExcept() on drop down lists.");
			}

			var option = dropDownListWrapper.Options.First(x => !unwantedValues.Contains(x.Text));
			dropDownListWrapper.Select(option.Text);
			return input;
		}

		public static IAmInputThatCanBeChanged SelectedOptionShouldBeEqualTo(this IAmInputThatCanBeChanged input, string text)
		{
			var dropDown = input as DropDownListWrapper;
			if (dropDown == null)
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
			var selectedTexts = dropDown.GetSelectedTexts().ToArray();
			selectedTexts.Contains(text).ShouldBeTrue("Selected value of " + dropDown.HowFound + " should be '" + text + "' but is/are '" + String.Join(", ", selectedTexts) + "'");
			return input;
		}

		public static IAmInputThatCanBeChanged SetTo(this IAmInputThatCanBeChanged input, string text)
		{
			if (input is IAmTextInput)
			{
				(input as IAmTextInput).SetTo(text);
			}
			else if (input is IAmSelectionInput)
			{
				(input as IAmSelectionInput).Select(text);
			}
			else if (input is IAmToggleableInput)
			{
				(input as IAmToggleableInput).Toggle();
			}
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldBeChecked(this IAmInputThatCanBeChanged input)
		{
			var checkBoxWrapper = input as CheckBoxWrapper;
			if (checkBoxWrapper == null)
			{
				throw new AssertionException(input.HowFound + " is not a checkbox.");
			}
			checkBoxWrapper.CheckedState().ShouldBeTrue();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldBeEmpty(this IAmInputThatCanBeChanged input)
		{
			input.HasTextValue("").ShouldBeTrue();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldBeEqualTo(this IAmInputThatCanBeChanged input, string text)
		{
			var textBoxWrapper = input as TextBoxWrapper;
			if (textBoxWrapper == null)
			{
				throw new AssertionException(input.HowFound + " is not a text box.");
			}
			textBoxWrapper.Text().ShouldBeEqualTo(text ?? "");
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldBeVisible(this IAmInputThatCanBeChanged input)
		{
			input.IsVisible().ShouldBeTrue();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldHaveFocus(this IAmInputThatCanBeChanged input)
		{
			input.BrowserContext.Browser.SwitchTo().ActiveElement().ShouldBeEqualTo(input.Element);
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldHaveOption(this IAmInputThatCanBeChanged input, string optionText)
		{
			var dropDown = input as DropDownListWrapper;
			if (dropDown == null)
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
			dropDown.Options.Select(x => (string)x.Text).ToList().ShouldContainAll(new[] { optionText });
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldNotBeChecked(this IAmInputThatCanBeChanged input)
		{
			var checkBoxWrapper = input as CheckBoxWrapper;
			if (checkBoxWrapper == null)
			{
				throw new AssertionException(input.HowFound + " is not a checkbox.");
			}
			checkBoxWrapper.CheckedState().ShouldBeFalse();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldNotBeEmpty(this IAmInputThatCanBeChanged input)
		{
			input.HasTextValue("").ShouldBeFalse();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldNotBeVisible(this IAmInputThatCanBeChanged input)
		{
			input.IsVisible().ShouldBeFalse();
			return input;
		}

		public static IAmInputThatCanBeChanged ShouldNotHaveOption(this IAmInputThatCanBeChanged input, string optionText)
		{
			var dropDown = input as DropDownListWrapper;
			if (dropDown == null)
			{
				throw new AssertionException(input.HowFound + " is not a drop down list.");
			}
			dropDown.Options.Select(x => (string)x.Text).Any(x => x == optionText).ShouldBeFalse(dropDown.HowFound + " should not have option '" + optionText + "' but does.");
			return input;
		}

		public static IAmInputThatCanBeChanged UncheckIt(this IAmInputThatCanBeChanged input)
		{
			var checkBox = input as CheckBoxWrapper;
			if (checkBox == null)
			{
				throw new AssertionException(input.HowFound + " is not a checkbox.");
			}
			checkBox.CheckedState().SetValueTo(false);
			return input;
		}

		public static IAmInputThatCanBeChanged WaitUntil(this IAmInputThatCanBeChanged input, Func<IAmInputThatCanBeChanged, bool> func, int secondsToWait = 10)
		{
			for (var i = 0; i < secondsToWait; i++)
			{
				try
				{
					if (func(input))
					{
						return input;
					}
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.Message + " -- waiting 1 sec");
				}
				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
			throw new AssertionException("state being waited upon never happened.");
		}
	}
}
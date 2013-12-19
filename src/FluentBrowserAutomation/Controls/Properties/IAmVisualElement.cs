using System;
using System.Linq;

using FluentAssert;

using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Controls;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

// ReSharper disable once CheckNamespace
namespace FluentBrowserAutomation
{
	public interface IAmVisualElement : IHaveBasicInfo
	{
	}

	public static class IAmVisualElementExtensions
	{
		public static void Focus(this IAmVisualElement element)
		{
			var action = new Actions(element.BrowserContext.Browser);
			action.MoveToElement(element.Element).Perform();
		}

		public static BooleanState HasFocus(this IAmVisualElement input)
		{
			var elementWithFocus = input.BrowserContext.Browser.SwitchTo().ActiveElement();
			var state = ReferenceEquals(elementWithFocus, input.Element);
			return new BooleanState("Expected focus to be on " + input.HowFound + " but was not.", "Expected focus NOT to be on " + input.HowFound + " but was.", () => state);
		}

		public static void MoveMouseToIt(this IAmVisualElement element)
		{
			element.ScrollToIt();
			element.Focus();
		}

		public static void ScrollToIt(this IAmVisualElement element, int yOffset = 0)
		{
			var browser = element.BrowserContext.Browser;

			var elementPosition = ((RemoteWebElement)element.Element).LocationOnScreenOnceScrolledIntoView;
			var yPosition = elementPosition.Y + yOffset;
			if (yPosition < 0)
			{
				yPosition = 0;
			}
			var js = String.Format("window.scroll({0}, {1})", elementPosition.X, yPosition);
			((IJavaScriptExecutor)browser).ExecuteScript(js);
		}

		public static IAmVisualElement ShouldHaveFocus(this IAmVisualElement input)
		{
			input.BrowserContext.Browser.SwitchTo().ActiveElement().ShouldBeEqualTo(input.Element);
			return input;
		}

		public static ReadOnlyText Text(this IAmVisualElement input)
		{
			var textField = input as TextBoxWrapper;
			if (textField != null)
			{
				input.Exists().ShouldBeTrue();
				input.IsVisible().ShouldBeTrue();
				return textField.Text();
			}
			var dropDown = input as DropDownListWrapper;
			if (dropDown != null)
			{
				input.Exists().ShouldBeTrue();
				input.IsVisible().ShouldBeTrue();
				var selectedTexts = dropDown.GetSelectedTexts().ToArray();
				if (selectedTexts.Length != 1)
				{
					throw new ArgumentException("drop down list " + input.HowFound + " must have exactly 1 selected value to call .Text()");
				}
				return new ReadOnlyText(input.HowFound, selectedTexts[0]);
			}
			var checkBoxOrRadio = input as IAmToggleableInput;
			if (checkBoxOrRadio != null)
			{
				input.Exists().ShouldBeTrue();
				input.IsVisible().ShouldBeTrue();
				return new ReadOnlyText(input.HowFound, input.Element.GetAttribute("value"));
			}

			var text = input.Element.Text;
			return new ReadOnlyText(input.HowFound, text);
		}
	}
}
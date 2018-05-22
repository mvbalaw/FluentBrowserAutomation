using System;
using System.Linq;

using FluentAssert;

using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

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
		public static TextBoxWrapper AsTextBox(this IAmVisualElement element, string howFound = null)
		{
			if (element is TextBoxWrapper)
			{
				return element as TextBoxWrapper;
			}
			var elem = element.Element.IsTextBox() ? element.Element : null;
			return new TextBoxWrapper(elem, howFound ?? element.HowFound, element.BrowserContext);
		}

		public static void Focus(this IAmVisualElement element)
		{
			element.BrowserContext.WaitUntil(x => element.Exists().IsTrue, errorMessage:"move Focus to " + element.HowFound);

			var action = new Actions(element.BrowserContext.Browser);
			action.MoveToElement(element.Element.RemoteWebElement).Perform();
			ScrollToIt(element);
		}

		public static BooleanState HasFocus(this IAmVisualElement input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"verify " + input.HowFound + " has focus");
			var elementWithFocus = (RemoteWebElement)input.BrowserContext.Browser.SwitchTo().ActiveElement();
			var inputAsRemoteWebElement = (RemoteWebElement)input.Element.RemoteWebElement;
			var isSameElement = elementWithFocus.Size == inputAsRemoteWebElement.Size &&
				elementWithFocus.Location == inputAsRemoteWebElement.Location &&
				elementWithFocus.LocationOnScreenOnceScrolledIntoView == inputAsRemoteWebElement.LocationOnScreenOnceScrolledIntoView;
			return new BooleanState("Expected focus to be on " + input.HowFound + " but was not.", "Expected focus NOT to be on " + input.HowFound + " but was.", () => isSameElement);
		}

		public static void MoveMouseToIt(this IAmVisualElement element)
		{
			element.ScrollToIt();
			element.Focus();
		}

		public static void ScrollToIt(this IAmVisualElement element, int yOffset = 0)
		{
			var browser = element.BrowserContext.Browser;
			element.BrowserContext.WaitUntil(x => element.Exists().IsTrue, errorMessage:"scroll to " + element.HowFound);

			var webElement = element.Element.RemoteWebElement;
			ScrollToIt(yOffset, webElement, browser);
		}

		private static void ScrollToIt(int yOffset, IWebElement webElement, IWebDriver browser)
		{
			// from: https://stackoverflow.com/a/38321310/102536
			const string scrollElementIntoMiddle = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
			                                       + "var elementTop = arguments[0].getBoundingClientRect().top;"
			                                       + "window.scrollBy(0, elementTop-(viewPortHeight/2));";

			((IJavaScriptExecutor) browser).ExecuteScript(scrollElementIntoMiddle, webElement);
		}

		public static T ShouldBeVisible<T>(this T input, string errorMessage = null) where T : IAmVisualElement
		{
			input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:errorMessage ?? "wait for " + input.HowFound + " to be visible");
			return input;
		}

		public static IAmVisualElement ShouldHaveFocus(this IAmVisualElement input)
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"verify " + input.HowFound + " exists");
			try
			{
				input.BrowserContext.WaitUntil(x => input.HasFocus().IsTrue, errorMessage:"verify " + input.HowFound + " has focus");
			}
			catch (ArgumentException)
			{
			}
			var activeElement = input.BrowserContext.Browser.SwitchTo().ActiveElement();
			var activeElementId = activeElement.GetAttribute("id");
			if (activeElementId != null && activeElementId.Trim() != "")
			{
				activeElementId.ShouldBeEqualTo(input.Element.GetAttribute("id"), "Cursor is focussed on " + activeElement.TagName + " with id " + activeElementId);

				return input;
			}
			var activeElementName = activeElement.GetAttribute("name") ?? "";
			activeElementName.ShouldBeEqualTo(input.Element.GetAttribute("name"), "Cursor is focussed on " + activeElement.TagName + " with name " + activeElementName);
			return input;
		}

		public static T ShouldNotBeVisible<T>(this T input, string errorMessage = null) where T : IAmVisualElement
		{
			input.BrowserContext.WaitUntil(x => input.Exists().IsFalse || input.IsVisible().IsFalse, errorMessage: errorMessage ?? "wait for " + input.HowFound + " to NOT be visible");
			return input;
		}

		public static ReadOnlyText Text(this IAmVisualElement input)
		{
			if (input.IsTextBox())
			{
				var textField = AsTextBox(input);
				input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible");
				return textField.Text();
			}

			if (input.IsDropDownList())
			{
				var dropDown = input.AsDropDownList();
				input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible");
				var selectedTexts = dropDown.GetSelectedTexts().ToArray();
				if (selectedTexts.Length != 1)
				{
					throw new AssertionException("drop down list " + input.HowFound + " must have exactly 1 selected value to call .Text()");
				}
				return new ReadOnlyText(input.HowFound, selectedTexts[0]);
			}
			if (input.IsCheckBox() || input.IsRadioOption())
			{
				input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible");
				return new ReadOnlyText(input.HowFound, input.Element.GetAttribute("value"));
			}

			input.BrowserContext.WaitUntil(x => input.Exists().IsTrue, errorMessage:"get text from " + input.HowFound);
			var text = input.Element.Text;
			return new ReadOnlyText(input.HowFound, text);
		}
	}
}
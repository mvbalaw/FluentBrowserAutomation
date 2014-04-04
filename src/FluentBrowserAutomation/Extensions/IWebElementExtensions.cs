using System;
using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Controls;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Extensions
{
	public static class IWebElementExtensions
	{
		internal static IEnumerable<IWebElement> GetChildElementsByTagName(this IWebElement webElement, string tagName, Func<IWebElement, bool> isMatch = null)
		{
			IEnumerable<IWebElement> children = webElement.FindElements(By.TagName(tagName));
			if (isMatch != null)
			{
				children = children.Where(isMatch);
			}
			return children;
		}

		public static IWebElement GetParent(this IWebElement e)
		{
			// http://watirmelon.com/2012/07/25/getting-an-elements-parent-in-webdriver-in-c/
			IWebElement parent = null;
			try
			{
				parent = e.FindElement(By.XPath(".."));
			}
			catch (InvalidSelectorException)
			{
				// happens if the parent is the document
			}

			return parent;
		}

		internal static bool IsButton(this IWebElement webElement)
		{
			return webElement.TypeAttributeHasValue("submit", "button") ||
				webElement.TagNameHasValue("button");
		}

		public static bool IsCheckBox(this IWebElement element)
		{
			return element != null &&
				element.TagNameHasValue("input") &&
				element.TypeAttributeHasValue("checkbox");
		}

		public static bool IsDropDownList(this IWebElement element)
		{
			return element != null &&
				element.TagNameHasValue("select");
		}

		public static bool IsFileUpload(this IWebElement element)
		{
			return element != null &&
				element.TagNameHasValue("input") &&
				element.TypeAttributeHasValue("file");
		}

		public static bool IsRadioOption(this IWebElement element)
		{
			return element != null &&
				element.TagNameHasValue("input") &&
				element.TypeAttributeHasValue("radio");
		}

		public static bool IsTextBox(this IWebElement element)
		{
			return element != null &&
				(element.TagNameHasValue("input") && element.TypeAttributeHasValue("text") ||
					element.TagNameHasValue("textarea"));
		}

		internal static bool TagNameHasValue(this IWebElement webElement, params string[] tagNames)
		{
			var name = webElement.TagName;
			return tagNames.Any(x => x.Equals(name));
		}

		internal static bool TypeAttributeHasValue(this IWebElement webElement, params string[] types)
		{
			var attribute = webElement.GetAttribute("type");
			return types.Any(x => x.Equals(attribute));
		}

		internal static bool ValueAttributeHasValue(this IWebElement webElement, params string[] values)
		{
			var attribute = webElement.GetAttribute("value");
			return values.Any(x => x.Equals(attribute));
		}
	}
}
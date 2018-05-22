using System;
using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Extensions
{
	public static class RemoteWebElementWrapperExtensions
	{
		internal static IEnumerable<IWebElement> GetChildElementsByTagName(this RemoteWebElementWrapper webElement, string tagName, Func<IWebElement, bool> isMatch = null)
		{
			IEnumerable<IWebElement> children = webElement.FindElements(By.TagName(tagName));
			if (isMatch != null)
			{
				children = children.Where(isMatch);
			}
			return children;
		}

		public static RemoteWebElementWrapper GetParent(this RemoteWebElementWrapper e)
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

			return new RemoteWebElementWrapper(null, parent, e.Browser);
		}

		internal static bool IsButton(this RemoteWebElementWrapper webElement)
		{
			return webElement.TypeAttributeHasValue("submit", "button") ||
				webElement.TagNameHasValue("button");
		}

		public static bool IsCheckBox(this RemoteWebElementWrapper element)
		{
			return element.Exists &&
				element.TagNameHasValue("input") &&
				element.TypeAttributeHasValue("checkbox");
		}

		public static bool IsDropDownList(this RemoteWebElementWrapper element)
		{
			return element.Exists && element.TagNameHasValue("select");
		}

		public static bool IsFileUpload(this RemoteWebElementWrapper element)
		{
			return element.Exists &&
				element.TagNameHasValue("input") &&
				element.TypeAttributeHasValue("file");
		}

		internal static bool IsLink(this RemoteWebElementWrapper webElement)
		{
			return webElement.TagNameHasValue("a");
		}

		public static bool IsRadioOption(this RemoteWebElementWrapper element)
		{
			return element.Exists &&
				element.TagNameHasValue("input") &&
				element.TypeAttributeHasValue("radio");
		}

		public static bool IsTextBox(this RemoteWebElementWrapper element)
		{
			return element.Exists && (element.TagNameHasValue("input") && 
				(element.TypeAttributeHasValue("text") || element.TypeAttributeHasValue("number")) ||
				element.TagNameHasValue("textarea"));
		}

		internal static bool TagNameHasValue(this RemoteWebElementWrapper webElement, params string[] tagNames)
		{
			var name = webElement.TagName;
			return tagNames.Any(x => x.Equals(name));
		}

		internal static bool TypeAttributeHasValue(this RemoteWebElementWrapper webElement, params string[] types)
		{
			var attribute = webElement.GetAttribute("type");
			return types.Any(x => x.Equals(attribute));
		}

		internal static bool ValueAttributeHasValue(this RemoteWebElementWrapper webElement, params string[] values)
		{
			var attribute = webElement.GetAttribute("value");
			return values.Any(x => x.Equals(attribute));
		}
	}
}
using System;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Extensions
{
	public static class IWebElementExtensions
	{
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

		public static bool IsCheckBox(this IWebElement element)
		{
			return element != null &&
			       (String.Compare(element.TagName, "input", true) == 0 &&
			        String.Compare(element.GetAttribute("type"), "checkbox", true) == 0);
		}

		public static bool IsDropDownList(this IWebElement element)
		{
			return element != null && String.Compare(element.TagName, "select", true) == 0;
		}

		public static bool IsFileUpload(this IWebElement element)
		{
			return element != null &&
			       (String.Compare(element.TagName, "input", true) == 0 &&
			        String.Compare(element.GetAttribute("type"), "file", true) == 0);
		}

		public static bool IsRadioOption(this IWebElement element)
		{
			return element != null &&
			       (String.Compare(element.TagName, "input", true) == 0 &&
			        String.Compare(element.GetAttribute("type"), "radio", true) == 0);
		}

		public static bool IsTextBox(this IWebElement element)
		{
			return element != null &&
			       ((String.Compare(element.TagName, "input", true) == 0 &&
			         String.Compare(element.GetAttribute("type"), "text", true) == 0) ||
			        String.Compare(element.TagName, "textarea", true) == 0);
		}
	}
}
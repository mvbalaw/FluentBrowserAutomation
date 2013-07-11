using System;

using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

//// ReSharper disable CheckNamespace
// ReSharper disable CheckNamespace
namespace FluentBrowserAutomation
// ReSharper restore CheckNamespace
//// ReSharper restore CheckNamespace
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

		public static void MoveMouseToIt(this IAmVisualElement element)
		{
			element.ScrollToIt();
			element.Focus();
		}

		public static void ScrollToIt(this IAmVisualElement element)
		{
			var browser = element.BrowserContext.Browser;
			var windowSize = browser.Manage().Window.Size;

			var currentScreenLocation = ((ILocatable)browser.SwitchTo().ActiveElement()).LocationOnScreenOnceScrolledIntoView;

			var elementPosition = ((ILocatable)element.Element).LocationOnScreenOnceScrolledIntoView;
			var offset = windowSize.Height / 4;
			var yOffset = currentScreenLocation.Y < elementPosition.Y ? offset : -offset;
			var yPosition = elementPosition.Y + yOffset;
			if (yPosition < 0)
			{
				yPosition = 0;
			}
			else if (yPosition > windowSize.Height)
			{
				yPosition = windowSize.Height;
			}
			var js = String.Format("window.scroll({0}, {1})", elementPosition.X, yPosition);
			((IJavaScriptExecutor)browser).ExecuteScript(js);
		}

		public static ReadOnlyText Text(this IAmVisualElement element)
		{
			var text = element.Element.Text;
			return new ReadOnlyText(element.HowFound, text);
		}
	}
}
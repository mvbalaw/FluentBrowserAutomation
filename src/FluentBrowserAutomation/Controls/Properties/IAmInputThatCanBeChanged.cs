using System;
using System.Diagnostics;

using FluentBrowserAutomation.Controls;

using OpenQA.Selenium.Interactions;

//// ReSharper disable CheckNamespace
// ReSharper disable CheckNamespace

namespace FluentBrowserAutomation
// ReSharper restore CheckNamespace
//// ReSharper restore CheckNamespace
{
// ReSharper disable RedundantExtendsListEntry
	public interface IAmInputThatCanBeChanged : IAmVisualElement, IHaveBasicInfo, IInputControl
// ReSharper restore RedundantExtendsListEntry
	{
	}

	public static class IAmInputThatCanBeChangedExtensions
	{
		public static IAmInputThatCanBeChanged Blur(this IAmInputThatCanBeChanged input)
		{
			input.Element.SendKeys("\t");
			return input;
		}

		public static IAmInputThatCanBeChanged Focus(this IAmInputThatCanBeChanged input)
		{
			input.BrowserContext.WaitUntil(x => input.IsVisible().IsTrue, errorMessage:"wait for " + input.HowFound + " to be visible in Focus");

			new Actions(input.BrowserContext.Browser)
				.MoveToElement(input.Element.RemoteWebElement)
				.Click()
				.Perform();
			input.Element.SendKeys("");
			return input;
		}

		[DebuggerStepThrough]
		public static IAmInputThatCanBeChanged WaitUntil(this IAmInputThatCanBeChanged input, Func<IAmInputThatCanBeChanged, bool> func, int secondsToWait = 10, string errorMessage = null)
		{
			input.BrowserContext.WaitUntil(x => func(input), secondsToWait, errorMessage);
			return input;
		}
	}
}
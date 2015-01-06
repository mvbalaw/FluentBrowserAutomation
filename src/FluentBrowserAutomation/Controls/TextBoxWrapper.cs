using FluentBrowserAutomation.Accessors;

using JetBrains.Annotations;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TextBoxWrapper : BasicInfoWrapper, IAmInputThatCanBeChanged, IAmTextInput, INeedFocus
	{
		public TextBoxWrapper(RemoteWebElementWrapper textField, string howFound, IBrowserContext browserContext)
			: base(textField, howFound, browserContext)
		{
		}

		public bool HasTextValue(string expected)
		{
			return Text() == expected;
		}

		public void KeyboardCopy()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			Element.SendKeys(Keys.Control + "c");
		}

		public void KeyboardPaste()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			Element.SendKeys(Keys.Control + "v");
		}

		public TextBoxWrapper KeyboardSelectAll()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			Element.SendKeys(Keys.Control + "a");
			return this;
		}

		public dynamic SetTo(string text)
		{
			this.ScrollToIt();
			Element.Clear();
			Element.SendKeys(text);
			return this;
		}

		public void ShouldBeEqualTo([NotNull] string text)
		{
			Text().ShouldBeEqualTo(text);
		}

		public void ShouldHaveTextValue(string expected, string errorMessage = null)
		{
			BrowserContext.WaitUntil(x => HasTextValue(expected), errorMessage:errorMessage ?? ("wait for " + HowFound + " to have text value '" + expected + "'"));
		}

		public void ShouldNotHaveTextValue(string expected, string errorMessage = null)
		{
			BrowserContext.WaitUntil(x => !HasTextValue(expected), errorMessage:errorMessage ?? ("wait for " + HowFound + " to NOT have text value '" + expected + "'"));
		}

		// unsupported in ChromeDriver1, maybe work in chromedriver2
//		public void MousePaste(string text)
//		{
//			this.ScrollToIt();
//			Element.Clear();
//			Clipboard.SetText(text, TextDataFormat.Text);
//			new Actions(BrowserContext.Browser)
//				.ContextClick(Element)
//				.SendKeys(Keys.ArrowDown)
//				.SendKeys(Keys.ArrowDown)
//				.SendKeys(Keys.ArrowDown)
//				.SendKeys(Keys.ArrowDown)
//				.SendKeys(Keys.ArrowDown)
//				.SendKeys(Keys.Return)
//				.Build()
//				.Perform();
////			new Actions(BrowserContext.Browser)
////				.ContextClick(Element)
////				.MoveByOffset(10,10)
////				.SendKeys(Keys.Return)
////				.Build()
////				.Perform();
//		}

		public ReadOnlyText Text()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			return new ReadOnlyText(HowFound, Element.GetAttribute("value"));
		}

		public static implicit operator ReadOnlyText(TextBoxWrapper textBox)
		{
			return textBox.Text();
		}
	}
}
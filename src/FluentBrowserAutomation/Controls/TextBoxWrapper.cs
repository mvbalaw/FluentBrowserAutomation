using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TextBoxWrapper : BasicInfoWrapper, IAmInputThatCanBeChanged, IAmTextInput, INeedFocus
	{
		public TextBoxWrapper(IWebElement textField, string howFound, IBrowserContext browserContext)
			: base(textField, howFound, browserContext)
		{
		}

		public void SetTo(string text)
		{
			this.ScrollToIt();
			Element.Clear();
			Element.SendKeys(text);
		}

		public void KeyboardCopy()
		{
			Element.SendKeys(Keys.Control + "c");
		}

		public void KeyboardPaste()
		{
			Element.SendKeys(Keys.Control + "v");
		}

		public TextBoxWrapper KeyboardSelectAll()
		{
			Element.SendKeys(Keys.Control + "a");
			return this;
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
			this.Exists().ShouldBeTrue();
			return new ReadOnlyText(HowFound, Element.GetAttribute("value"));
		}
	}
}
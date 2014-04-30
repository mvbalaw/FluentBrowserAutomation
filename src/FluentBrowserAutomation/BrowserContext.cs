using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

using FluentAssert;

using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Declarative;
using FluentBrowserAutomation.Extensions;
using FluentBrowserAutomation.Framework;

using JetBrains.Annotations;

using OpenQA.Selenium;

namespace FluentBrowserAutomation
{
	public interface IBrowserContext
	{
		string BaseUrl { get; }
		IWebDriver Browser { get; }
		ButtonWrapper ButtonWithId([NotNull] string id);
		ButtonWrapper ButtonWithText([NotNull] string text);
		IEnumerable<ButtonWrapper> Buttons();
		IEnumerable<ButtonWrapper> ButtonsWithClassName([NotNull] string className);
		CheckBoxWrapper CheckBoxWithId([NotNull] string id);
		CheckBoxWrapper CheckBoxWithIdAndValue([NotNull] string id, [NotNull] string value);
		CheckBoxWrapper CheckBoxWithLabel([NotNull] string id);
		CheckBoxWrapper CheckBoxWithNameAndValue([NotNull] string name, [NotNull] string value);
		IEnumerable<CheckBoxWrapper> CheckBoxes();
		IEnumerable<CheckBoxWrapper> CheckBoxesWithName([NotNull] string name);
		void CloseBrowser();
		ContainerWrapper ContainerWithId([NotNull] string id);
		DialogHandlerWrapper Dialog([NotNull] Action action);
		DivWrapper DivWithId([NotNull] string id);
		IEnumerable<DivWrapper> Divs();
		IEnumerable<DivWrapper> DivsWithClassName([NotNull] string className);
		DropDownListWrapper DropDownListWithId([NotNull] string idOfList);
		DropDownListWrapper DropDownListWithLabel([NotNull] string label);
		IEnumerable<DropDownListWrapper> DropDownLists();
		string GetHiddenValueWithId([NotNull] string id);
		IEnumerable<IWebElement> GetWebElementsWithClassName([NotNull] string className);
		void GoToUrl([NotNull] string url);
		TextBoxWrapper HiddenWithId([NotNull] string id);
		IEnumerable<TextBoxWrapper> Hiddens();
		BrowserContext IdOfFieldWithFocusShouldBe([NotNull] string expectedId);
		IEnumerable<ButtonWrapper> ImageButtons();
		ImageWrapper ImageWithId([NotNull] string id);
		IAmInputThatCanBeChanged InputWithClassName([NotNull] string className);
		IAmInputThatCanBeChanged InputWithId([NotNull] string id);
		IAmInputThatCanBeChanged InputWithLabel([NotNull] string label);
		IAmInputThatCanBeChanged InputWithValue([NotNull] string value);
		LabelWrapper LabelWithId([NotNull] string id);
		IEnumerable<LabelWrapper> Labels();
		LinkWrapper LinkWithId([NotNull] string id);
		LinkWrapper LinkWithText([NotNull] string text);
		IEnumerable<LinkWrapper> Links();
		IEnumerable<LinkWrapper> LinksWithClassName(string className);
		IEnumerable<LinkWrapper> LinksWithText(string text);
		ListWrapper ListWithId([NotNull] string id);
		INavigationControl NavigationControlWithId([NotNull] string id);
		INavigationControl NavigationControlWithText([NotNull] string text);
		IEnumerable<INavigationControl> NavigationControlsWithClassName([NotNull] string className);
		PageWrapper Page();
		RadioOptionWrapper RadioOptionWithId([NotNull] string idOfOption);
		RadioOptionWrapper RadioOptionWithLabel([NotNull] string label);
		RadioOptionWrapper RadioOptionWithNameAndValue([NotNull] string name, [NotNull] string value);
		IEnumerable<RadioOptionWrapper> RadioOptions();
		IEnumerable<RadioOptionWrapper> RadioOptionsWithName([NotNull] string name);
		IAmInputThatCanBeChanged Set([NotNull] string labelText);
		SpanWrapper SpanWithId([NotNull] string id);
		SpanWrapper SpanWithText([NotNull] string spanText);
		IEnumerable<SpanWrapper> Spans();
		IEnumerable<SpanWrapper> SpansWithClassName([NotNull] string className);
		TableWrapper TableWithId([NotNull] string id);
		IEnumerable<TableWrapper> Tables();
		TextBoxWrapper TextBoxWithFocus();
		TextBoxWrapper TextBoxWithId([NotNull] string id);
		TextBoxWrapper TextBoxWithLabel([NotNull] string label);
		IEnumerable<TextBoxWrapper> TextBoxes();
		IBrowserContext WaitUntil([NotNull] Func<IBrowserContext, bool> func, int secondsToWait = 10, string errorMessage = null);
	    IBrowserContext TryAndWaitUntil([NotNull] Func<IBrowserContext, bool> func,
	        [NotNull] Func<IBrowserContext, bool> waitOn, int secondsToWait = 5, int totalNumberOfRetries = 1,
	        string errorMessage = null);
	}

	public class BrowserContext : IBrowserContext
	{
		public BrowserContext(IBrowserManager browserManager)
		{
			_browserManager = browserManager;
		}

		private IWebDriver _browser;
		private readonly IBrowserManager _browserManager;

		public ButtonWrapper ButtonWithId(string id)
		{
			var howFound = String.Format("button with id '{0}'", id);
			var button = this.TryGetElementById(id, IWebElementExtensions.IsButton);
			return new ButtonWrapper(button, howFound, this);
		}

		public ButtonWrapper ButtonWithText(string text)
		{
			var howFound = String.Format("button with visible text '{0}'", text);
			var button = Buttons().FirstOrDefault(x => x.Text == text);
			if (button != null)
			{
				button.HowFound = howFound;
			}
			else
			{
				button = new ButtonWrapper(null, howFound, this);
			}
			return button;
		}

		public IEnumerable<ButtonWrapper> Buttons()
		{
			const string howFound = "type 'button'";
			var inputWrappers = this.GetElements(By.XPath("//input[@type='submit' or @type='button']|//button"))
				.Select(input => new ButtonWrapper(input, howFound, this));
			return inputWrappers;
		}

		public IEnumerable<ButtonWrapper> ButtonsWithClassName(string className)
		{
			var howFound = String.Format("button with class '{0}'", className);
			return this.GetElementsByClassName(className, IWebElementExtensions.IsButton)
				.Select(x => new ButtonWrapper(x, howFound, this));
		}

		public CheckBoxWrapper CheckBoxWithId(string id)
		{
			var howFound = String.Format("checkbox with id '{0}'", id);
			var checkBox = this.TryGetElementByIdAndType(id, "checkbox");
			return new CheckBoxWrapper(checkBox, howFound, this);
		}

		public CheckBoxWrapper CheckBoxWithIdAndValue(string id, string value)
		{
			var howFound = String.Format("checkbox with id '{0}'", id);
			var checkBox = this.TryGetElementByIdAndTypeAndValue(id, "checkbox", value);
			return new CheckBoxWrapper(checkBox, howFound, this);
		}

		public CheckBoxWrapper CheckBoxWithLabel(string label)
		{
			var input = InputWithLabel(label);
			var checkBoxWrapper = input as CheckBoxWrapper;
			var howFound = String.Format("checkbox with label '{0}'", label);
			if (checkBoxWrapper != null)
			{
				checkBoxWrapper.HowFound = howFound;
				return checkBoxWrapper;
			}
			return new CheckBoxWrapper(null, howFound, input.BrowserContext);
		}

		public CheckBoxWrapper CheckBoxWithNameAndValue(string name, string value)
		{
			var howFound = String.Format("checkbox with name '{0}'", name);
			var checkBox = this.TryGetElementByNameAndTypeAndValue(name, "checkbox", value);
			return new CheckBoxWrapper(checkBox, howFound, this);
		}

		public IEnumerable<CheckBoxWrapper> CheckBoxes()
		{
			const string howFound = "type 'checkbox'";
			var checkBoxes = this.GetInputsByType("checkbox");
			return checkBoxes.Select(x => new CheckBoxWrapper(x, howFound, this));
		}

		public IEnumerable<CheckBoxWrapper> CheckBoxesWithName(string name)
		{
			const string howFound = "type 'checkbox'";
			var checkBoxes = this.GetInputsByTypeAndName("checkbox", name);
			return checkBoxes.Select(x => new CheckBoxWrapper(x, howFound, this));
		}

		public void CloseBrowser()
		{
			_browserManager.Close();
		}

		public ContainerWrapper ContainerWithId(string id)
		{
			var howFound = String.Format("container with id '{0}'", id);
			var container = this.TryGetElementByIdAndTagName(id, "div", "span", "ul", "ol", "table");
			return new ContainerWrapper(container, howFound, this);
		}

		public DialogHandlerWrapper Dialog(Action action)
		{
			return new DialogHandlerWrapper(this, action);
		}

		public DivWrapper DivWithId(string id)
		{
			var howFound = String.Format("div with id '{0}'", id);
			var div = this.TryGetElementByIdAndTagName(id, "div");
			return new DivWrapper(div, howFound, this);
		}

		public IEnumerable<DivWrapper> Divs()
		{
			const string howFound = "type 'div'";
			var divs = this.GetElementsByTagName("div");
			return divs.Select(x => new DivWrapper(x, howFound, this));
		}

		public IEnumerable<DivWrapper> DivsWithClassName(string className)
		{
			var howFound = String.Format("div with class '{0}'", className);
			return this.GetElementsByClassNameAndTagName(className, "div")
				.Select(x => new DivWrapper(x, howFound, this));
		}

		public DropDownListWrapper DropDownListWithId(string idOfList)
		{
			var howFound = String.Format("drop down list with id '{0}'", idOfList);
			var dropDownList = this.TryGetElementByIdAndTagName(idOfList, "select");
			return new DropDownListWrapper(dropDownList, howFound, this);
		}

		public DropDownListWrapper DropDownListWithLabel(string label)
		{
			var input = InputWithLabel(label);
			var downListWithLabel = input as DropDownListWrapper;
			if (downListWithLabel != null)
			{
				return downListWithLabel;
			}
			var howFound = String.Format("drop down list with label '{0}'", label);
			return new DropDownListWrapper(null, howFound, input.BrowserContext);
		}

		public IEnumerable<DropDownListWrapper> DropDownLists()
		{
			const string howFound = "type 'select'";
			var dropDowns = this.GetElementsByTagName("select");
			return dropDowns.Select(x => new DropDownListWrapper(x, howFound, this));
		}

		public string GetHiddenValueWithId(string id)
		{
			var hidden = this.TryGetElementByIdAndType(id, "hidden");
			return hidden.GetAttribute("value");
		}

		public IEnumerable<IWebElement> GetWebElementsWithClassName(string className)
		{
			return this.GetElementsByClassName(className);
		}

		public void GoToUrl(string url)
		{
			Browser.Navigate().GoToUrl(url);
		}

		public TextBoxWrapper HiddenWithId(string id)
		{
			var howFound = String.Format("hidden input with id '{0}'", id);
			var hidden = this.TryGetElementByIdAndType(id, "hidden");
			return new TextBoxWrapper(hidden, howFound, this);
		}

		public IEnumerable<TextBoxWrapper> Hiddens()
		{
			const string howFound = "type 'hidden'";
			var hiddens = this.GetInputsByType("hidden");
			return hiddens.Select(x => new TextBoxWrapper(x, howFound, this));
		}

		public BrowserContext IdOfFieldWithFocusShouldBe(string expectedId)
		{
			var id = Browser.SwitchTo().ActiveElement().GetAttribute("id");
			id.ShouldBeEqualTo(expectedId);
			return this;
		}

		public IEnumerable<ButtonWrapper> ImageButtons()
		{
			const string howFound = "type 'img'";
			var images = this.GetElementsByTagName("img");
			return images.Select(x => new ButtonWrapper(x, howFound, this));
		}

		public ImageWrapper ImageWithId(string id)
		{
			var howFound = String.Format("image with id '{0}'", id);
			var button = this.TryGetElementByIdAndTagName(id, "img");
			return new ImageWrapper(button, howFound, this);
		}

		public IAmInputThatCanBeChanged InputWithClassName(string className)
		{
			_browserManager.Trace("Getting input with class name '" + className + "'");
			return SetterFor.InputWithClassName(this, className);
		}

		public IAmInputThatCanBeChanged InputWithId(string id)
		{
			_browserManager.Trace("Getting input with id '" + id + "'");
			return SetterFor.InputWithId(this, id);
		}

		public IAmInputThatCanBeChanged InputWithLabel(string label)
		{
			_browserManager.Trace("Getting input with label '" + label + "'");
			return SetterFor.InputWithLabel(this, label);
		}

		public IAmInputThatCanBeChanged InputWithValue(string value)
		{
			_browserManager.Trace("Getting input with value '" + value + "'");
			return SetterFor.InputWithValue(this, value);
		}

		public LabelWrapper LabelWithId(string id)
		{
			var howFound = String.Format("label with id '{0}'", id);
			var label = this.TryGetElementByIdAndTagName(id, "label");
			return new LabelWrapper(label, howFound, this);
		}

		public IEnumerable<LabelWrapper> Labels()
		{
			const string howFound = "type 'label'";
			var labels = this.GetElementsByTagName("label");
			return labels.Select(x => new LabelWrapper(x, howFound, this));
		}

		public LinkWrapper LinkWithId(string id)
		{
			var howFound = String.Format("link with id '{0}'", id);
			var link = this.TryGetElementByIdAndTagName(id, "a");
			return new LinkWrapper(link, howFound, this);
		}

		public LinkWrapper LinkWithText(string text)
		{
			var howFound = String.Format("link with visible text '{0}'", text);
			var link = LinksWithText(text).FirstOrDefault() ??
				new LinkWrapper(null, howFound, this);
			return link;
		}

		public IEnumerable<LinkWrapper> Links()
		{
			const string howFound = "type 'a'";
			var links = this.GetElementsByTagName("a");
			return links.Select(x => new LinkWrapper(x, howFound, this));
		}

		public IEnumerable<LinkWrapper> LinksWithClassName(string className)
		{
			var howFound = String.Format("link with class '{0}'", className);
			return this.GetElementsByClassNameAndTagName(className, "a")
				.Select(x => new LinkWrapper(x, howFound, this));
		}

		public IEnumerable<LinkWrapper> LinksWithText(string text)
		{
			var htmlEscapedText = HttpUtility.HtmlEncode(text);
			var howFound = String.Format("link with visible text '{0}'", text);
			var items = this.GetElements(By.LinkText(text))
				.Concat(this.GetElementsByTagName("a").Where(x =>
				{
					var attribute = x.GetAttribute("innerHTML");
					return attribute == htmlEscapedText ||
						attribute == text ||
						attribute.Trim() == text;
				}))
				.Select(link => new LinkWrapper(link, howFound, this));
			return items;
		}

		public ListWrapper ListWithId(string id)
		{
			var howFound = String.Format("list with id '{0}'", id);
			var list = this.TryGetElementByIdAndTagName(id, "ul");
			return new ListWrapper(list, howFound, this);
		}

		public INavigationControl NavigationControlWithId(string id)
		{
			var elementWithId = this.TryGetElementById(id);
			if (elementWithId != null)
			{
				if (elementWithId.TagNameHasValue("a"))
				{
					return new LinkWrapper(elementWithId, "link with id '" + id + "'", this);
				}
				if (elementWithId.IsButton())
				{
					return new ButtonWrapper(elementWithId, "button with id '" + id + "'", this);
				}
			}
			return new ButtonWrapper(null, "navigation control with id '" + id + "'", this);
		}

		public INavigationControl NavigationControlWithText(string text)
		{
			var button = ButtonWithText(text);
			if (button.Exists().IsTrue)
			{
				return button;
			}
			return LinkWithText(text);
		}

		public IEnumerable<INavigationControl> NavigationControlsWithClassName(string className)
		{
			var howFound = String.Format("link with class '{0}'", className);
			var buttonsAndLinks = this.GetElementsByClassName(className)
				.Select(x => x.TagNameHasValue("a")
					? new LinkWrapper(x, howFound, this)
					: x.IsButton()
						? (INavigationControl)new ButtonWrapper(x, howFound, this)
						: null)
				.Where(x => x != null)
				.ToArray();
			return buttonsAndLinks;
		}

		public PageWrapper Page()
		{
			return new PageWrapper(this);
		}

		public RadioOptionWrapper RadioOptionWithId(string idOfOption)
		{
			var howFound = String.Format("radio option with id '{0}'", idOfOption);
			var radioButton = this.TryGetElementByIdAndType(idOfOption, "radio");
			return new RadioOptionWrapper(radioButton, howFound, this);
		}

		public RadioOptionWrapper RadioOptionWithLabel(string label)
		{
			var input = InputWithLabel(label);
			var radioOptionWrapper = input as RadioOptionWrapper;
			if (radioOptionWrapper != null)
			{
				return radioOptionWrapper;
			}
			return new RadioOptionWrapper(null, "radio option with label '" + label + "'", input.BrowserContext);
		}

		public RadioOptionWrapper RadioOptionWithNameAndValue(string name, string value)
		{
			var howFound = String.Format("radio option with name '{0}'", name);
			var checkBox = this.TryGetElementByNameAndTypeAndValue(name, "radio", value);
			return new RadioOptionWrapper(checkBox, howFound, this);
		}

		public IEnumerable<RadioOptionWrapper> RadioOptions()
		{
			const string howFound = "type 'radio'";
			var radios = this.GetInputsByType("radio");
			return radios.Select(x => new RadioOptionWrapper(x, howFound, this));
		}

		public IEnumerable<RadioOptionWrapper> RadioOptionsWithName(string name)
		{
			var howFound = String.Format("type 'radio' with name '{0}'", name);
			var checkBoxes = this.GetInputsByTypeAndName("radio", name);
			return checkBoxes.Select(x => new RadioOptionWrapper(x, howFound, this));
		}

		public IAmInputThatCanBeChanged Set(string labelText)
		{
			return InputWithLabel(labelText);
		}

		public SpanWrapper SpanWithId(string id)
		{
			var howFound = String.Format("span with id '{0}'", id);
			var span = this.TryGetElementByIdAndTagName(id, "span");
			return new SpanWrapper(span, howFound, this);
		}

		public SpanWrapper SpanWithText(string spanText)
		{
			return Spans().First(x => x.Text() == spanText);
		}

		public IEnumerable<SpanWrapper> Spans()
		{
			const string howFound = "type 'span'";
			var spans = this.GetElementsByTagName("span");
			return spans.Select(x => new SpanWrapper(x, howFound, this));
		}

		public IEnumerable<SpanWrapper> SpansWithClassName(string className)
		{
			var howFound = String.Format("span with class '{0}'", className);
			return this.GetElementsByClassNameAndTagName(className, "span")
				.Select(x => new SpanWrapper(x, howFound, this));
		}

		public TableWrapper TableWithId(string id)
		{
			var howFound = String.Format("table with id '{0}'", id);
			var table = this.TryGetElementByIdAndTagName(id, "table");
			return new TableWrapper(table, howFound, this);
		}

		public IEnumerable<TableWrapper> Tables()
		{
			const string howFound = "type 'table'";
			var tables = this.GetElementsByTagName("table");
			return tables.Select(x => new TableWrapper(x, howFound, this));
		}

		public TextBoxWrapper TextBoxWithFocus()
		{
			var active = Browser.SwitchTo().ActiveElement();
			return new TextBoxWrapper(active, "textbox with focus", this);
		}

		public TextBoxWrapper TextBoxWithId(string id)
		{
			var howFound = String.Format("text box with id '{0}'", id);
			var textField = this.TryGetElement(By.XPath("//textarea[@id='" + id + "']|//input[@id='" + id + "' and @type='text']"));
			return new TextBoxWrapper(textField, howFound, this);
		}

		public TextBoxWrapper TextBoxWithLabel(string label)
		{
			var input = InputWithLabel(label);
			var textBoxWrapper = input as TextBoxWrapper;
			if (textBoxWrapper != null)
			{
				return textBoxWrapper;
			}
			return new TextBoxWrapper(null, "Textbox with label '" + label + "'", input.BrowserContext);
		}

		public IEnumerable<TextBoxWrapper> TextBoxes()
		{
			var textBoxes = this.GetElements(By.XPath("//textarea|//input[@type='text']"))
				.Select(x => new TextBoxWrapper(x, "text box or textarea", this));
			return textBoxes;
		}

		public IBrowserContext WaitUntil(Func<IBrowserContext, bool> func, int secondsToWait = 10, string errorMessage = null)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			Exception caughtException = null;
			do
			{
				try
				{
					if (func(this))
					{
						return this;
					}
				}
// ReSharper disable once EmptyGeneralCatchClause
				catch (Exception exception)
				{
					caughtException = exception;
				}
				Thread.Sleep(TimeSpan.FromSeconds(.25));
			} while (stopwatch.Elapsed.TotalSeconds < secondsToWait);
			if (caughtException != null)
			{
				if (errorMessage != null)
				{
					throw new ArgumentException("WaitUntil '" + errorMessage + "' caught: " + caughtException.Message, caughtException);
				}
				throw new ArgumentException(caughtException.Message, caughtException);
			}
			throw new AssertionException(errorMessage ?? "state being waited upon never happened.");
		}

        public IBrowserContext TryAndWaitUntil(Func<IBrowserContext, bool> func, Func<IBrowserContext, bool> waitOn, int secondsToWait = 5, int totalNumberOfRetries = 1, string errorMessage = null)
		{
		    int numberOfTries = 0;
            Exception caughtException = null;

            while (numberOfTries < totalNumberOfRetries)
            {
                func(this);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                do
                {
                    try
                    {
                        if (waitOn(this))
                        {
                            return this;
                        }
                    }
                        // ReSharper disable once EmptyGeneralCatchClause
                    catch (Exception exception)
                    {
                        caughtException = exception;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(.25));
                } while (stopwatch.Elapsed.TotalSeconds < secondsToWait);
                
                numberOfTries++;
            }

            if (caughtException != null)
            {
                if (errorMessage != null)
                {
                    throw new ArgumentException(
                        "WaitUntil '" + errorMessage + "' caught: " + caughtException.Message, caughtException);
                }
                throw new ArgumentException(caughtException.Message, caughtException);
            }
            throw new AssertionException(errorMessage ?? "state being waited upon never happened.");
		}

		public string BaseUrl
		{
			get { return _browserManager.BaseUrl; }
		}

		public IWebDriver Browser
		{
			get { return _browser ?? (_browser = _browserManager.GetBrowser()); }
		}
	}

	internal static class IBrowserContextExtensions
	{
		internal static IEnumerable<IWebElement> GetElements(this IBrowserContext browserContext, By @by, Func<IWebElement, bool> isMatch = null)
		{
			var result = browserContext.Browser.FindElements(@by).AsParallel();
			if (isMatch != null)
			{
				result = result.Where(isMatch);
			}
			return result;
		}

		internal static IEnumerable<IWebElement> GetElementsByClassName(this IBrowserContext browserContext, string className, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.GetElements(By.ClassName(className), isMatch);
		}

		internal static IEnumerable<IWebElement> GetElementsByClassNameAndTagName(this IBrowserContext browserContext, string className, string tagName)
		{
			var xPath = By.XPath("//" + tagName + "[contains(@class, '" + className + "')]");
			return browserContext.GetElements(xPath);
		}

		internal static IEnumerable<IWebElement> GetElementsByTagName(this IBrowserContext browserContext, string tag, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.GetElements(By.TagName(tag), isMatch);
		}

		internal static IEnumerable<IWebElement> GetInputs(this IBrowserContext browserContext, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.GetElementsByTagName("input", isMatch);
		}

		internal static IEnumerable<IWebElement> GetInputsByType(this IBrowserContext browserContext, params string[] type)
		{
			return browserContext.GetElements(By.XPath("//input[@type='" + type + "']"));
		}

		internal static IEnumerable<IWebElement> GetInputsByTypeAndName(this IBrowserContext browserContext, string type, string name, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.GetElements(By.XPath("//input[@type='" + type + "' and @name='" + name + "']"), isMatch);
		}

		internal static IWebElement TryGetElement(this IBrowserContext browserContext, By by, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.GetElements(@by, isMatch).FirstOrDefault();
		}

		internal static IWebElement TryGetElementById(this IBrowserContext browserContext, string id, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.TryGetElement(By.Id(id), isMatch);
		}

		internal static IWebElement TryGetElementByIdAndTagName(this IBrowserContext browserContext, string id, params string[] tagNames)
		{
			var query = String.Join("|", tagNames.Select(tagName => "//" + tagName + "[@id='" + id + "']"));
			return browserContext.TryGetElement(By.XPath(query));
		}

		internal static IWebElement TryGetElementByIdAndType(this IBrowserContext browserContext, string id, params string[] types)
		{
			var typeQuery = String.Join(" or ", types.Select(x => "@type = '" + x + "'"));
			return browserContext.TryGetElement(By.XPath("//*[@id='" + id + "' and (" + typeQuery + ")]"));
		}

		internal static IWebElement TryGetElementByIdAndTypeAndValue(this IBrowserContext browserContext, string id, string type, string value)
		{
			return browserContext.TryGetElement(By.XPath("//*[@id='" + id + "' and @type='" + type + "' and @value='" + value.Replace("'", "&apos;") + "']"));
		}

		internal static IWebElement TryGetElementByNameAndTypeAndValue(this IBrowserContext browserContext, string name, string type, string value)
		{
			return browserContext.TryGetElement(By.XPath("//*[@name='" + name + "' and @type='" + type + "' and @value='" + value.Replace("'", "&apos;") + "']"));
		}

		internal static IWebElement TryGetElementByName(this IBrowserContext browserContext, string name, Func<IWebElement, bool> isMatch = null)
		{
			return browserContext.TryGetElement(By.Name(name), isMatch);
		}
	}
}
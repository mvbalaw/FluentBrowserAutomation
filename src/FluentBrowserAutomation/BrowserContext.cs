using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

using FluentAssert;

using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Declarative;
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
		CheckBoxWrapper CheckBoxWithId([NotNull] string id);
		CheckBoxWrapper CheckBoxWithLabel([NotNull] string id);
		IEnumerable<CheckBoxWrapper> CheckBoxes();
		void CloseBrowser();
		ContainerWrapper ContainerWithId([NotNull] string id);
		DialogHandlerWrapper Dialog([NotNull] Action action);
		DivWrapper DivWithId([NotNull] string id);
		IEnumerable<DivWrapper> Divs();
		DropDownListWrapper DropDownListWithId([NotNull] string idOfList);
		DropDownListWrapper DropDownListWithLabel([NotNull] string label);
		IEnumerable<DropDownListWrapper> DropDownLists();
		void GoToUrl([NotNull] string url);
		TextBoxWrapper HiddenWithId([NotNull] string id);
		IEnumerable<TextBoxWrapper> Hiddens();
		BrowserContext IdOfFieldWithFocusShouldBe([NotNull] string expectedId);
		IEnumerable<ButtonWrapper> ImageButtons();
		ImageWrapper ImageWithId([NotNull] string id);
		IAmInputThatCanBeChanged InputWithId([NotNull] string id);
		IAmInputThatCanBeChanged InputWithLabel([NotNull] string label);
		IAmInputThatCanBeChanged InputWithValue([NotNull] string value);
		LabelWrapper LabelWithId([NotNull] string id);
		IEnumerable<LabelWrapper> Labels();
		LinkWrapper LinkWithId([NotNull] string id);
		LinkWrapper LinkWithText([NotNull] string text);
		IEnumerable<LinkWrapper> Links();
		INavigationControl NavigationControlWithId([NotNull] string id);
		INavigationControl NavigationControlWithText([NotNull] string text);
		PageWrapper Page();
		RadioOptionWrapper RadioOptionWithId([NotNull] string idOfOption);
		RadioOptionWrapper RadioOptionWithLabel([NotNull] string label);
		IEnumerable<RadioOptionWrapper> RadioOptions();
		IAmInputThatCanBeChanged Set([NotNull] string labelText);
		SpanWrapper SpanWithId([NotNull] string id);
		SpanWrapper SpanWithText([NotNull] string spanText);
		IEnumerable<SpanWrapper> Spans();
		TableWrapper TableWithId([NotNull] string id);
		IEnumerable<TableWrapper> Tables();
		TextBoxWrapper TextBoxWithId([NotNull] string id);
		TextBoxWrapper TextBoxWithLabel([NotNull] string label);
		IEnumerable<TextBoxWrapper> TextBoxes();
		IBrowserContext WaitUntil([NotNull] Func<IBrowserContext, bool> func, int secondsToWait = 10);
	}

	public class BrowserContext : IBrowserContext
	{
		private readonly IBrowserManager _browserManager;
		private IWebDriver _browser;

		public BrowserContext(IBrowserManager browserManager)
		{
			_browserManager = browserManager;
		}

		public string BaseUrl
		{
			get { return _browserManager.BaseUrl; }
		}

		public IWebDriver Browser
		{
			get { return _browser ?? (_browser = _browserManager.GetBrowser()); }
		}

		public ButtonWrapper ButtonWithId(string id)
		{
			const string howFound = "button with id '{0}'";
			var button = TryGetElementByIdAndInputType(id, "submit") ?? TryGetElementByIdAndInputType(id, "button");
			return new ButtonWrapper(button, String.Format(howFound, id), this);
		}

		public ButtonWrapper ButtonWithText(string text)
		{
			const string howFound = "button with visible text '{0}'";
			var button = Buttons().FirstOrDefault(x => x.Element.GetAttribute("value") == text);
			return new ButtonWrapper(button == null ? null : button.Element, String.Format(howFound, text), this);
		}

		public IEnumerable<ButtonWrapper> Buttons()
		{
			const string howFound = "type 'button'";
			var submits = GetInputsByInputType("submit");
			var buttons = GetInputsByInputType("button");
			return submits.Concat(buttons).Select(x => new ButtonWrapper(x, howFound, this));
		}

		public CheckBoxWrapper CheckBoxWithId(string id)
		{
			const string howFound = "checkbox with id '{0}'";
			var checkBox = TryGetElementByIdAndInputType(id, "checkbox");
			return new CheckBoxWrapper(checkBox, String.Format(howFound, id), this);
		}

		public CheckBoxWrapper CheckBoxWithLabel(string label)
		{
			var input = InputWithLabel(label);
			var checkBoxWrapper = input as CheckBoxWrapper;
			if (checkBoxWrapper != null)
			{
				return checkBoxWrapper;
			}
			return new CheckBoxWrapper(null, "checkbox with label '" + label + "'", input.BrowserContext);
		}

		public IEnumerable<CheckBoxWrapper> CheckBoxes()
		{
			const string howFound = "type 'checkbox'";
			var checkBoxes = GetInputsByInputType("checkbox");
			return checkBoxes.Select(x => new CheckBoxWrapper(x, howFound, this));
		}

		public void CloseBrowser()
		{
			_browserManager.Close();
		}

		public DialogHandlerWrapper Dialog(Action action)
		{
			return new DialogHandlerWrapper(this, action);
		}

		public DivWrapper DivWithId(string id)
		{
			const string howFound = "div with id '{0}'";
			var div = TryGetElementByIdAndTagType(id, "div");
			return new DivWrapper(div, String.Format(howFound, id), this);
		}

		public IEnumerable<DivWrapper> Divs()
		{
			const string howFound = "type 'div'";
			var divs = GetElementsByTagType("div");
			return divs.Select(x => new DivWrapper(x, howFound, this));
		}

		public DropDownListWrapper DropDownListWithId(string idOfList)
		{
			const string howFound = "drop down list with id '{0}'";
			var dropDownList = TryGetElementByIdAndTagType(idOfList, "select");
			return new DropDownListWrapper(dropDownList, String.Format(howFound, idOfList), this);
		}

		public DropDownListWrapper DropDownListWithLabel(string label)
		{
			var input = InputWithLabel(label);
			var downListWithLabel = input as DropDownListWrapper;
			if (downListWithLabel != null)
			{
				return downListWithLabel;
			}
			return new DropDownListWrapper(null, "drop down list with label '" + label + "'", input.BrowserContext);
		}

		public IEnumerable<DropDownListWrapper> DropDownLists()
		{
			const string howFound = "type 'select'";
			var dropDowns = GetElementsByTagType("select");
			return dropDowns.Select(x => new DropDownListWrapper(x, howFound, this));
		}

		public void GoToUrl(string url)
		{
			Browser.Navigate().GoToUrl(url);
		}

		public TextBoxWrapper HiddenWithId(string id)
		{
			const string howFound = "hidden input with id '{0}'";
			var hidden = TryGetElementByIdAndInputType(id, "hidden");
			return new TextBoxWrapper(hidden, String.Format(howFound, id), this);
		}

		public IEnumerable<TextBoxWrapper> Hiddens()
		{
			const string howFound = "type 'hidden'";
			var hiddens = GetElementsByTagType("input").Where(x => x.GetAttribute("type") == "hidden");
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
			var images = GetElementsByTagType("img");
			return images.Select(x => new ButtonWrapper(x, howFound, this));
		}

		public ImageWrapper ImageWithId(string id)
		{
			const string howFound = "image with id '{0}'";
			var button = TryGetElementByIdAndTagType(id, "img");
			return new ImageWrapper(button, String.Format(howFound, id), this);
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
			const string howFound = "label with id '{0}'";
			var label = TryGetElementByIdAndTagType(id, "label");
			return new LabelWrapper(label, String.Format(howFound, id), this);
		}

		public IEnumerable<LabelWrapper> Labels()
		{
			const string howFound = "type 'label'";
			var labels = GetElementsByTagType("label");
			return labels.Select(x => new LabelWrapper(x, howFound, this));
		}

		public LinkWrapper LinkWithId(string id)
		{
			const string howFound = "link with id '{0}'";
			var link = TryGetElementByIdAndTagType(id, "a");
			return new LinkWrapper(link, String.Format(howFound, id), this);
		}

		public LinkWrapper LinkWithText(string text)
		{
			var htmlEscapedText = HttpUtility.HtmlEncode(text);
			const string howFound = "link with visible text '{0}'";
			var link = Browser.FindElements(By.LinkText(text)).FirstOrDefault() ?? GetElementsByTagType("a").FirstOrDefault(x =>
			{
				var attribute = x.GetAttribute("innerHTML");
				return attribute == htmlEscapedText || attribute == text;
			});
			return new LinkWrapper(link, String.Format(howFound, text), this);
		}

		public IEnumerable<LinkWrapper> Links()
		{
			const string howFound = "type 'a'";
			var links = GetElementsByTagType("a");
			return links.Select(x => new LinkWrapper(x, howFound, this));
		}

		public INavigationControl NavigationControlWithId(string id)
		{
			var button = ButtonWithId(id);
			if (button.Exists().IsTrue)
			{
				return button;
			}
			return LinkWithId(id);
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

		public PageWrapper Page()
		{
			return new PageWrapper(this);
		}

		public RadioOptionWrapper RadioOptionWithId(string idOfOption)
		{
			const string howFound = "radio option with id '{0}'";
			var radioButton = TryGetElementByIdAndInputType(idOfOption, "radio");
			return new RadioOptionWrapper(radioButton, String.Format(howFound, idOfOption), this);
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

		public IEnumerable<RadioOptionWrapper> RadioOptions()
		{
			const string howFound = "type 'radio'";
			var radios = GetInputsByInputType("radio");
			return radios.Select(x => new RadioOptionWrapper(x, howFound, this));
		}

		public IAmInputThatCanBeChanged Set(string labelText)
		{
			return InputWithLabel(labelText);
		}

		public SpanWrapper SpanWithId(string id)
		{
			const string howFound = "span with id '{0}'";
			var span = TryGetElementByIdAndTagType(id, "span");
			return new SpanWrapper(span, String.Format(howFound, id), this);
		}

		public SpanWrapper SpanWithText(string spanText)
		{
			return Spans().First(x => x.Text() == spanText);
		}

		public IEnumerable<SpanWrapper> Spans()
		{
			const string howFound = "type 'span'";
			var spans = GetElementsByTagType("span");
			return spans.Select(x => new SpanWrapper(x, howFound, this));
		}

		public TableWrapper TableWithId(string id)
		{
			const string howFound = "table with id '{0}'";
			var table = TryGetElementByIdAndTagType(id, "table");
			return new TableWrapper(table, String.Format(howFound, id), this);
		}

		public IEnumerable<TableWrapper> Tables()
		{
			const string howFound = "type 'table'";
			var tables = GetElementsByTagType("table");
			return tables.Select(x => new TableWrapper(x, howFound, this));
		}

		public TextBoxWrapper TextBoxWithId(string id)
		{
			const string howFound = "text box with id '{0}'";
			var textField = TryGetElementByIdAndInputType(id, "text") ?? TryGetElementByIdAndTagType(id, "textarea");
			return new TextBoxWrapper(textField, String.Format(howFound, id), this);
		}

		public IEnumerable<TextBoxWrapper> TextBoxes()
		{
			var textBoxes = GetInputsByInputType("text").Select(x => new TextBoxWrapper(x, "type 'text'", this));
			var textAreas = GetElementsByTagType("textarea").Select(x => new TextBoxWrapper(x, "type 'textarea'", this));
			return textBoxes.Concat(textAreas);
		}

		public IBrowserContext WaitUntil(Func<IBrowserContext, bool> func, int secondsToWait = 10)
		{
			for (var i = 0; i < secondsToWait; i++)
			{
				try
				{
					if (func(this))
					{
						return this;
					}
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.Message + " -- waiting 1 sec");
				}
				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
			throw new AssertionException("state being waited upon never happened.");
		}

		public ContainerWrapper ContainerWithId(string id)
		{
			const string howFound = "container with id '{0}'";
			var container = TryGetElementByIdAndTagType(id, "div");
			if (container == null)
			{
				container = TryGetElementByIdAndTagType(id, "span");
			}
			if (container == null)
			{
				container = TryGetElementByIdAndTagType(id, "ul");
			}
			if (container == null)
			{
				container = TryGetElementByIdAndTagType(id, "ol");
			}
			if (container == null)
			{
				container = TryGetElementByIdAndTagType(id, "table");
			}
			return new ContainerWrapper(container, String.Format(howFound, id), this);
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

		private IEnumerable<IWebElement> GetElementsByTagType(string tag)
		{
			return Browser.FindElements(By.TagName(tag));
		}

		private IEnumerable<IWebElement> GetInputs()
		{
			return Browser.FindElements(By.TagName("input"));
		}

		private IEnumerable<IWebElement> GetInputsByInputType(string type)
		{
			return GetInputs().Where(x => x.GetAttribute("type") == type);
		}

		private IWebElement TryGetElementByIdAndInputType(string id, string type)
		{
			return Browser.FindElements(By.Id(id)).FirstOrDefault(x => x.GetAttribute("type") == type);
		}

		private IWebElement TryGetElementByIdAndTagType(string id, string tag)
		{
			return Browser.FindElements(By.Id(id)).FirstOrDefault(x => x.TagName == tag);
		}
	}
}
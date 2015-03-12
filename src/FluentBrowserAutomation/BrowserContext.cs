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
using OpenQA.Selenium.Support.UI;

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

        CheckBoxWrapper CheckBoxWithNameAndDataAttribute([NotNull] string name, [NotNull] string dataId,
            [NotNull] string dataValue);

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
        string GetHiddenDataValueWithId([NotNull] string id);
        string GetHiddenValueWithId([NotNull] string id);
        IEnumerable<RemoteWebElementWrapper> GetWebElementsWithClassName([NotNull] string className);
        void GoToUrl([NotNull] string url);
        TextBoxWrapper HiddenWithId([NotNull] string id);
        IEnumerable<TextBoxWrapper> Hiddens();
        BrowserContext IdOfFieldWithFocusShouldBe([NotNull] string expectedId);
        IEnumerable<ButtonWrapper> ImageButtons();
        ImageWrapper ImageWithId([NotNull] string id);
        IAmGenericInputThatCanBeChanged InputWithClassName([NotNull] string className);
        IAmGenericInputThatCanBeChanged InputWithId([NotNull] string id);
        IAmGenericInputThatCanBeChanged InputWithLabel([NotNull] string label);
        dynamic InputWithValue([NotNull] string value);
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
        IAmGenericInputThatCanBeChanged Set([NotNull] string labelText);
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

        IBrowserContext TryAndWaitUntil([NotNull] Func<IBrowserContext, bool> func,
            [NotNull] Func<IBrowserContext, bool> waitOn, int secondsToWait = 5, int totalNumberOfRetries = 1,
            string errorMessage = null);

        IBrowserContext WaitUntil([NotNull] Func<IBrowserContext, bool> func, int secondsToWait = 10,
            string errorMessage = null);

        IBrowserContext WaitUntil([NotNull] Func<IBrowserContext, bool> func,
            Func<string> buildActivityDescriptionMessage, int secondsToWait = 10);

        void WaitForPendingRequests(int milliseconds = 1000);
        int GetPendingRequests();
    }

    public class BrowserContext : IBrowserContext
    {
        private readonly IBrowserManager _browserManager;
        private readonly TimeSpan _timeToWait;
        private IWebDriver _browser;

        public BrowserContext(IBrowserManager browserManager)
        {
            _browserManager = browserManager;
            _timeToWait = TimeSpan.FromSeconds(.25);
        }

        public void WaitForPendingRequests(int milliseconds = 1000)
        {
            while (GetPendingRequests() > 0)
            {
                Thread.Sleep(milliseconds);
            }
        }

        public int GetPendingRequests()
        {
            int pendingRequests = 0;
            var js = Browser as IJavaScriptExecutor;
            if (js != null)
            {
                var executeScript =
                    js.ExecuteScript(
                        "return (typeof angular !== 'undefined' && angular.element(document.body).injector().get('$http').pendingRequests.length) || 0;");
                pendingRequests = executeScript is int ? (int) executeScript : 0;
            }
            return pendingRequests;
        }

        public ButtonWrapper ButtonWithId(string id)
        {
            string howFound = String.Format("button with id '{0}'", id);
            var button = this.TryGetElementById(id, RemoteWebElementWrapperExtensions.IsButton);
            return new ButtonWrapper(button, howFound, this);
        }

        public ButtonWrapper ButtonWithText(string text)
        {
            string howFound = String.Format("button with visible text '{0}'", text);
            var button = new ButtonWrapper(new RemoteWebElementWrapper(() =>
            {
                var firstOrDefault = Buttons().FirstOrDefault(x => x.Text == text);
                return firstOrDefault == null ? null : firstOrDefault.Element.RemoteWebElement;
            }), howFound, this);
            return button;
        }

        public IEnumerable<ButtonWrapper> Buttons()
        {
            const string howFound = "type 'button'";
            var inputWrappers = this.GetElements(By.XPath("//input[@type='submit' or @type='button']|//button"))
                .Select(input => new ButtonWrapper(input, howFound, this)).ToList();
            var buttonWrappers =
                this.GetElements(By.TagName("button")).Select(input => new ButtonWrapper(input, howFound, this));
            var listWrappers = inputWrappers.Concat(buttonWrappers);
            return listWrappers;
        }

        public IEnumerable<ButtonWrapper> ButtonsWithClassName(string className)
        {
            string howFound = String.Format("button with class '{0}'", className);
            return this.GetElementsByClassName(className, RemoteWebElementWrapperExtensions.IsButton)
                .Select(x => new ButtonWrapper(x, howFound, this));
        }

        public CheckBoxWrapper CheckBoxWithId(string id)
        {
            string howFound = String.Format("checkbox with id '{0}'", id);
            var checkBox = this.TryGetElementByIdAndType(id, "checkbox");
            return new CheckBoxWrapper(checkBox, howFound, this);
        }

        public CheckBoxWrapper CheckBoxWithIdAndValue(string id, string value)
        {
            string howFound = String.Format("checkbox with id '{0}'", id);
            var checkBox = this.TryGetElementByIdAndTypeAndValue(id, "checkbox", value);
            return new CheckBoxWrapper(checkBox, howFound, this);
        }

        public CheckBoxWrapper CheckBoxWithLabel(string label)
        {
            return InputWithLabel(label).AsCheckBox(String.Format("checkbox with label '{0}'", label));
        }

        public CheckBoxWrapper CheckBoxWithNameAndDataAttribute(string name, string dataId, string dataValue)
        {
            string howFound = String.Format("checkbox with name '{0}'", name);
            var checkBox = this.TryGetElementByNameAndData(name, dataId, dataValue);
            return new CheckBoxWrapper(checkBox, howFound, this);
        }

        public CheckBoxWrapper CheckBoxWithNameAndValue(string name, string value)
        {
            string howFound = String.Format("checkbox with name '{0}'", name);
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
            _browser = null;
        }

        public ContainerWrapper ContainerWithId(string id)
        {
            string howFound = String.Format("container with id '{0}'", id);
            var container = this.TryGetElementByIdAndTagName(id, "div", "span", "ul", "ol", "table");
            return new ContainerWrapper(container, howFound, this);
        }

        public DialogHandlerWrapper Dialog(Action action)
        {
            return new DialogHandlerWrapper(this, action);
        }

        public DivWrapper DivWithId(string id)
        {
            string howFound = String.Format("div with id '{0}'", id);
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
            string howFound = String.Format("div with class '{0}'", className);
            return this.GetElementsByClassNameAndTagName(className, "div")
                .Select(x => new DivWrapper(x, howFound, this));
        }

        public DropDownListWrapper DropDownListWithId(string idOfList)
        {
            string howFound = String.Format("drop down list with id '{0}'", idOfList);
            var dropDownList = this.TryGetElementByIdAndTagName(idOfList, "select");
            return new DropDownListWrapper(dropDownList, howFound, this);
        }

        public DropDownListWrapper DropDownListWithLabel(string label)
        {
            return InputWithLabel(label).AsDropDownList(String.Format("drop down list with label '{0}'", label));
        }

        public IEnumerable<DropDownListWrapper> DropDownLists()
        {
            const string howFound = "type 'select'";
            var dropDowns = this.GetElementsByTagName("select");
            return dropDowns.Select(x => new DropDownListWrapper(x, howFound, this));
        }

        public string GetHiddenDataValueWithId(string id)
        {
            var hidden = this.TryGetElementByIdAndType(id, "hidden");
            return hidden.GetAttribute("data-value");
        }

        public string GetHiddenValueWithId(string id)
        {
            var hidden = this.TryGetElementByIdAndType(id, "hidden");
            return hidden.GetAttribute("value");
        }

        public IEnumerable<RemoteWebElementWrapper> GetWebElementsWithClassName(string className)
        {
            return this.GetElementsByClassName(className);
        }

        public void GoToUrl(string url)
        {
            Browser.Navigate().GoToUrl(url);
        }

        public TextBoxWrapper HiddenWithId(string id)
        {
            string howFound = String.Format("hidden input with id '{0}'", id);
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
            string id = Browser.SwitchTo().ActiveElement().GetAttribute("id");
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
            string howFound = String.Format("image with id '{0}'", id);
            var button = this.TryGetElementByIdAndTagName(id, "img");
            return new ImageWrapper(button, howFound, this);
        }

        public IAmGenericInputThatCanBeChanged InputWithClassName(string className)
        {
            _browserManager.Trace("Getting input with class name '" + className + "'");
            return SetterFor.InputWithClassName(this, className);
        }

        public IAmGenericInputThatCanBeChanged InputWithId(string id)
        {
            _browserManager.Trace("Getting input with id '" + id + "'");
            return SetterFor.InputWithId(this, id);
        }

        public IAmGenericInputThatCanBeChanged InputWithLabel(string label)
        {
            _browserManager.Trace("Getting input with label '" + label + "'");
            return SetterFor.InputWithLabel(this, label);
        }

        public dynamic InputWithValue(string value)
        {
            _browserManager.Trace("Getting input with value '" + value + "'");
            return SetterFor.InputWithValue(this, value);
        }

        public LabelWrapper LabelWithId(string id)
        {
            string howFound = String.Format("label with id '{0}'", id);
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
            string howFound = String.Format("link with id '{0}'", id);
            var link = this.TryGetElementByIdAndTagName(id, "a");
            return new LinkWrapper(link, howFound, this);
        }

        public LinkWrapper LinkWithText(string text)
        {
            string howFound = String.Format("link with visible text '{0}'", text);
            var link = new LinkWrapper(new RemoteWebElementWrapper(() =>
            {
                var firstOrDefault = LinksWithText(text).FirstOrDefault();
                return firstOrDefault == null ? null : firstOrDefault.Element.RemoteWebElement;
            }), howFound, this);
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
            string howFound = String.Format("link with class '{0}'", className);
            return this.GetElementsByClassNameAndTagName(className, "a")
                .Select(x => new LinkWrapper(x, howFound, this));
        }

        public IEnumerable<LinkWrapper> LinksWithText(string text)
        {
            string htmlEscapedText = HttpUtility.HtmlEncode(text);
            string howFound = String.Format("link with visible text '{0}'", text);
            var items = this.GetElements(By.LinkText(text))
                .Concat(this.GetElementsByTagName("a").Where(x =>
                {
                    var attribute = x.GetAttribute("innerHTML");
                    return attribute == htmlEscapedText ||
                           attribute == text ||
                           attribute.Trim() == text;
                })).Select(link => new LinkWrapper(link, howFound, this));
            return items;
        }

        public ListWrapper ListWithId(string id)
        {
            string howFound = String.Format("list with id '{0}'", id);
            var list = this.TryGetElementByIdAndTagName(id, "ul");
            return new ListWrapper(list, howFound, this);
        }

        public INavigationControl NavigationControlWithId(string id)
        {
            return new GenericNavigationControl(new RemoteWebElementWrapper(() =>
            {
                var elementById = this.TryGetElementById(id);
                return elementById == null ? null : elementById.RemoteWebElement;
            }), "navigation control with id '" + id + "'", this);
        }

        public INavigationControl NavigationControlWithText(string text)
        {
            return new GenericNavigationControl(new RemoteWebElementWrapper(() =>
            {
                var button = ButtonWithText(text);
                if (button.Exists().IsTrue)
                {
                    return button.Element.RemoteWebElement;
                }
                var link = LinkWithText(text);
                if (link.Exists().IsTrue)
                {
                    return link.Element.RemoteWebElement;
                }
                return null;
            }), "navigation control with text '" + text + "'", this);
        }

        public IEnumerable<INavigationControl> NavigationControlsWithClassName(string className)
        {
            string howFound = String.Format("navigation control with class '{0}'", className);
            var buttonsAndLinks = this.GetElementsByClassName(className)
                .Where(x => x.IsButton() || x.IsLink())
                .Select(x => new GenericNavigationControl(x, howFound, this))
                .ToArray();
            return buttonsAndLinks;
        }

        public PageWrapper Page()
        {
            return new PageWrapper(this);
        }

        public RadioOptionWrapper RadioOptionWithId(string idOfOption)
        {
            string howFound = String.Format("radio option with id '{0}'", idOfOption);
            var radioButton = this.TryGetElementByIdAndType(idOfOption, "radio");
            return new RadioOptionWrapper(radioButton, howFound, this);
        }

        public RadioOptionWrapper RadioOptionWithLabel(string label)
        {
            return InputWithLabel(label).AsRadioOption("radio option with label '" + label + "'");
        }

        public RadioOptionWrapper RadioOptionWithNameAndValue(string name, string value)
        {
            string howFound = String.Format("radio option with name '{0}'", name);
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
            string howFound = String.Format("type 'radio' with name '{0}'", name);
            var checkBoxes = this.GetInputsByTypeAndName("radio", name);
            return checkBoxes.Select(x => new RadioOptionWrapper(x, howFound, this));
        }

        public IAmGenericInputThatCanBeChanged Set(string labelText)
        {
            return InputWithLabel(labelText);
        }

        public SpanWrapper SpanWithId(string id)
        {
            string howFound = String.Format("span with id '{0}'", id);
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
            string howFound = String.Format("span with class '{0}'", className);
            return this.GetElementsByClassNameAndTagName(className, "span")
                .Select(x => new SpanWrapper(x, howFound, this));
        }

        public TableWrapper TableWithId(string id)
        {
            string howFound = String.Format("table with id '{0}'", id);
            var table = this.TryGetElementByIdAndTagName(id, "table");
            return new TableWrapper(table, howFound, this);
        }

        public IEnumerable<TableWrapper> Tables()
        {
            const string howFound = "type 'table'";
            var tables = this.GetElementsByTagName("table");
            return tables.Select((x, index) => new TableWrapper(x, howFound, this));
        }

        public TextBoxWrapper TextBoxWithFocus()
        {
            var active = Browser.SwitchTo().ActiveElement();
            return new TextBoxWrapper(new RemoteWebElementWrapper(() => null, active), "textbox with focus", this);
        }

        public TextBoxWrapper TextBoxWithId(string id)
        {
            string howFound = String.Format("text box with id '{0}'", id);
            var textField =
                this.TryGetElement(By.XPath("//textarea[@id='" + id + "']|//input[@id='" + id + "' and @type='text']"));
            return new TextBoxWrapper(textField, howFound, this);
        }

        public TextBoxWrapper TextBoxWithLabel(string label)
        {
            return InputWithLabel(label).AsTextBox("Textbox with label '" + label + "'");
        }

        public IEnumerable<TextBoxWrapper> TextBoxes()
        {
            var textBoxes = this.GetElements(By.XPath("//textarea|//input[@type='text']"))
                .Select(x => new TextBoxWrapper(x, "text box or textarea", this));
            return textBoxes;
        }

        public IBrowserContext TryAndWaitUntil(Func<IBrowserContext, bool> func, Func<IBrowserContext, bool> waitOn,
            int secondsToWait = 5, int totalNumberOfRetries = 1, string errorMessage = null)
        {
            int numberOfTries = 0;
            Exception caughtException = null;

            WaitForPendingRequests();

            while (numberOfTries < totalNumberOfRetries)
            {
                func(this);
                var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(secondsToWait));
                try
                {
                    bool result = wait.Until(b => waitOn(this));
                    if (result)
                    {
                        return this;
                    }
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception exception)
                {
                    caughtException = exception;
                }

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

        public IBrowserContext WaitUntil(Func<IBrowserContext, bool> func, int secondsToWait = 10,
            string errorMessage = null)
        {
            WaitForPendingRequests();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Exception caughtException = null;
            do
            {
                try
                {
                    bool result = func(this);
                    if (result)
                    {
                        return this;
                    }
                    Thread.Sleep(_timeToWait);
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("stale element reference"))
                    {
                        caughtException = exception;
                        continue;
                    }
                    if (errorMessage != null)
                    {
                        throw new ArgumentException("WaitUntil '" + errorMessage + "' caught: " + exception.Message,
                            exception);
                    }
                    throw new ArgumentException(exception.Message, exception);
                }
            } while (stopwatch.Elapsed.TotalSeconds < secondsToWait);

            if (caughtException != null)
            {
                throw new ArgumentException("WaitUntil '" + errorMessage + "' caught: " + caughtException.Message,
                    caughtException);
            }
            throw new AssertionException(errorMessage ?? "state being waited upon never happened.");
        }

        public IBrowserContext WaitUntil(Func<IBrowserContext, bool> func, Func<string> buildActivityDescriptionMessage,
            int secondsToWait = 10)
        {
            WaitForPendingRequests();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Exception caughtException = null;
            do
            {
                try
                {
                    bool result = func(this);
                    if (result)
                    {
                        return this;
                    }
                    Thread.Sleep(_timeToWait);
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("stale element reference"))
                    {
                        caughtException = exception;
                        continue;
                    }
                    throw new ArgumentException(
                        "WaitUntil '" + buildActivityDescriptionMessage() + "' caught: " + exception.Message, exception);
                }
            } while (stopwatch.Elapsed.TotalSeconds < secondsToWait);
            if (caughtException != null)
            {
                throw new ArgumentException(
                    "WaitUntil '" + buildActivityDescriptionMessage() + "' caught: " + caughtException.Message,
                    caughtException);
            }
            throw new AssertionException(buildActivityDescriptionMessage() ?? "state being waited upon never happened.");
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
        internal static IEnumerable<RemoteWebElementWrapper> GetElements(this IBrowserContext browserContext, By @by,
            Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            Func<IEnumerable<IWebElement>> result = () => browserContext.Browser.FindElements(@by);
            var items = result().Select(x => new RemoteWebElementWrapper(null, x));
            return isMatch != null ? items.Where(isMatch) : items;
        }

        internal static IEnumerable<RemoteWebElementWrapper> GetElementsByClassName(this IBrowserContext browserContext,
            string className, Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            return browserContext.GetElements(By.ClassName(className), isMatch);
        }

        internal static IEnumerable<RemoteWebElementWrapper> GetElementsByClassNameAndTagName(
            this IBrowserContext browserContext, string className, string tagName)
        {
            var xPath = By.XPath("//" + tagName + "[contains(@class, '" + className + "')]");
            return browserContext.GetElements(xPath);
        }

        internal static IEnumerable<RemoteWebElementWrapper> GetElementsByTagName(this IBrowserContext browserContext,
            string tag, Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            return browserContext.GetElements(By.TagName(tag), isMatch);
        }

        internal static IEnumerable<RemoteWebElementWrapper> GetInputs(this IBrowserContext browserContext,
            Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            return browserContext.GetElementsByTagName("input", isMatch);
        }

        internal static IEnumerable<RemoteWebElementWrapper> GetInputsByType(this IBrowserContext browserContext,
            params string[] type)
        {
            return browserContext.GetElements(By.XPath("//input[@type='" + type + "']"));
        }

        internal static IEnumerable<RemoteWebElementWrapper> GetInputsByTypeAndName(this IBrowserContext browserContext,
            string type, string name, Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            return browserContext.GetElements(By.XPath("//input[@type='" + type + "' and @name='" + name + "']"),
                isMatch);
        }

        internal static RemoteWebElementWrapper TryGetElement(this IBrowserContext browserContext, By by,
            Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            Func<IWebElement> howToGetIt = () =>
            {
                var item = browserContext.GetElements(@by, isMatch).FirstOrDefault();
                return item == null ? null : item.RemoteWebElement;
            };
            return new RemoteWebElementWrapper(howToGetIt);
        }

        internal static RemoteWebElementWrapper TryGetElementById(this IBrowserContext browserContext, string id,
            Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            return browserContext.TryGetElement(By.Id(id), isMatch);
        }

        internal static RemoteWebElementWrapper TryGetElementByIdAndTagName(this IBrowserContext browserContext,
            string id, params string[] tagNames)
        {
            string query = String.Join("|", tagNames.Select(tagName => "//" + tagName + "[@id='" + id + "']"));
            return browserContext.TryGetElement(By.XPath(query));
        }

        internal static RemoteWebElementWrapper TryGetElementByIdAndType(this IBrowserContext browserContext, string id,
            params string[] types)
        {
            string typeQuery = String.Join(" or ", types.Select(x => "@type = '" + x + "'"));
            return browserContext.TryGetElement(By.XPath("//*[@id='" + id + "' and (" + typeQuery + ")]"));
        }

        internal static RemoteWebElementWrapper TryGetElementByIdAndTypeAndValue(this IBrowserContext browserContext,
            string id, string type, string value)
        {
            return
                browserContext.TryGetElement(
                    By.XPath("//*[@id='" + id + "' and @type='" + type + "' and " + value.EscapeForXpath("@value") + "]"));
        }

        internal static RemoteWebElementWrapper TryGetElementByName(this IBrowserContext browserContext, string name,
            Func<RemoteWebElementWrapper, bool> isMatch = null)
        {
            return browserContext.TryGetElement(By.Name(name), isMatch);
        }

        internal static RemoteWebElementWrapper TryGetElementByNameAndData(this IBrowserContext browserContext,
            string name, string dataId, string dataValue)
        {
            return
                browserContext.TryGetElement(
                    By.XPath("//*[@name='" + name + "' and " + dataValue.EscapeForXpath("@data-" + dataId) + "]"));
        }

        internal static RemoteWebElementWrapper TryGetElementByNameAndTypeAndValue(this IBrowserContext browserContext,
            string name, string type, string value)
        {
            return
                browserContext.TryGetElement(
                    By.XPath("//*[@name='" + name + "' and @type='" + type + "' and " + value.EscapeForXpath("@value") +
                             "]"));
        }
    }
}
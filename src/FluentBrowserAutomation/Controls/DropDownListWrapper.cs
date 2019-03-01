using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssert;

using FluentBrowserAutomation.Extensions;

using JetBrains.Annotations;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class DropDownListWrapper : BasicInfoWrapper, ICouldBeDisabled, IAmGenericInputThatCanBeChanged, IAmSelectionInput, INeedFocus
	{
		private const string SideBySideIdDestinationSuffix = "__dx";
		private const string SideBySideIdMarker = "ms2side";
		private const string SideBySideIdSourceSuffix = "__sx";

		public DropDownListWrapper(RemoteWebElementWrapper dropDownList, string howFound, IBrowserContext browserContext)
			: base(dropDownList, howFound, browserContext)
		{
		}

		private IEnumerable<RemoteWebElementWrapper> GetOptions(Func<IWebElement, bool> isMatch = null)
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			return Element.GetChildElementsByTagName("option", isMatch).Select(x => new RemoteWebElementWrapper(null, x, BrowserContext));
		}

		private IEnumerable<RemoteWebElementWrapper> GetOptionsWithText(string text)
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			var ddlId = Element.GetAttribute("id");
			var selector = By.XPath("//select[" + ddlId.EscapeForXpath("@id") + "]/option[" + text.EscapeForXpath("normalize-space(text())") + "]");
			var children = BrowserContext.GetElements(selector);
			return children;
		}

		private IEnumerable<RemoteWebElementWrapper> GetOptionsWithTextOrValue(string toFind)
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			var ddlId = Element.GetAttribute("id");
			var selector = By.XPath("//select[" + ddlId.EscapeForXpath("@id") + "]/option[" + toFind.EscapeForXpath("normalize-space(text())")
				+ " or " + toFind.EscapeForXpath("@value") + "]");
			var children = BrowserContext.GetElements(selector);
			return children;
		}

		private IEnumerable<RemoteWebElementWrapper> GetOptionsWithValue(string value)
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			var ddlId = Element.GetAttribute("id");
			var selector = By.XPath("//select[" + ddlId.EscapeForXpath("@id") + "]/option[" + value.EscapeForXpath("@value") + "]");
			var children = BrowserContext.GetElements(selector);
			return children;
		}

		public string GetSelectedText()
		{
			var selectedOption = GetOptions(x => x.Selected).FirstOrDefault();
			return selectedOption == null ? "" : selectedOption.Text;
		}

		public IEnumerable<string> GetSelectedTexts()
		{
			return GetOptions(x => x.Selected).Select(x => x.Text);
		}

		public IEnumerable<string> GetSelectedValues()
		{
			return GetOptions(x => x.Selected).Select(x => x.GetAttribute("value"));
		}

		private void HandleSideBySide(string id, string text, bool verifySelected)
		{
			var sourceDdl = BrowserContext.DropDownListWithId(id + SideBySideIdMarker + SideBySideIdSourceSuffix);

			SelectOption(sourceDdl, text, false);

			// side by side drop down
			var parent = sourceDdl.Element.GetParent().GetParent();
			var paraButtons = parent.GetChildElementsByTagName("p");
			var moveToRightPseudoButton = paraButtons.First(x => x.Text.Equals("›")); // note: › not >
			moveToRightPseudoButton.Click(); // sets focus
			moveToRightPseudoButton.Click(); // moves the element to the targetDdl

			if (verifySelected)
			{
				var targetDdl = BrowserContext.DropDownListWithId(id + SideBySideIdMarker + SideBySideIdDestinationSuffix);
				BrowserContext.WaitUntil(x => targetDdl.GetOptionsWithTextOrValue(text).Any(), errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
			}
		}

		public bool HasOption(string expected)
		{
			return GetOptionsWithText(expected).Any();
		}

		private bool HasSideBySide(string id)
		{
			return BrowserContext.DropDownListWithId(id + SideBySideIdMarker + SideBySideIdSourceSuffix).Exists().IsTrue;
		}

		public bool HasTextValue(string expected)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");
			return GetOptionsWithText(expected).Any();
		}

		public OptionWrapper OptionWithText([NotNull] string text)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");
			BrowserContext.WaitUntil(x => this.IsEnabled().IsTrue, errorMessage:"wait for " + HowFound + " to be enabled");

			var howFound = string.Format("option with text '{0}'", text);
			var option = GetOptionsWithText(text).FirstOrDefault();
			return new OptionWrapper(option, howFound, this, BrowserContext);
		}

		public OptionWrapper OptionWithValue([NotNull] string text)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");
			BrowserContext.WaitUntil(x => this.IsEnabled().IsTrue, errorMessage:"wait for " + HowFound + " to be enabled");

			var howFound = string.Format("option with value '{0}'", text);
			var option = GetOptionsWithValue(text).FirstOrDefault();
			return new OptionWrapper(option, howFound, this, BrowserContext);
		}

		public void Select(string text)
		{
			Select(text, true);
		}

		public void Select(string text, bool verifySelected)
		{
			var id = Id;

			text = (text ?? "").Trim();
			try
			{
				if (HasSideBySide(id))
				{
					HandleSideBySide(id, text, verifySelected);
					// verification is handled in the recursion
				}
				else
				{
					SelectOption(this, text, verifySelected);
				}
			}
			catch (NoSuchElementException)
			{
				try
				{
					if (HasSideBySide(id))
					{
						HandleSideBySide(id, text, verifySelected);
						// verification is handled in the recursion
					}
					else
					{
						SelectOption(this, text, verifySelected);
					}
				}
				catch (NoSuchElementException)
				{
					throw new AssertionException(string.Format("{0} does not have option '{1}'", HowFound, text));
				}
			}
			BrowserContext.WaitForPendingRequests();
		}

		public void SelectAnyOptionExcept(params string[] unwantedValues)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");
			RemoteWebElementWrapper option = null;
			var partialXpath = string.Join(" and ", unwantedValues.Select(y => "not(" + y.EscapeForXpath("text()") + ")"));
			var ddlId = Element.GetAttribute("id");
			var selector = By.XPath("//select[" + ddlId.EscapeForXpath("@id") + "]/option[" + partialXpath + "]");
			this.WaitUntil(x => (option = BrowserContext.TryGetElement(selector)) != null,
				errorMessage:"wait for " + HowFound + " to have option other than: " + string.Join(" or ", unwantedValues));

			Select(option.Text);
		}

		private void SelectOption(DropDownListWrapper dropDown, string text, bool verifySelected)
		{
			BrowserContext.WaitUntil(x => dropDown.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");

			RemoteWebElementWrapper option = null;
			BrowserContext.WaitUntil(x => (option = dropDown.GetOptionsWithTextOrValue(text).FirstOrDefault()) != null, errorMessage:"wait for " + dropDown.HowFound + " to have option '" + text + "'");
			if (option != null)
			{
				option.Click();
				dropDown.BrowserContext.WaitForPendingRequests();
			}
			if (verifySelected)
			{
				BrowserContext.WaitUntil(x => dropDown.GetOptionsWithTextOrValue(text).Any(y => y.Selected),
					errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
			}
		}

		public void SelectedOptionShouldBeEqualTo(string text)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");
			BrowserContext.WaitUntil(x => Options.Any(), errorMessage:"wait for " + HowFound + " options to be visible");
			var optionsWithText = GetOptionsWithText(text);
			var textIsSelected = optionsWithText.Any(x => x.Selected);
			if (!textIsSelected)
			{
				var selectedTexts = GetSelectedTexts().ToArray();
				selectedTexts.Contains(text).ShouldBeTrue("Selected value of " + HowFound + " should be '" + text + "' but is/are '" + string.Join(", ", selectedTexts) + "'");
			}
		}

		public DropDownListWrapper SetTo(string text)
		{
			Select(text);
			return this;
		}

		public void ShouldNotHaveOption(string optionText)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage:"wait for " + HowFound + " to be visible");
			var anyOptionsWithText = GetOptionsWithText(optionText).Any();
			anyOptionsWithText.ShouldBeFalse(HowFound + " should not have option '" + optionText + "' but does.");
		}

		public IEnumerable<OptionWrapper> Options
		{
			get { return GetOptions().Select(x => new OptionWrapper(x, "", this, BrowserContext)); }
		}
	}
}
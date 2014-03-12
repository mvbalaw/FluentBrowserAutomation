using System;
using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Extensions;

using JetBrains.Annotations;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FluentBrowserAutomation.Controls
{
	public class DropDownListWrapper : BasicInfoWrapper, ICouldBeDisabled, IAmInputThatCanBeChanged, IAmSelectionInput, INeedFocus
	{
		public DropDownListWrapper(IWebElement dropDownList, string howFound, IBrowserContext browserContext)
			: base(dropDownList, howFound, browserContext)
		{
		}

		public string GetSelectedText()
		{
			var selectedOption = Element.FindElements(By.TagName("option")).FirstOrDefault(x => x.Selected);
			return selectedOption == null ? "" : selectedOption.Text;
		}

		public IEnumerable<string> GetSelectedTexts()
		{
			return Element.FindElements(By.TagName("option")).Where(x => x.Selected).Select(x => x.Text);
		}

		public IEnumerable<string> GetSelectedValues()
		{
			return Element.FindElements(By.TagName("option")).Where(x => x.Selected).Select(x => x.GetAttribute("value"));
		}

		private void HandleSideBySide(string id)
		{
			if (IsSideBySide(id))
			{
				// side by side drop down
				var parent = Element.GetParent().GetParent();
				var paraButtons = parent.FindElements(By.TagName("p"));
				var moveToRightPseudoButton = paraButtons.First(x => x.Text.Equals("›")); // note: › not >
				moveToRightPseudoButton.Click(); // sets focus
				moveToRightPseudoButton.Click();
			}
		}

		private static bool IsSideBySide(string id)
		{
			return id.Contains("ms2side");
		}

		public OptionWrapper OptionWithText([NotNull] string text)
		{
			this.Exists().ShouldBeTrue();
			this.IsEnabled().ShouldBeTrue();
			this.IsVisible().ShouldBeTrue();
			var option = OptionWithText(text, BrowserContext);
			return option;
		}

		private OptionWrapper OptionWithText([NotNull] string text, IBrowserContext browserContext)
		{
			const string optionWithText = "option with text '{0}'";
			var options = Element.FindElements(By.TagName("option"));
			var option = options.FirstOrDefault(x => x.Text == text);
			return new OptionWrapper(option, String.Format(optionWithText, text), this, browserContext);
		}

		public OptionWrapper OptionWithValue([NotNull] string text)
		{
			this.Exists().ShouldBeTrue();
			this.IsEnabled().ShouldBeTrue();
			this.IsVisible().ShouldBeTrue();
			var option = OptionWithValue(text, BrowserContext);
			return option;
		}

		private OptionWrapper OptionWithValue([NotNull] string value, IBrowserContext browserContext)
		{
			const string optionWithValue = "option with value '{0}'";
			var options = Element.FindElements(By.TagName("option"));
			var option = options.FirstOrDefault(x => x.GetAttribute("value") == value);
			return new OptionWrapper(option, String.Format(optionWithValue, value), this, browserContext);
		}

		public void Select(string text)
		{
			Select(text, true);
		}

		public void Select(string text, bool verifySelected)
		{
			var selector = new SelectElement(Element);
			var id = Id;

			try
			{
				selector.SelectByText(text);
				HandleSideBySide(id);
				if (verifySelected)
				{
					BrowserContext.WaitUntil(x => x.DropDownListWithId(id + (IsSideBySide(id) ? "__dx" : "")).GetSelectedTexts().Any(y => y.Equals(text)), errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
				}
			}
			catch (NoSuchElementException)
			{
				try
				{
					selector.SelectByValue(text);
					HandleSideBySide(id);

					if (verifySelected)
					{
						BrowserContext.WaitUntil(x => x.DropDownListWithId(id + (IsSideBySide(id) ? "__dx" : "")).GetSelectedValues().Any(y => y.Equals(text)), errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
					}
				}
				catch (NoSuchElementException)
				{
					throw new AssertionException(String.Format("{0} does not have option '{1}'", HowFound, text));
				}
			}
		}

		public IEnumerable<OptionWrapper> Options
		{
			get { return Element.FindElements(By.TagName("option")).Select(x => new OptionWrapper(x, "", this, BrowserContext)); }
		}
	}
}
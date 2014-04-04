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
		private const string SideBySideIdDestinationSuffix = "__dx";
		private const string SideBySideIdMarker = "ms2side";
		private const string SideBySideIdSourceSuffix = "__sx";

		public DropDownListWrapper(IWebElement dropDownList, string howFound, IBrowserContext browserContext)
			: base(dropDownList, howFound, browserContext)
		{
		}

		private IEnumerable<IWebElement> GetOptions(Func<IWebElement, bool> isMatch = null)
		{
			return Element.GetChildElementsByTagName("option", isMatch);
		}

		private IEnumerable<IWebElement> GetOptionsWithText(string text)
		{
			var children = Element.FindElements(By.XPath("//option[normalize-space(text())='" + text.Replace("'", "&apos;") + "']"));
			return children;
		}

		private IEnumerable<IWebElement> GetOptionsWithValue(string value)
		{
			var children = Element.FindElements(By.XPath("//option[@value='" + value.Replace("'", "&apos;") + "']"));
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
			sourceDdl.Select(text, verifySelected);

			// side by side drop down
			var parent = sourceDdl.Element.GetParent().GetParent();
			var paraButtons = parent.GetChildElementsByTagName("p");
			var moveToRightPseudoButton = paraButtons.First(x => x.Text.Equals("›")); // note: › not >
			moveToRightPseudoButton.Click(); // sets focus
			moveToRightPseudoButton.Click();

			if (verifySelected)
			{
				BrowserContext.WaitUntil(x => x.DropDownListWithId(id + SideBySideIdMarker + SideBySideIdDestinationSuffix).Options.Any(y => text.Equals(y.Text.Text) || text.Equals(y.Value.Text)), errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
			}
		}

		private bool HasSideBySide(string id)
		{
			return BrowserContext.DropDownListWithId(id + SideBySideIdMarker + SideBySideIdSourceSuffix).Exists().IsTrue;
		}

		public OptionWrapper OptionWithText([NotNull] string text)
		{
			this.Exists().ShouldBeTrue();
			this.IsEnabled().ShouldBeTrue();
			this.IsVisible().ShouldBeTrue();
			var howFound = String.Format("option with text '{0}'", text);
			var option = GetOptionsWithText(text).FirstOrDefault();
			return new OptionWrapper(option, howFound, this, BrowserContext);
		}

		public OptionWrapper OptionWithValue([NotNull] string text)
		{
			this.Exists().ShouldBeTrue();
			this.IsEnabled().ShouldBeTrue();
			this.IsVisible().ShouldBeTrue();
			var howFound = String.Format("option with value '{0}'", text);
			var option = GetOptionsWithValue(text).FirstOrDefault();
			return new OptionWrapper(option, howFound, this, BrowserContext);
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
				if (HasSideBySide(id))
				{
					HandleSideBySide(id, text, verifySelected);
					// verification is handled in the recursion
				}
				else
				{
					selector.SelectByText(text);
					if (verifySelected)
					{
						BrowserContext.WaitUntil(x => x.DropDownListWithId(id).GetOptionsWithText(text).Any(y => y.Selected), errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
					}
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
						selector.SelectByValue(text);
						if (verifySelected)
						{
							BrowserContext.WaitUntil(x => x.DropDownListWithId(id).GetOptionsWithValue(text).Any(y => y.Selected), errorMessage:"Failed to set selected value of " + HowFound + " to '" + text + "'");
						}
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
			get { return GetOptions().Select(x => new OptionWrapper(x, "", this, BrowserContext)); }
		}
	}
}
using System;
using System.Linq;

using FluentAssert;

using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Declarative
{
	public static class SetterFor
	{
		private static IAmInputThatCanBeChanged GetInputElement(IBrowserContext browserContext, IWebElement control, string howFound)
		{
			if (control.IsTextBox())
			{
				return new TextBoxWrapper(control, howFound, browserContext);
			}
			if (control.IsDropDownList())
			{
				return new DropDownListWrapper(control, howFound, browserContext);
			}
			if (control.IsCheckBox())
			{
				return new CheckBoxWrapper(control, howFound, browserContext);
			}
			if (control.IsRadioOption())
			{
				return new RadioOptionWrapper(control, howFound, browserContext);
			}
			if (control.IsFileUpload())
			{
				return new FileUploadWrapper(control, howFound, browserContext);
			}

			return new MissingInputWrapper(howFound, browserContext);
		}

		public static IAmInputThatCanBeChanged InputWithClassName(this IBrowserContext browserContext, string className)
		{
			var control = browserContext.GetElementsByClassName(className).FirstOrDefault();
			var howFound = String.Format("Input with class '{0}'", className);
			return GetInputElement(browserContext, control, howFound);
		}

		public static IAmInputThatCanBeChanged InputWithId(this IBrowserContext browserContext, string id)
		{
			var control = browserContext.TryGetElementById(id);
			var howFound = String.Format("Input with id '{0}'", id);
			return GetInputElement(browserContext, control, howFound);
		}

		public static IAmInputThatCanBeChanged InputWithLabel(this IBrowserContext browserContext, string labelText)
		{
			var labels = browserContext.Labels();
			var label = labels.FirstOrDefault(x => ((string)x.Text()).Trim() == labelText.Trim());
			label.ShouldNotBeNull(String.Format("Could not find Label with text '{0}'", labelText.Trim()));

//// ReSharper disable PossibleNullReferenceException
			var itsLinkedControlId = label.For;
//// ReSharper restore PossibleNullReferenceException
			itsLinkedControlId.ShouldNotBeNullOrEmpty(String.Format("Label with text '{0}' does not have a For attribute", labelText));
			var control = browserContext.TryGetElementById(itsLinkedControlId);
			var howFound = "Input with id '" + itsLinkedControlId + "' as referenced in For attribute of Label with text '" + labelText + "'";

			return GetInputElement(browserContext, control, howFound);
		}

		public static IAmInputThatCanBeChanged InputWithValue(this IBrowserContext browserContext, string value)
		{
			var textBoxes = browserContext.TextBoxes();
			var textBoxesWithValue = textBoxes.Where(x => ((string)x.Text()).Trim() == value.Trim()).ToList();

			var dropDowns = browserContext.DropDownLists();
			var dropDownsWithValue = dropDowns.Where(x => x.GetSelectedTexts().Any(y => y == value)).ToList();

			var checkboxesAndRadiosWithValue = browserContext
				.GetInputs(x => x.TypeAttributeHasValue("checkbox", "radio") && x.ValueAttributeHasValue(value))
				.ToList();

			var elements = textBoxesWithValue.Select(x => x.Element)
				.Concat(dropDownsWithValue.Select(x => x.Element))
				.Concat(checkboxesAndRadiosWithValue)
				.ToList();

			elements.Count.ShouldBeLessThan(2, string.Format("Unexpectedly found multiple fields with value '{0}'", value));

			var howFound = "Input with value '" + value + "'";
			var element = elements.FirstOrDefault();

			if (textBoxesWithValue.Any())
			{
				var input = textBoxesWithValue.First();
				input.HowFound = howFound;
				return input;
			}
			if (dropDownsWithValue.Any())
			{
				var input = dropDownsWithValue.First();
				input.HowFound = howFound;
				return input;
			}

			return GetInputElement(browserContext, element, howFound);
		}
	}
}
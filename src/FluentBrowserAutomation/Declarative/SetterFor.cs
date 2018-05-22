using System.Linq;

using FluentAssert;

using FluentBrowserAutomation.Controls;
using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation.Declarative
{
	public static class SetterFor
	{
		private static IAmGenericInputThatCanBeChanged GetInputElement(IBrowserContext browserContext, RemoteWebElementWrapper control, string howFound)
		{
			return new GenericInputThatCanBeChangedWrapper(control,howFound, browserContext);
		}

		public static IAmGenericInputThatCanBeChanged InputWithClassName(this IBrowserContext browserContext, string className)
		{
			var control = browserContext.GetElementsByClassName(className).FirstOrDefault();
			var howFound = string.Format("Input with class '{0}'", className);
			return GetInputElement(browserContext, control, howFound);
		}

		public static IAmGenericInputThatCanBeChanged InputWithId(this IBrowserContext browserContext, string id)
		{
			var control = browserContext.TryGetElementById(id);
			var howFound = string.Format("Input with id '{0}'", id);
			return GetInputElement(browserContext, control, howFound);
		}

		public static IAmGenericInputThatCanBeChanged InputWithLabel(this IBrowserContext browserContext, string labelText)
		{
			LabelWrapper label = null;
			var trimmedLabelText = labelText.Trim();
			browserContext.WaitUntil(x => (label = x.LabelsWithText(trimmedLabelText).FirstOrDefault(y=>y.Element.RemoteWebElement.Displayed)) != null, () => "wait for label with text '" + trimmedLabelText + "' to exist, found: " + string.Join(", ", browserContext.Labels().Select(x => x.Text().Text).ToArray()));
			label.ShouldNotBeNull(string.Format("Could not find Label with text '{0}'", trimmedLabelText));

//// ReSharper disable PossibleNullReferenceException
			var itsLinkedControlId = label.For;
//// ReSharper restore PossibleNullReferenceException
			itsLinkedControlId.ShouldNotBeNullOrEmpty(string.Format("Label with text '{0}' does not have a For attribute", labelText));
			var control = browserContext.TryGetElementById(itsLinkedControlId);
			var howFound = "Input with id '" + itsLinkedControlId + "' as referenced in For attribute of Label with text '" + labelText + "'";

			return GetInputElement(browserContext, control, howFound);
		}

		public static dynamic InputWithValue(this IBrowserContext browserContext, string value)
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

			if (textBoxesWithValue.Any())
			{
				var input = new TextBoxWrapper(textBoxesWithValue.First().Element, howFound, browserContext);
				return input;
			}
			if (dropDownsWithValue.Any())
			{
				var input = new DropDownListWrapper(dropDownsWithValue.First().Element, howFound, browserContext);
				return input;
			}

			var remoteWebElementWrapper = checkboxesAndRadiosWithValue.First();
			if (remoteWebElementWrapper.IsCheckBox())
			{
				return new CheckBoxWrapper(remoteWebElementWrapper, howFound, browserContext);
			}
			return new RadioOptionWrapper(remoteWebElementWrapper, howFound, browserContext);
		}
	}
}
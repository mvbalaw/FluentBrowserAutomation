using System;

using FluentAssert;

using JetBrains.Annotations;

namespace FluentBrowserAutomation.Accessors
{
	public abstract class TextBase
	{
		private readonly string _howFound;

		protected TextBase(string text, string howFound)
		{
			Text = text;
			_howFound = howFound;
		}

		internal string Text { get; set; }

		public IReadOnlyBooleanState Contains([NotNull] string containedText)
		{
			const string unexpectedlyTrue = "The text '{0}' in {1} should not contain '{2}'";
			const string unexpectedlyFalse = "The text '{0}' in {1} should contain '{2}'";
			var unexpectedlyFalseMessage = string.Format(unexpectedlyFalse, Text, _howFound, containedText);
			var unexpectedlyTrueMessage = string.Format(unexpectedlyTrue, Text, _howFound, containedText);
			var result = new BooleanState(unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() => Text != null && Text.Contains(containedText));
			return result;
		}

		public void ShouldBeEmpty()
		{
			var actual = Text ?? "";
			actual.Length.ShouldBeEqualTo(0);
		}

		public void ShouldBeEmpty(string errorMessage)
		{
			var actual = Text ?? "";
			actual.Length.ShouldBeEqualTo(0, errorMessage);
		}

		public void ShouldBeEqualTo([NotNull] string text)
		{
			const string unexpectedlyNotEqual = "The text '{0}' in {1} should be '{2}'";
			ShouldBeEqualTo(text, string.Format(unexpectedlyNotEqual, Text, _howFound, text));
		}

		public void ShouldBeEqualTo([NotNull] string text, string errorMessage)
		{
			var expected = text;
			var actual = Text ?? "";
			actual.ShouldBeEqualTo(expected, errorMessage);
		}

		private void ShouldBeEqualTo([NotNull] string text, Func<string> errorMessage)
		{
			var expected = text;
			var actual = Text ?? "";
			actual.ShouldBeEqualTo(expected, errorMessage);
		}

		public void ShouldNotBeEmpty()
		{
			var actual = Text ?? "";
			actual.Length.ShouldBeGreaterThan(0);
		}

		public void ShouldNotBeEmpty(string errorMessage)
		{
			var actual = Text ?? "";
			actual.Length.ShouldBeGreaterThan(0, errorMessage);
		}

		public void ShouldNotBeEqualTo([NotNull] string text)
		{
			const string unexpectedlyEqual = "The text '{0}' in {1} should not be '{2}'";
			ShouldNotBeEqualTo(text, string.Format(unexpectedlyEqual, Text, _howFound, text));
		}

		public void ShouldNotBeEqualTo([NotNull] string text, string errorMessage)
		{
			var expected = text;
			var actual = Text ?? "";
			actual.ShouldNotBeEqualTo(expected, errorMessage);
		}

		public IReadOnlyBooleanState StartsWith([NotNull] string startingText)
		{
			const string unexpectedlyTrue = "The text '{0}' in {1} should not start with '{2}'";
			const string unexpectedlyFalse = "The text '{0}' in {1} should start with '{2}'";
			var unexpectedlyTrueMessage = string.Format(unexpectedlyTrue, Text, _howFound, startingText);
			var unexpectedlyFalseMessage = string.Format(unexpectedlyFalse, Text, _howFound, startingText);
			var result = new BooleanState(unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() => Text != null && Text.StartsWith(startingText));
			return result;
		}
	}
}
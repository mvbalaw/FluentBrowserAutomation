using System;

using FluentAssert;

namespace FluentBrowserAutomation.Accessors
{
	public interface IReadOnlyBooleanState
	{
		bool IsFalse { get; }
		bool IsTrue { get; }
		void ShouldBeFalse(string errorMessage = null);
		void ShouldBeTrue(string errorMessage = null);
	}

	public class BooleanState : IReadOnlyBooleanState
	{
		public BooleanState(string unexpectedlyFalseMessage,
			string unexpectedlyTrueMessage, Func<bool> getState)
			: this(unexpectedlyFalseMessage, unexpectedlyTrueMessage, getState, null)
		{
		}

		public BooleanState(string unexpectedlyFalseMessage, string unexpectedlyTrueMessage, Func<bool> getState, Action<bool> setState)
		{
			_unexpectedlyFalseMessage = unexpectedlyFalseMessage;
			_unexpectedlyTrueMessage = unexpectedlyTrueMessage;
			_getState = getState;
			_setState = setState;
		}

		private readonly Func<bool> _getState;
		private readonly Action<bool> _setState;
		private readonly string _unexpectedlyFalseMessage;
		private readonly string _unexpectedlyTrueMessage;

		public void SetValueTo(bool state)
		{
			_setState(state);
		}

		public void ShouldBeFalse(string errorMessage = null)
		{
			IsFalse.ShouldBeTrue(errorMessage ?? _unexpectedlyTrueMessage);
		}

		public void ShouldBeTrue(string errorMessage = null)
		{
			IsTrue.ShouldBeTrue(errorMessage ?? _unexpectedlyFalseMessage);
		}

		public bool IsFalse
		{
			get { return !IsTrue; }
		}
		public bool IsTrue
		{
			get { return _getState(); }
		}
	}
}
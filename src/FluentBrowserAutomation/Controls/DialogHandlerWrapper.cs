using System;
using System.Linq;

namespace FluentBrowserAutomation.Controls
{
	public class DialogHandlerWrapper
	{
		private readonly Action _action;
		private readonly IBrowserContext _browserContext;

		public DialogHandlerWrapper(IBrowserContext browserContext, Action action)
		{
			_browserContext = browserContext;
			_action = action;
		}

		public void ClickCancel()
		{
			var parentHandle = _browserContext.Browser.CurrentWindowHandle;

			try
			{
				var handles = _browserContext.Browser.WindowHandles.Except(new[] { parentHandle });
				_browserContext.Browser.SwitchTo().Window(handles.Last());
				_action();
				_browserContext.ButtonWithText("Cancel").Click();
			}
			finally
			{
				_browserContext.Browser.SwitchTo().Window(parentHandle);
			}
		}

		public void ClickOk()
		{
			var parentHandle = _browserContext.Browser.CurrentWindowHandle;

			try
			{
				var handles = _browserContext.Browser.WindowHandles.Except(new[] { parentHandle });
				_browserContext.Browser.SwitchTo().Window(handles.Last());
				_action();
				_browserContext.ButtonWithText("OK").Click();
			}
			finally
			{
				_browserContext.Browser.SwitchTo().Window(parentHandle);
			}
		}
	}
}
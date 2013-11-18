using System;

// ReSharper disable once CheckNamespace
namespace FluentBrowserAutomation
{
	public interface IPageWrapper
	{
		IBrowserContext BrowserContext { get; }
	}

	public static class IPageWrapperExtensions
	{
		public static T UiState<T>(this T pageWrapper, params Action<IBrowserContext>[] funcs) where T : IPageWrapper
		{
			return UiState(pageWrapper, null, funcs);
		}

		public static T UiState<T>(this T pageWrapper, string activityDescription = null, params Action<IBrowserContext>[] funcs) where T : IPageWrapper
		{
			for (var index = 0; index < funcs.Length; index++)
			{
				var func = funcs[index];
				var func1 = func;
				pageWrapper.BrowserContext.WaitUntil(x =>
				{
					func1(pageWrapper.BrowserContext);
					return true;
				}, errorMessage: "UiState action " + (index + 1) + " of " + (activityDescription??funcs.Length.ToString()) + " failed.");
			}
			return pageWrapper;
		}
	}
}
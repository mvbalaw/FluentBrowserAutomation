using System;

//// ReSharper disable CheckNamespace
namespace FluentBrowserAutomation
//// ReSharper restore CheckNamespace
{
	public interface IPageWrapper
	{
		IBrowserContext BrowserContext { get; }
	}

	public static class IPageWrapperExtensions
	{
		public static T UiState<T>(this T pageWrapper, params Action<IBrowserContext>[] funcs) where T : IPageWrapper
		{
			foreach (var func in funcs)
			{
				func(pageWrapper.BrowserContext);
			}
			return pageWrapper;
		}
	}
}
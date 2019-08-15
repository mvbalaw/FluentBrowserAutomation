﻿using System;
using OpenQA.Selenium;

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
			pageWrapper.BrowserContext.Trace("-- Performing: unlabeled action");

	        return UiState(pageWrapper, null, funcs);
        }

        public static T UiState<T>(this T pageWrapper, string activityDescription = null,
            params Action<IBrowserContext>[] funcs) where T : IPageWrapper
        {
	        if (activityDescription != null)
	        {
		        pageWrapper.BrowserContext.Trace("-- Performing: " + activityDescription);
	        }

	        for (var index = 0; index < funcs.Length; index++)
            {
                var func = funcs[index];
                var func1 = func;
                pageWrapper.BrowserContext.WaitUntil(x =>
                {
                    func1(pageWrapper.BrowserContext);
                    return true;
                },
                    errorMessage:
                        "UiState action " + (index + 1) + " of " + (activityDescription ?? funcs.Length.ToString()));
            }
            return pageWrapper;
        }

        public static T SwitchToNewTabAndCloseCurrentTab<T>(this T pageWrapper, Func<IWebDriver, bool> getTabIdentifier) where T : IPageWrapper
        {
            var browser = pageWrapper.BrowserContext.Browser;
            browser.Close();
            foreach (var handle in browser.WindowHandles)
            {
                browser.SwitchTo().Window(handle);
                if (getTabIdentifier(browser))
                {
                    return pageWrapper;
                }
            }
            return pageWrapper;
        }
    }
}
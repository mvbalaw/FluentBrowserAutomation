using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FluentBrowserAutomation.Framework
{
	public interface IBrowserManager
	{
		string BaseUrl { get; }
		void Close(IWebDriver browser);
		void Close();
		void CloseAllOpenBrowsers();
		IWebDriver GetBrowser();
		IWebDriver GetBrowser<TBrowser>() where TBrowser : RemoteWebDriver, new();
		void Trace(string s);
	}

	public class BrowserManager<T> : IBrowserManager
		where T : RemoteWebDriver, new()
	{
//// ReSharper disable StaticFieldInGenericType
//// ReSharper disable InconsistentNaming
		private static readonly List<IWebDriver> _browsers = new List<IWebDriver>();
//// ReSharper restore InconsistentNaming
//// ReSharper restore StaticFieldInGenericType
		private readonly bool _uniqueBrowserEachTime;
		private readonly string _initialUrl;
		private readonly bool _writeTraceOutput;

		public BrowserManager(string initialUrl, bool uniqueBrowserEachTime = true, bool writeTraceOutput = false)
		{
			_initialUrl = initialUrl;
			if (initialUrl != null)
			{
				var uri = new Uri(initialUrl);
				BaseUrl = uri.AbsoluteUri;
			}
			_uniqueBrowserEachTime = uniqueBrowserEachTime;
			_writeTraceOutput = writeTraceOutput;
		}

		public string BaseUrl { get; private set; }

		public void Close()
		{
			var browser = AttachToExistingBrowser<T>();
			if (browser != null)
			{
				Close(browser);
			}
		}

		public void Close(IWebDriver browser)
		{
			if (browser != null)
			{
				_browsers.Remove(browser);
				var windowHandle = browser.CurrentWindowHandle;
				try
				{
					browser.Quit();
					browser.Dispose();
				}
				catch
				{
					if (windowHandle != null)
					{
						var process = Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == new IntPtr(Int32.Parse(windowHandle)));
						if (process != null)
						{
							process.Kill();
						}
					}
				}
			}
		}

		public void CloseAllOpenBrowsers()
		{
			foreach (var browser in _browsers)
			{
				Close(browser);
			}
		}

		public IWebDriver GetBrowser()
		{
			return GetBrowser<T>();
		}

		public IWebDriver GetBrowser<TBrowser>() where TBrowser : RemoteWebDriver, new()
		{
			var driver = AttachToExistingBrowser<TBrowser>();
			if (driver == null)
			{
				driver = new T();
				_browsers.Add(driver);
			}
			if (_initialUrl != null)
			{
				driver.Navigate().GoToUrl(_initialUrl);
			}
			return driver;
		}

		public void Trace(string s)
		{
			if (!_writeTraceOutput)
			{
				return;
			}
			Console.WriteLine(s);
		}

		private IWebDriver AttachToExistingBrowser<TBrowser>()
		{
			if (!_uniqueBrowserEachTime && _browsers.Exists(x => x.GetType() == typeof(TBrowser)))
			{
				var browser = _browsers.First(x => x.GetType() == typeof(TBrowser));
				return browser;
			}
			return null;
		}
	}
}
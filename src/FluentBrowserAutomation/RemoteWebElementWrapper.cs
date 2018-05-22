using System;
using System.Collections.ObjectModel;
using System.Drawing;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace FluentBrowserAutomation
{
	public class RemoteWebElementWrapper
	{
		public RemoteWebElementWrapper(Func<IWebElement> howToGetIt, IWebElement webElement, IWebDriver browser)
			: this(howToGetIt, browser)
		{
			_remoteElement = webElement;
		}

		public RemoteWebElementWrapper(Func<IWebElement> howToGetIt, IWebDriver browser)
		{
			_howToGetIt = howToGetIt;
			Browser = browser;
		}

		private readonly Func<IWebElement> _howToGetIt;
		private IWebElement _remoteElement;
		private string _tagName;

		public void Clear()
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.Clear();
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.Clear();
			}
		}

		public void Click()
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}

				// ReSharper disable once PossibleNullReferenceException
				_remoteElement.Click();
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
				if (_remoteElement != null)
				{
					var actions = new Actions(Browser);
					actions.Click(_remoteElement).Perform();
				}
				else
				{
					// ReSharper disable once PossibleNullReferenceException
					_remoteElement.Click();
				}
			}
		}

		public IWebElement FindElement(By by)
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.FindElement(by);
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.FindElement(by);
			}
		}

// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.FindElements(by);
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.FindElements(by);
			}
		}

		public string GetAttribute(string attributeName)
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.GetAttribute(attributeName);
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.GetAttribute(attributeName);
			}
		}

		public string GetCssValue(string propertyName)
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.GetCssValue(propertyName);
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				return _remoteElement.GetCssValue(propertyName);
			}
		}

		public void SendKeys(string text)
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.SendKeys(text);
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.SendKeys(text);
			}
		}

		public void Submit()
		{
			try
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.Submit();
			}
			catch (WebDriverException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.Submit();
			}
		}

		public void TryEnsureExists()
		{
			if (_remoteElement == null && _howToGetIt != null)
			{
				_remoteElement = _howToGetIt();
			}
		}

		public bool Displayed
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Displayed;
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Displayed;
				}
			}
		}
		public bool Enabled
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Enabled;
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Enabled;
				}
			}
		}
		public bool Exists
		{
			get
			{
				if (_remoteElement == null)
				{
					TryEnsureExists();
				}
				return _remoteElement != null;
			}
		}
		public Point Location
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Location;
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Location;
				}
			}
		}
		public IWebElement RemoteWebElement
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once UnusedVariable
					if (_remoteElement != null)
					{
						var x = _remoteElement.TagName;
					}
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
				}
				return _remoteElement;
			}
		}
		public bool Selected
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Selected;
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Selected;
				}
			}
		}
		public Size Size
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Size;
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Size;
				}
			}
		}
		public string TagName
		{
			get
			{
				if (_tagName == null)
				{
					try
					{
						if (_remoteElement == null)
						{
							TryEnsureExists();
						}
// ReSharper disable once PossibleNullReferenceException
						return _tagName = _remoteElement.TagName;
					}
					catch (WebDriverException)
					{
						_remoteElement = null;
						TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
						return _tagName = _remoteElement.TagName;
					}
				}
				return _tagName;
			}
		}
		public string Text
		{
			get
			{
				try
				{
					if (_remoteElement == null)
					{
						TryEnsureExists();
					}
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Text;
				}
				catch (WebDriverException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Text;
				}
			}
		}

		public IWebDriver Browser { get; private set; }
	}
}
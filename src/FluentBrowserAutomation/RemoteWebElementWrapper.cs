using System;
using System.Collections.ObjectModel;
using System.Drawing;

using OpenQA.Selenium;

namespace FluentBrowserAutomation
{
	public class RemoteWebElementWrapper
	{
		public RemoteWebElementWrapper(Func<IWebElement> howToGetIt, IWebElement webElement)
			: this(howToGetIt)
		{
			_remoteElement = webElement;
		}

		public RemoteWebElementWrapper(Func<IWebElement> howToGetIt)
		{
			_howToGetIt = howToGetIt;
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
			catch (StaleElementReferenceException)
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
			catch (StaleElementReferenceException)
			{
				_remoteElement = null;
				TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
				_remoteElement.Click();
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
			catch (StaleElementReferenceException)
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
			catch (StaleElementReferenceException)
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
			catch (StaleElementReferenceException)
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
			catch (StaleElementReferenceException)
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
			catch (StaleElementReferenceException)
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
			catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
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
					catch (StaleElementReferenceException)
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
				catch (StaleElementReferenceException)
				{
					_remoteElement = null;
					TryEnsureExists();
// ReSharper disable once PossibleNullReferenceException
					return _remoteElement.Text;
				}
			}
		}
	}
}
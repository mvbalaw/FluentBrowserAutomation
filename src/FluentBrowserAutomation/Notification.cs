namespace FluentBrowserAutomation
{
	public class Notification
	{
		public string BrowserType { get; set; }
		public string Message { get; set; }
		public bool Success { get; set; }

		public override string ToString()
		{
			return string.Format("{0}: {1}", BrowserType, Message);
		}
	}
}
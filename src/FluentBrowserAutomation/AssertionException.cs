using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FluentBrowserAutomation
{
	public class AssertionException : Exception
	{
		protected AssertionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal AssertionException(string errorMessage)
			: base(errorMessage)
		{
		}

		internal AssertionException(string errorMessage, Exception innerException)
			: base(errorMessage, innerException)
		{
		}

		internal AssertionException(Exception exception)
			: base("", exception)
		{
		}

		public override string StackTrace
		{
			get
			{
				var lines = base.StackTrace
					.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
					.Where(x => !x.Contains("FluentBrowserAutomation"))
					.ToArray();
				var result = string.Join(Environment.NewLine, lines);
				return result;
			}
		}
	}
}
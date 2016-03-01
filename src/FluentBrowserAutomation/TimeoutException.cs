using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FluentBrowserAutomation
{
	public class TimeoutException : Exception
	{
		protected TimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal TimeoutException(string errorMessage)
			: base(errorMessage)
		{
		}

		internal TimeoutException(string errorMessage, Exception innerException)
			: base(errorMessage, innerException)
		{
		}

		internal TimeoutException(Exception exception)
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
				var result = String.Join(Environment.NewLine, lines);
				return result;
			}
		}
	}
}
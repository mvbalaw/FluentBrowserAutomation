using System;

using FluentBrowserAutomation.Controls;

namespace FluentBrowserAutomation.Extensions
{
	public static class ControlWrapperExtensions
	{
		public static void Verify<T>(this T control, params Action<T>[] actions) where T : BasicInfoWrapper
		{
			foreach (var action in actions)
			{
				action(control);
			}
		}
	}
}
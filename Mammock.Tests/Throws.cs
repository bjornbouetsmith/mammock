using System;
using Xunit;

namespace Mammock.Tests
{

	public class Throws
	{
		public static void Exception<TException>(Mammock.Delegates.Action action)
			where TException  : Exception
		{
			try
			{
				action();
				Assert.False(true, "Should have thrown exception");
			}
			catch(TException)
			{
			}
		}

        public static void Exception<TException>(string message, Mammock.Delegates.Action action)
			where TException : Exception
		{
			try
			{
				action();
				Assert.False(true, "Should have thrown exception");
			}
			catch (TException e)
			{
				Assert.Equal(message, e.Message);
			}
		}
	}
}
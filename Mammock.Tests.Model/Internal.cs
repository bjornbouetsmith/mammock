namespace Mammock.Tests.Model
{
	internal class Internal
	{
		internal virtual string Bar()
		{
			return "abc";
		}

		internal virtual string Foo()
		{
			return Bar();
		}
	}
}
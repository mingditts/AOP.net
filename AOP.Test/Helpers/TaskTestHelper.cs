using System;
using System.Threading.Tasks;

namespace AOP.Test.Helpers
{
	public class TaskTestHelper
	{
		/// <summary>
		/// Handle task assertion
		/// </summary>
		/// <param name="task"></param>
		public static void StartAndHandleAssertion(Task task)
		{
			task.Start();
			task.Wait();
		}

		/// <summary>
		/// Handle task assertion
		/// </summary>
		/// <param name="action"></param>
		public static void StartAndHandleAssertion(Action action)
		{
			var task = new Task(action);
			task.Start();
			task.Wait();
		}
	}
}
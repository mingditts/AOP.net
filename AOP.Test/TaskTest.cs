using AOP.Core;
using AOP.Test.Helpers;
using AOP.Test.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AOP.Test
{
	public class TaskTest
	{
		private class MyAspect<T> : Aspect<T> where T : class
		{
			public static volatile int Counter = 0;

			protected override void OnAfter(ExecutionContext context, object result)
			{
				Counter++;
			}

			protected override AroundExecutionResult OnAround(ExecutionContext context)
			{
				Counter++;
				return AroundExecutionResult.BuildForProceed();
			}

			protected override void OnBefore(ExecutionContext context)
			{
				Counter++;
			}

			protected override void OnThrow(ExecutionContext context, Exception exception)
			{

			}
		}

		[Fact]
		public void BasicTaskTest()
		{
			IService service = Aspect<IService>.BuildForAll(
				new Service(),
				new MyAspect<IService>()
			);

			Task<bool> task = service.DoWorkAsync();

			task.Wait();

			Assert.True(task.Result);

			//TaskTestHelper.StartAndHandleAssertion(() =>
			//{
			//	Task.Delay(3000);
			//	Assert.Equal(3, MyAspect<IService>.Counter);
			//});
		}
	}
}
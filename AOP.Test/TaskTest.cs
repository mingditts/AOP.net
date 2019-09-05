using AOP.Core;
using AOP.Test.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AOP.Test
{
	public class TaskTest
	{
		private class MyAspect<T> : Aspect<T>, IBeforeAdvice, IAroundAdvice, IAfterAdvice, IAfterThrowAdvice where T : class
		{
			public static volatile int Counter = 0;

			public void OnAfter(ExecutionContext context, object result)
			{
				Counter++;
			}

			public AroundExecutionResult OnAround(ExecutionContext context)
			{
				Counter++;
				return AroundExecutionResult.BuildForProceed();
			}

			public void OnBefore(ExecutionContext context)
			{
				Counter++;
			}

			public void OnThrow(ExecutionContext context, Exception exception)
			{

			}
		}

		[Fact]
		public void BasicTaskTest()
		{
			IService service = Aspect<IService>.Build(
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
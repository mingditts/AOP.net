using AOP.Core;
using AOP.Test.Mocks;
using System;
using Xunit;

namespace AOP.Test
{
	public class BasicTest
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
				return null;
			}

			protected override void OnBefore(ExecutionContext context)
			{
				Counter++;
			}

			protected override void OnThrow(ExecutionContext context, Exception exception)
			{
				Counter++;      //no exception should be thrown
			}
		}

		[Fact]
		public void BasicCreationTest()
		{
			IService service = Aspect<IService>.BuildForAll(
				new Service(),
				new MyAspect<IService>(),
				new MyAspect<IService>()
			);

			service.DoWork();

			Assert.Equal(6, MyAspect<IService>.Counter);
		}
	}
}
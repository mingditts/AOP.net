using AOP.Core;
using AOP.Test.Mocks;
using System;
using Xunit;

namespace AOP.Test
{
	public class BasicTest
	{
		private class MyAspect<T> : Aspect<T>, IBeforeAdvice, IAroundAdvice, IAfterAdvice, IAfterThrowAdvice where T : class
		{
			public bool OnBeforeReached = false;
			public bool OnAroundReached = false;
			public bool OnAfterReached = false;
			public bool OnThrowReached = false;

			private bool _proceed = true;
			private object _overwrittenResult = null;

			public MyAspect()
			{

			}

			public MyAspect(bool proceed, object overwrittenResult)
			{
				this._proceed = proceed;
				this._overwrittenResult = overwrittenResult;
			}

			public void OnBefore(ExecutionContext context)
			{
				this.OnBeforeReached = true;
			}

			public AroundExecutionResult OnAround(ExecutionContext context)
			{
				this.OnAroundReached = true;

				return new AroundExecutionResult
				{
					Proceed = this._proceed,
					OverwrittenResult = this._overwrittenResult
				};
			}

			public void OnAfter(ExecutionContext context, object result)
			{
				this.OnAfterReached = true;
			}

			public void OnThrow(ExecutionContext context, Exception exception)
			{
				this.OnThrowReached = true;
			}
		}

		[Fact]
		public void BasicCreationTest()
		{
			var aspect0 = new MyAspect<IService>();
			var aspect1 = new MyAspect<IService>();

			IService service = Aspect<IService>.Build(
				new Service(),
				aspect0,
				aspect1
			);

			service.DoWork();

			Assert.True(aspect0.OnBeforeReached);
			Assert.True(aspect0.OnAroundReached);
			Assert.True(aspect0.OnAfterReached);
			Assert.False(aspect0.OnThrowReached);

			Assert.True(aspect1.OnBeforeReached);
			Assert.True(aspect1.OnAroundReached);
			Assert.True(aspect1.OnAfterReached);
			Assert.False(aspect1.OnThrowReached);
		}

		[Fact]
		public void ThrowTest()
		{
			var aspect0 = new MyAspect<IService>();

			IService service = Aspect<IService>.Build(
				new Service(),
				aspect0
			);

			bool externalException = false;

			try
			{
				service.ThrowException();
			}
			catch
			{
				externalException = true;
			}

			Assert.True(aspect0.OnBeforeReached);
			Assert.True(aspect0.OnAroundReached);
			Assert.False(aspect0.OnAfterReached);
			Assert.True(aspect0.OnThrowReached);

			Assert.True(externalException);
		}

		[Fact]
		public void AroundTest()
		{
			var aspect0 = new MyAspect<IService>(false, false);

			IService service = Aspect<IService>.Build(
				new Service(),
				aspect0
			);

			bool result = service.DoWork();

			Assert.True(aspect0.OnBeforeReached);
			Assert.True(aspect0.OnAroundReached);
			Assert.False(aspect0.OnAfterReached);
			Assert.False(aspect0.OnThrowReached);

			Assert.False(result);
		}

		[Fact]
		public void BasicPropertyTest()
		{
			var aspect0 = new MyAspect<IService>();

			IService service = Aspect<IService>.Build(
				new Service(),
				aspect0
			);

			service.Property1 = true;

			Assert.True(aspect0.OnBeforeReached);
			Assert.True(aspect0.OnAroundReached);
			Assert.True(aspect0.OnAfterReached);
			Assert.False(aspect0.OnThrowReached);
		}
	}
}
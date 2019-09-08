using AOP.Core;
using AOP.Test.Mocks;
using System;
using Xunit;

namespace AOP.Test
{
	public class ExpressionTest
	{
		private class MyAspect<T> : Aspect<T>, IBeforeAdvice, IAroundAdvice, IAfterAdvice, IAfterThrowAdvice where T : class
		{
			public int ExecutionCounter = 0;
			public int ThrowCounter = 0;

			public MyAspect()
			{

			}

			public MyAspect(string expression) : base(expression)
			{

			}

			public MyAspect(bool proceed, object overwrittenResult)
			{

			}

			public void OnBefore(ExecutionContext context)
			{
				this.ExecutionCounter++;
			}

			public AroundExecutionResult OnAround(ExecutionContext context)
			{
				this.ExecutionCounter++;
				return new AroundExecutionResult
				{
					Proceed = true
				};
			}

			public void OnAfter(ExecutionContext context, object result)
			{
				this.ExecutionCounter++;
			}

			public void OnThrow(ExecutionContext context, Exception exception)
			{
				this.ThrowCounter++;
			}
		}

		[Fact]
		public void NoExpressionTest()
		{
			var expressions = new string[] {
				null,
				"",
				"*",
				"*:*",
				"methods|properties",
				"methods|properties:*",
			};

			foreach (var expression in expressions)
			{
				var aspect = new MyAspect<IService>(expression);

				IService service = aspect.Build(new Service());

				service.DoWork();
				service.Property1 = true;
				try { service.ThrowException(); } catch { }

				Assert.Equal(3 * 3 - 1, aspect.ExecutionCounter);       //3 * is for the method (2 times) and property (1 time) executions, -1 for the throw that prevents the after execution
				Assert.Equal(1, aspect.ThrowCounter);
			}
		}

		[Fact]
		public void ExpressionsMethodTest()
		{
			var expressions = new string[] {
				"methods:",
				"methods:*",
				"methods:",
				"methods:Do*"
			};

			foreach (var expression in expressions)
			{
				var aspect = new MyAspect<IService>(expression);

				IService service = aspect.Build(new Service());

				service.DoWork();
				service.DoAnotherWork();
				service.Property1 = true;
				try { service.ThrowException(); } catch { }

				if ("methods:Do*".Equals(expression))
				{
					Assert.Equal(2 * 3, aspect.ExecutionCounter);           //2 * is for the Do* method (2 times) executions
					Assert.Equal(0, aspect.ThrowCounter);
				}
				else
				{
					Assert.Equal(3 * 3 - 1, aspect.ExecutionCounter);       //3 * is for the method (2 times) executions, -1 for the throw that prevents the after execution
					Assert.Equal(1, aspect.ThrowCounter);
				}
			}
		}

		[Fact]
		public void ExpressionsPropertiesTest()
		{
			var expressions = new string[] {
				"properties:",
				"properties:*",
				"properties:",
				"properties:Property*"
			};

			foreach (var expression in expressions)
			{
				var aspect = new MyAspect<IService>(expression);

				IService service = aspect.Build(new Service());

				service.DoWork();
				service.DoAnotherWork();
				service.Property1 = true;
				service.Property2 = true;
				try { service.ThrowException(); } catch { }

				Assert.Equal(2 * 3, aspect.ExecutionCounter);       //2 * is for the properties (3 times) executions
				Assert.Equal(0, aspect.ThrowCounter);
			}
		}
	}
}
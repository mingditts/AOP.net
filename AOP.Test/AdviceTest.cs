using AOP.Core;
using AOP.Test.Helpers;
using AOP.Test.Mocks;
using System;
using Xunit;

namespace AOP.Test
{
	public class AdviceTest
	{
		[Fact]
		public void BasicTests()
		{
			var systemHelper = new SystemHelper();

			long tbefore = 0, taround = 0, tafter = 0;

			bool exceptionRaised = false;

			IService service = Advice<IService>.Build(
				new Service(),
				null,
				(AdviceExecutionContext context) =>
				{
					tbefore = systemHelper.GetTicks();
				},
				(AdviceExecutionContext context) =>
				{
					taround = systemHelper.GetTicks();
					return AroundExecutionResult.BuildForProceed();
				},
				(AdviceExecutionContext context, object result) =>
				{
					tafter = systemHelper.GetTicks();
				},
				(AdviceExecutionContext context, Exception exception) =>
				{
					exceptionRaised = true;
				}
			);

			long tinitial = systemHelper.GetTicks();

			service.DoWork();

			long tfinal = systemHelper.GetTicks();

			Assert.True(tinitial < tfinal);

			Assert.True(tbefore > tinitial && tbefore < taround);
			Assert.True(taround > tbefore && taround < tafter);
			Assert.True(tafter < tfinal);

			Assert.True(exceptionRaised == false);
		}

		[Fact]
		public void AroundTest()
		{
			bool methodCalled = false;

			bool berforeCalled = false;
			bool aroundCalled = false;
			bool afterCalled = false;
			bool throwCalled = false;

			IService service = Advice<IService>.Build(
				new Service(),
				null,
				(AdviceExecutionContext context) =>
				{
					berforeCalled = true;
				},
				(AdviceExecutionContext context) =>
				{
					aroundCalled = true;
					return AroundExecutionResult.BuildForOverwrite(true);
				},
				(AdviceExecutionContext context, object result) =>
				{
					afterCalled = true;
				},
				(AdviceExecutionContext context, Exception exception) =>
				{
					throwCalled = true;
				}
			);

			methodCalled = service.DoWork();

			Assert.True(berforeCalled == true);
			Assert.True(aroundCalled == true);
			Assert.True(afterCalled == false);
			Assert.True(throwCalled == false);

			Assert.True(methodCalled == true);      //read method call test
		}

		[Fact]
		public void BuildChainTest()
		{
			bool advice1BeforeCalled = false;
			bool advice2BeforeCalled = false;

			IService service = Advice<IService>.Build(
				new Service(),
				null,
				(AdviceExecutionContext context) =>
				{
					advice1BeforeCalled = true;
				},
				null,
				null,
				null
			);

			service = Advice<IService>.Build(
				service,        //Important: reuse the proxy
				null,
				(AdviceExecutionContext context) =>
				{
					advice2BeforeCalled = true;
				},
				null,
				null,
				null
			);

			service.DoWork();

			Assert.True(advice1BeforeCalled == true);
			Assert.True(advice2BeforeCalled == true);
		}
	}
}
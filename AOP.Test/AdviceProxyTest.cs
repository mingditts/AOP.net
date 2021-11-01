using AOP.Core;
using Xunit;

namespace AOP.Test
{
	public class AdviceProxyTest
	{
		[Theory]
		[InlineData(null, true, true, "*")]
		[InlineData("", true, true, "*")]
		[InlineData("*", true, true, "*")]
		[InlineData("*:*", true, true, "*")]
		[InlineData("methods", true, false, "*")]
		[InlineData("properties", false, true, "*")]
		[InlineData("methods|properties", true, true, "*")]
		[InlineData("properties|methods", true, true, "*")]
		[InlineData("*:Get*", true, true, "Get*")]
		[InlineData("methods:Get*", true, false, "Get*")]
		[InlineData("properties:Get*", false, true, "Get*")]
		public void ExtractExecutionFilter_AllCases(string info, bool expectedIncludeMethods, bool expectedIncludeProperties, string expectedPattern)
		{
			//ACT

			var proxy = new AdviceProxy<object>();

			var filter = proxy.ExtractExecutionFilter(info);

			//ASSERTS

			Assert.Equal(filter.IncludeMethods, expectedIncludeMethods);
			Assert.Equal(filter.IncludeProperties, expectedIncludeProperties);
			Assert.Equal(filter.Pattern, expectedPattern);
		}
	}
}

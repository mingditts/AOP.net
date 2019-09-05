using System;

namespace AOP.Core
{
	public interface IAfterThrowAdvice
	{
		/// <summary>
		/// On throw
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		void OnThrow(ExecutionContext context, Exception exception);
	}
}
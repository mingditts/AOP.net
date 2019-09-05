namespace AOP.Core
{
	public interface IAfterAdvice
	{
		/// <summary>
		/// On after
		/// </summary>
		/// <param name="context"></param>
		/// <param name="result"></param>
		void OnAfter(ExecutionContext context, object result);
	}
}
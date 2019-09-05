namespace AOP.Core
{
	public interface IAroundAdvice
	{
		/// <summary>
		/// On around
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		AroundExecutionResult OnAround(ExecutionContext context);
	}
}
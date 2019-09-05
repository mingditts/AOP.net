namespace AOP.Core
{
	public interface IBeforeAdvice
	{
		/// <summary>
		/// On before
		/// </summary>
		/// <param name="context"></param>
		void OnBefore(ExecutionContext context);
	}
}
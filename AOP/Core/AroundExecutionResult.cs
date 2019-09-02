namespace AOP.Core
{
	public class AroundExecutionResult
	{
		/// <summary>
		/// Proceed with the standard execution
		/// </summary>
		public bool Proceed { get; set; }

		/// <summary>
		/// Overwritten result returned instead of the normal execution
		/// </summary>
		public object OverwrittenResult { get; set; }

		public AroundExecutionResult()
		{

		}

		public AroundExecutionResult(bool proceed, object overwrittenResult)
		{
			this.Proceed = proceed;
			this.OverwrittenResult = overwrittenResult;
		}

		/// <summary>
		/// Build for proceed
		/// </summary>
		/// <returns></returns>
		public static AroundExecutionResult BuildForProceed()
		{
			return new AroundExecutionResult { Proceed = true, OverwrittenResult = null };
		}

		/// <summary>
		/// Build for overwrite
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public static AroundExecutionResult BuildForOverwrite(object result)
		{
			return new AroundExecutionResult { Proceed = false, OverwrittenResult = result };
		}
	}
}

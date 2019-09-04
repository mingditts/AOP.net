namespace AOP.Core
{
	public class ExecutionContext
	{
		/// <summary>
		/// Execution unique uuid
		/// </summary>
		public string Uid { get; set; }

		/// <summary>
		/// Execution call timestamp
		/// </summary>
		public long StartTimestamp { get; set; }

		/// <summary>
		/// Execution return timestamp
		/// </summary>
		public long EndTimestamp { get; set; }

		/// <summary>
		/// Execution arguments
		/// </summary>
		public object[] Args { get; set; }
	}
}

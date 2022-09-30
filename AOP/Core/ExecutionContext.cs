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

		public bool IsPublic { get; set; }
		public bool IsAbstract { get; set; }
		public bool IsPrivate { get; set; }
		public bool IsVirtual { get; set; }

		/// <summary>
		/// Member name of the execution
		/// </summary>
		public string MemberName { get; set; }
	}
}
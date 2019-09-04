namespace AOP.Test.Helpers
{
	public class SystemTestHelper
	{
		private readonly object _sync = new object();

		private volatile int _ticks = 0;

		/// <summary>
		/// Get ticks count
		/// </summary>
		public int GetTicks()
		{
			lock (this._sync)
			{
				return this._ticks++;
			}
		}
	}
}
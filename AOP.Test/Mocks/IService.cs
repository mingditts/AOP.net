using System.Threading.Tasks;

namespace AOP.Test.Mocks
{
	public interface IService
	{
		/// <summary>
		/// A property 1
		/// </summary>
		bool Property1 { get; set; }

		/// <summary>
		/// A property 2
		/// </summary>
		bool Property2 { get; set; }

		/// <summary>
		/// Working method
		/// </summary>
		/// <returns></returns>
		bool DoWork();

		/// <summary>
		/// Another working method
		/// </summary>
		/// <returns></returns>
		bool DoAnotherWork();

		/// <summary>
		/// Do work async
		/// </summary>
		/// <returns></returns>
		Task<bool> DoWorkAsync();

		/// <summary>
		/// Throw exception
		/// </summary>
		void ThrowException();
	}
}
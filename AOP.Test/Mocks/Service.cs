using System;
using System.Threading.Tasks;

namespace AOP.Test.Mocks
{
	public class Service : IService
	{
		public bool Property1 { get; set; }

		public bool Property2 { get; set; }

		public Service()
		{

		}

		public bool DoWork()
		{
			return true;
		}

		public bool DoAnotherWork()
		{
			return true;
		}

		public Task<bool> DoWorkAsync()
		{
			return Task.FromResult(true);
		}

		public void ThrowException()
		{
			throw new Exception("service exception");
		}
	}
}
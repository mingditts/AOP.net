using System.Threading.Tasks;

namespace AOP.Test.Mocks
{
	public class Service : IService
	{
		public Service()
		{

		}

		public bool DoWork()
		{
			return true;
		}

        public Task<bool> DoWorkAsync()
        {
            return Task.FromResult(true);
        }
    }
}
using System.Threading.Tasks;

namespace AOP.Test.Mocks
{
    public interface IService
    {
        /// <summary>
        /// Working method
        /// </summary>
        /// <returns></returns>
        bool DoWork();

        /// <summary>
        /// Do work async
        /// </summary>
        /// <returns></returns>
        Task<bool> DoWorkAsync();
    }
}
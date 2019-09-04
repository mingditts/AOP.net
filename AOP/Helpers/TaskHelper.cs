using System.Linq;
using System.Threading.Tasks;

namespace AOP.Helpers
{
	public class TaskHelper
	{
		/// <summary>
		/// Get task result
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		public static object GetTaskResult(Task task)
		{
			if (task.GetType().IsGenericType && task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
			{
				var property = task.GetType().GetProperties().FirstOrDefault(p => p.Name == "Result");

				if (property != null)
				{
					return property.GetValue(task);
				}
			}

			return null;
		}
	}
}

﻿using AOP.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOP.Core
{
	public class Advice<T> : DispatchProxy where T : class
	{
		#region Private Classes

		private class ExecutionFilter
		{
			public bool IncludeMethods { get; set; }

			public bool IncludeProperties { get; set; }

			public string Pattern { get; set; }
		}

		#endregion

		private T _target;
		private string _expression;
		private Action<ExecutionContext> _before;
		private Func<ExecutionContext, AroundExecutionResult> _around;
		private Action<ExecutionContext, object> _after;
		private Action<ExecutionContext, Exception> _throw;

		public Advice()
		{

		}

		/// <summary>
		/// Advice
		/// </summary>
		/// <param name="target"></param>
		/// <param name="expression">
		/// The expression filters the execution on a subset of joinpoints.
		/// Null means on all methods and properties execution.
		/// If it's not null the pattern is 'properties|method:methodNameRegExp'.
		/// Example: expression = "methods:Build*" means all the method only that starts with Build.
		/// <param name="before"></param>
		/// <param name="around"></param>
		/// <param name="after"></param>
		/// <param name="throwm"></param>
		public Advice(T target,
			string expression,
			Action<ExecutionContext> before,
			Func<ExecutionContext, AroundExecutionResult> around,
			Action<ExecutionContext, object> after,
			Action<ExecutionContext, Exception> throwm)
		{
			this.SetTarget(target, expression, before, around, after, throwm);
		}

		/// <summary>
		/// Set target
		/// </summary>
		/// <param name="target"></param>
		/// <param name="expression"></param>
		/// <param name="before"></param>
		/// <param name="around"></param>
		/// <param name="after"></param>
		/// <param name="throwm"></param>
		private void SetTarget(T target,
			string expression,
			Action<ExecutionContext> before,
			Func<ExecutionContext, AroundExecutionResult> around,
			Action<ExecutionContext, object> after,
			Action<ExecutionContext, Exception> throwm)
		{
			this._target = target;
			this._expression = expression;
			this._before = before;
			this._around = around;
			this._after = after;
			this._throw = throwm;
		}

		/// <summary>
		/// Build aspect
		/// </summary>
		/// <param name="target"></param>
		/// <param name="expression"></param>
		/// <param name="before"></param>
		/// <param name="around"></param>
		/// <param name="after"></param>
		/// <param name="throwm"></param>
		/// <returns></returns>
		public static T Build(T target,
			string expression,
			Action<ExecutionContext> before,
			Func<ExecutionContext, AroundExecutionResult> around,
			Action<ExecutionContext, object> after,
			Action<ExecutionContext, Exception> throwm)
		{
			object proxy = Create<T, Advice<T>>();

			((Advice<T>)proxy).SetTarget(target, expression, before, around, after, throwm);

			return (T)proxy;
		}

		/// <summary>
		/// Extract execution filter
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		private ExecutionFilter ExtractExecutionFilter(string info)
		{
			if (string.IsNullOrEmpty(info))
			{
				return new ExecutionFilter { IncludeMethods = true, IncludeProperties = true, Pattern = "*" };
			}

			bool includeMethods = false;
			bool includeProperties = false;
			string pattern = "*";

			string[] parts = info.Split(':');

			if (parts == null || parts.Length <= 0)
			{
				return new ExecutionFilter { IncludeMethods = true, IncludeProperties = true, Pattern = "*" };
			}

			if (parts.Length == 0)
			{
				includeMethods = true;
				includeProperties = true;

				pattern = "*";
			}
			else if (parts.Length == 1)
			{
				includeMethods = true;
				includeProperties = true;

				pattern = parts[0];
			}
			else if (parts.Length == 2)
			{
				string[] typeParts = parts[0].Split('|');

				includeMethods = typeParts != null && typeParts.Contains("methods");
				includeProperties = typeParts != null && typeParts.Contains("properties");

				pattern = parts[1];
			}

			return new ExecutionFilter { IncludeMethods = includeMethods, IncludeProperties = includeProperties, Pattern = pattern };
		}

		/// <summary>
		/// Evaluate execution
		/// </summary>
		/// <param name="targetMethod"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		private bool EvaluateExecution(MethodInfo targetMethod, ExecutionFilter filter)
		{
			if ("*".Equals(filter.Pattern) || Regex.IsMatch(targetMethod.Name, filter.Pattern))
			{
				bool isProperty = (targetMethod.Name.StartsWith("get_") || targetMethod.Name.StartsWith("set_")) && targetMethod.IsSpecialName;
				bool isMethod = !isProperty;

				return (isProperty && filter.IncludeProperties) || (isMethod && filter.IncludeMethods);
			}

			return false;
		}

		/// <summary>
		/// Invoke
		/// </summary>
		/// <param name="targetMethod"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		protected override object Invoke(MethodInfo targetMethod, object[] args)
		{
			#region Expression filter check

			if (this.EvaluateExecution(targetMethod, this.ExtractExecutionFilter(this._expression)) != true)
			{
				var result = typeof(T).InvokeMember(
					targetMethod.Name,
					BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
					null,
					this._target,
					args
				);

				if (result is Task)
				{
					((Task)result).ContinueWith(task =>
					{
						if (task.Exception != null)
						{
							throw task.Exception;
						}
						else
						{
							return TaskHelper.GetTaskResult(task);
						}
					});
				}
				else
				{
					return result;
				}
			}

			#endregion

			var executionContext = new ExecutionContext();

			try
			{
				executionContext.Args = args;
				executionContext.Uid = Guid.NewGuid().ToString();
				executionContext.StartTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

				#region Before

				if (this._before != null)
				{
					try
					{
						this._before.Invoke(executionContext);
					}
					catch
					{

					}
				}

				#endregion

				#region Around

				if (this._around != null)
				{
					AroundExecutionResult aroundResult = this._around.Invoke(executionContext);

					if (aroundResult != null && aroundResult.Proceed != true)
					{
						return aroundResult.OverwrittenResult;
					}
				}

				#endregion

				var result = typeof(T).InvokeMember(
					targetMethod.Name,
					BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
					null,
					this._target,
					args
				);

				if (result is Task)
				{
					((Task)result).ContinueWith(task =>
					{
						if (task.Exception != null)
						{
							this._throw.Invoke(executionContext, task.Exception);
						}
						else
						{
							object taskResult = TaskHelper.GetTaskResult(task);

							executionContext.EndTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

							#region After

							if (this._after != null)
							{
								try
								{
									this._after.Invoke(executionContext, taskResult);
								}
								catch
								{

								}
							}

							#endregion
						}
					});
				}
				else
				{
					executionContext.EndTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

					#region After

					if (this._after != null)
					{
						try
						{
							this._after.Invoke(executionContext, result);
						}
						catch
						{

						}
					}

					#endregion
				}

				return result;
			}
			catch (Exception ex)
			{
				if (ex is TargetInvocationException)
				{
					this._throw.Invoke(executionContext, ex.InnerException ?? ex);
				}

				return Activator.CreateInstance(targetMethod.ReturnType);       //check this
			}
		}
	}
}
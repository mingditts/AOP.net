using System;
using System.Collections.Generic;

namespace AOP.Core
{
	public abstract class Aspect<T> where T : class
	{
		private string _expression;

		public Aspect()
		{

		}

		/// <summary>
		/// Aspect
		/// </summary>
		/// <param name="expression">
		/// The expression filters the execution on a subset of joinpoints.
		/// Null means on all methods and properties execution.
		/// If it's not null the pattern is 'properties|method:methodNameRegExp'.
		/// Example: expression = "methods:Build*" means all the method only that starts with Build.
		/// </param>
		public Aspect(string expression)
		{
			this._expression = expression;
		}

		/// <summary>
		/// Build
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public T Build(T target)
		{
			return AdviceProxy<T>.Build(target,
				this._expression,
				this is IBeforeAdvice ? ((IBeforeAdvice)this).OnBefore : (Action<ExecutionContext>)null,
				this is IAroundAdvice ? ((IAroundAdvice)this).OnAround : (Func<ExecutionContext, AroundExecutionResult>)null,
				this is IAfterAdvice ? ((IAfterAdvice)this).OnAfter : (Action<ExecutionContext, object>)null,
				this is IAfterThrowAdvice ? ((IAfterThrowAdvice)this).OnThrow : (Action<ExecutionContext, Exception>)null
			);
		}

		/// <summary>
		/// Build
		/// </summary>
		/// <param name="target"></param>
		/// <param name="aspects"></param>
		/// <returns></returns>
		public static T Build(T target, IEnumerable<Aspect<T>> aspects)
		{
			T reference = target;

			foreach (Aspect<T> a in aspects)
			{
				reference = a.Build(reference);
			}

			return reference;
		}

		/// <summary>
		/// Build
		/// </summary>
		/// <param name="target"></param>
		/// <param name="aspects"></param>
		/// <returns></returns>
		public static T Build(T target, params Aspect<T>[] aspects)
		{
			T reference = target;

			foreach (Aspect<T> a in aspects)
			{
				reference = a.Build(reference);
			}

			return reference;
		}
	}
}
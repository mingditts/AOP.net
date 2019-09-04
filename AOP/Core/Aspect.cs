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
		/// On before
		/// </summary>
		/// <param name="context"></param>
		protected abstract void OnBefore(ExecutionContext context);

		/// <summary>
		/// On around
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected abstract AroundExecutionResult OnAround(ExecutionContext context);

		/// <summary>
		/// On after
		/// </summary>
		/// <param name="context"></param>
		/// <param name="result"></param>
		protected abstract void OnAfter(ExecutionContext context, object result);

		/// <summary>
		/// On throw
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		protected abstract void OnThrow(ExecutionContext context, Exception exception);

		/// <summary>
		/// Build for all
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public T BuildForAll(T target)
		{
			return Advice<T>.Build(target, this._expression, this.OnBefore, this.OnAround, this.OnAfter, this.OnThrow);
		}

		public T Build(T target, bool before = true, bool around = true, bool after = true, bool thrown = true)
		{
			return Advice<T>.Build(target,
				this._expression,
				before == true ? this.OnBefore : (Action<ExecutionContext>)null,
				around == true ? this.OnAround : (Func<ExecutionContext, AroundExecutionResult>)null,
				after == true ? this.OnAfter : (Action<ExecutionContext, object>)null,
				thrown == true ? this.OnThrow : (Action<ExecutionContext, Exception>)null
			);
		}

		/// <summary>
		/// Build for all
		/// </summary>
		/// <param name="target"></param>
		/// <param name="aspects"></param>
		/// <returns></returns>
		public static T BuildForAll(T target, IEnumerable<Aspect<T>> aspects)
		{
			T reference = target;

			foreach (Aspect<T> a in aspects)
			{
				reference = a.BuildForAll(reference);
			}

			return reference;
		}

		/// <summary>
		/// Build for all
		/// </summary>
		/// <param name="target"></param>
		/// <param name="aspects"></param>
		/// <returns></returns>
		public static T BuildForAll(T target, params Aspect<T>[] aspects)
		{
			T reference = target;

			foreach (Aspect<T> a in aspects)
			{
				reference = a.BuildForAll(reference);
			}

			return reference;
		}
	}
}
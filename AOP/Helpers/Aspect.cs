using AOP.Core;
using System;
using System.Collections.Generic;

namespace AOP.Helpers
{
	public abstract class Aspect<T> where T : class
	{
		private string _expression;

		public Aspect()
		{

		}

		public Aspect(string expression)
		{
			this._expression = expression;
		}

		/// <summary>
		/// On before
		/// </summary>
		/// <param name="context"></param>
		protected abstract void OnBefore(AdviceExecutionContext context);

		/// <summary>
		/// On around
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected abstract AroundExecutionResult OnAround(AdviceExecutionContext context);

		/// <summary>
		/// On after
		/// </summary>
		/// <param name="context"></param>
		/// <param name="result"></param>
		protected abstract void OnAfter(AdviceExecutionContext context, object result);

		/// <summary>
		/// On throw
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		protected abstract void OnThrow(AdviceExecutionContext context, Exception exception);

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
				before == true ? this.OnBefore : (Action<AdviceExecutionContext>)null,
				around == true ? this.OnAround : (Func<AdviceExecutionContext, AroundExecutionResult>)null,
				after == true ? this.OnAfter : (Action<AdviceExecutionContext, object>)null,
				thrown == true ? this.OnThrow : (Action<AdviceExecutionContext, Exception>)null
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
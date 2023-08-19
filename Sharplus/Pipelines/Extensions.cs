using System;

namespace Sharplus.Pipelines
{
    public static class Extensions
    {
        /// <summary>
        /// Execute the <paramref name="action"/> with <paramref name="param"/> as input.
        /// </summary>
        /// <param name="param">The input.</param>
        /// <param name="action">The action to execute.</param>
        public static void Pipe<T>(this T param, Action<T> action)
        {
            action(param);
        }

        /// <summary>
        /// Execute the <paramref name="func"/> with <paramref name="param"/> as input and return the result.
        /// </summary>
        /// <param name="param">The input.</param>
        /// <param name="func">The function to execute.</param>
        /// <returns>
        /// The result of the function.
        /// </returns>
        public static TOut Pipe<TIn, TOut>(this TIn param, Func<TIn, TOut> func)
        {
            return func(param);
        }
    }
}

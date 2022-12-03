using System.Collections.Generic;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Create a continuation that receives an <see cref="Action"/> and will be executed when the task has been executed.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task"/> representing the pending operation.
        /// </param>
        /// <param name="continuationAction">
        /// An <see cref="Action"/> to run when the task completes.
        /// </param>
        /// <returns>
        /// A new continuation <see cref="Task"/>.
        /// </returns>
        public static async Task ContinueWithResult(this Task task, Action continuationAction)
        {
            await task.ContinueWith(t =>
            {
                if (t.IsFaulted) throw t.Exception;
                continuationAction();
            });
        }

        /// <summary>
        /// Create a continuation that receives an <see cref="Action{TResult}"/> and will be executed when the task has been executed.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task{TResult}"/> representing the pending operation.
        /// </param>
        /// <param name="continuationAction">
        /// An <see cref="Action{TResult}"/> to run when the task completes.
        /// </param>
        /// <returns>
        /// A new continuation <see cref="Task"/>.
        /// </returns>
        public static async Task ContinueWithResult<TResult>(this Task<TResult> task, Action<TResult> continuationAction)
        {
            await task.ContinueWith(t =>
            {
                if (t.IsFaulted) throw t.Exception;
                continuationAction(task.Result);
            });
        }

        /// <summary>
        /// Create a continuation that receives an <see cref="Func{TResult, TNewResult}"/> and will be executed when the task has been executed.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task{TResult}"/> representing the pending operation.
        /// </param>
        /// <param name="continuationFunction">
        /// An <see cref="Func{TResult, TNewResult}"/> to run when the task completes.
        /// </param>
        /// <returns>
        /// A new continuation <see cref="Task"/> with the result of the function.
        /// </returns>
        public static async Task<TNewResult> ContinueWithResult<TResult, TNewResult>(this Task<TResult> task, Func<TResult, TNewResult> continuationFunction)
        {
            return await task.ContinueWith(t =>
            {
                if (t.IsFaulted) throw t.Exception;
                return continuationFunction(task.Result);
            });
        }

        /// <summary>
        /// Create a continuation that receives an <see cref="Func{TResult, Task}"/> and will be executed when the task has been executed.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task{TResult}"/> representing the pending operation.
        /// </param>
        /// <param name="continuationFunction">
        /// An <see cref="Func{TResult, Task}"/> to run when the task completes.
        /// </param>
        /// <returns>
        /// A new continuation <see cref="Task"/> with the result of the function.
        /// </returns>
        public static async Task<TNewResult> ContinueWithResult<TResult, TNewResult>(this Task<TResult> task, Func<TResult, Task<TNewResult>> continuationFunction)
        {
            return await task.ContinueWith(t =>
            {
                if (t.IsFaulted) throw t.Exception;

                return continuationFunction(task.Result);
            }).Unwrap();
        }

        /// <summary>
        /// Create a continuation that receives an <see cref="Func{TResult, IEnumerable}"/> and will be executed when the task has been executed.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task"/> representing the pending operation.
        /// </param>
        /// <param name="continuationFunction">
        /// An <see cref="Func{TResult, IEnumerable}"/> to run when the task completes.
        /// </param>
        /// <returns>
        /// A new continuation <see cref="Task"/> with the result of the function.
        /// </returns>
        public static async Task<TNewResult[]> ContinueWithResult<TResult, TNewResult>(this Task<TResult> task, Func<TResult, IEnumerable<Task<TNewResult>>> continuationFunction)
        {
            return await task.ContinueWith(delegate (Task<TResult> t)
            {
                if (t.IsFaulted)
                {
                    throw t.Exception;
                }

                return Task.WhenAll(continuationFunction(task.Result));
            }).Unwrap();
        }


        /// <summary>
        /// Returns a completed task if task is null.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task"/> for await.
        /// </param>
        public static Task ForAwait(this Task task)
        {
            return task ?? Task.CompletedTask;
        }

        /// <summary>
        /// Returns a task with the deafult value if task is null.
        /// </summary>
        /// <param name="task">
        /// The <see cref="Task{TResult}"/> for await.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the task is null.
        /// </param>
        public static Task<TResult> ForAwait<TResult>(this Task<TResult> task, TResult defaultValue = default)
        {
            return task ?? Task.FromResult(defaultValue);
        }
    }
}

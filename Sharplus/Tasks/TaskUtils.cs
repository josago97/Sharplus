using System;
using System.Threading.Tasks;

namespace Sharplus.Tasks
{
    // https://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously
    public static class TaskUtils
    {
        /// <summary>
        /// Executes an async <see cref="Task"/> method synchronously
        /// </summary>
        /// <param name="task">
        /// <see cref="Task"/> method to execute
        /// </param>
        public static void RunSync(Func<Task> task)
        {
            Task.Run(async () => await task().ConfigureAwait(false)).Wait();
        }

        /// <summary>
        /// Executes an async <see cref="Task{T}"/> method synchronously
        /// </summary>
        /// <param name="task">
        /// <see cref="Task{T}"/> method to execute
        /// </param>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            return Task.Run(async () => await task().ConfigureAwait(false)).Result;
        }
    }
}

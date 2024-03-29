﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Sharplus.Tasks;
using Xunit;

namespace Sharplus.Tests
{
    public class TaskTest
    {
        [Fact]
        public async void ContinueWithResultWithoutException()
        {
            await TaskWithoutException().ContinueWithResult(() => { });
        }

        [Fact]
        public async void ContinueWithResultWithException()
        {
            Task task = TaskWithException().ContinueWithResult(() => { });

            await Assert.ThrowsAsync<AggregateException>(() => task);
            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
        }

        [Fact]
        public async void ContinueWithResultActionWithParamenterWithoutException()
        {
            await TaskWithoutException().ContinueWithResult((_) => { });
        }

        [Fact]
        public async void ContinueWithResultActionWithParamenterWithException()
        {
            Task task = TaskWithException().ContinueWithResult((_) => { });

            await Assert.ThrowsAsync<AggregateException>(() => task);
            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
        }

        [Fact]
        public async void ContinueWithResultFunctionWithoutException()
        {
            await TaskWithoutException().ContinueWithResult((_) => _);
        }

        [Fact]
        public async void ContinueWithResultFunctionWithException()
        {
            Task task = TaskWithException().ContinueWithResult((_) => _);

            await Assert.ThrowsAsync<AggregateException>(() => task);
            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
        }

        [Fact]
        public async void ContinueWithResultTaskWithoutException()
        {
            int result = await TaskWithoutException().ContinueWithResult((_) => TaskWithoutException());

            Assert.Equal(await TaskWithoutException(), result);
        }

        [Fact]
        public async void ContinueWithResultTaskWithException()
        {
            Task task = TaskWithoutException().ContinueWithResult((_) => TaskWithException());

            await Assert.ThrowsAsync<Exception>(() => task);
            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
        }

        [Fact]
        public async void ContinueWithResultTaskIEnumarableWithoutException()
        {
            Task<int[]> task = TaskWithoutException()
                .ContinueWithResult(_ => Enumerable.Range(0, 10).Select(_ => TaskWithoutException()));

            int[] expected = await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => TaskWithoutException()));

            Assert.Equal(expected, await task);
        }

        [Fact]
        public async void ContinueWithResultTaskIEnumarableWithException()
        {
            Task task = TaskWithoutException()
                .ContinueWithResult(_ => Enumerable.Range(0, 10).Select(_ => TaskWithException()));

            await Assert.ThrowsAsync<Exception>(() => task);
            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
        }

        [Fact]
        public async void ForAwaitWithoutResult()
        {
            Task nullTask = null;
            Task task = Task.Run(() => { });

            await nullTask.ForAwait();
            await task.ForAwait();
        }

        [Fact]
        public async void ForAwaitWithResult()
        {
            int result = 1;
            Task<int> nullTask = null;
            Task<int> task = Task.Run(() => result);

            Assert.Equal(result, await nullTask.ForAwait(result));
            Assert.Equal(result, await task.ForAwait());
        }

        [Fact]
        public void RunSyncWithDeadlock()
        {
            TaskUtils.RunSync(async () =>
            {
                TaskUtils.RunSync(() => Task.Delay(1000));
                await Task.Delay(10000);
            });
        }

        [Fact]
        public void RunSyncWithoutException()
        {
            TaskUtils.RunSync(TaskWithoutException);
        }

        [Fact]
        public void RunSyncWithException()
        {
            Assert.Throws<AggregateException>(() => TaskUtils.RunSync(TaskWithException));
        }

        private async Task<int> TaskWithoutException()
        {
            await Task.Yield();

            return 0;
        }

        private async Task<int> TaskWithException()
        {
            await Task.Yield();
            throw new Exception();
        }
    }
}

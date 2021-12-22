using System;
using Sharplus.Architecture.Locator;
using Xunit;

namespace Sharplus.Tests.Architecture
{
    public class ServiceLocatorTest : IDisposable
    {
        public void Dispose()
        {
            ServiceLocator.Clear();
        }

        [Fact]
        public void GetWhenIsEmpty()
        {
            object result = ServiceLocator.Get<object>();
            Assert.Null(result);
        }

        [Fact]
        public void GetWhenTypeExist()
        {
            ServiceLocator.Set(new object());
            object result = ServiceLocator.Get<object>();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetWhenTypeNotExist()
        {
            ServiceLocator.Set(new object());
            int? result = ServiceLocator.Get<int?>();
            Assert.Null(result);
        }
    }
}

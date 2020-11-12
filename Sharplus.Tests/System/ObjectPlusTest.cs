using System;
using Xunit;

namespace Sharplus.Tests.System
{
    public class ObjectPlusTest
    {
        [Fact]
        public void Swap()
        {
            int a = 0;
            int b = 1;

            int auxA = a;
            int auxB = b;

            ObjectPlus.Swap(ref auxA, ref auxB);

            Assert.Equal(a, auxB);
            Assert.Equal(b, auxA);  
        }
    }
}

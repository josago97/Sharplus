using Xunit;

namespace Sharplus.Tests.System
{
    public class SharpUtilsTest
    {
        [Fact]
        public void SwapReferences()
        {
            int a = 0;
            int b = 1;

            int auxA = a;
            int auxB = b;

            SharpUtils.SwapReferences(ref auxA, ref auxB);

            Assert.Equal(a, auxB);
            Assert.Equal(b, auxA);  
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharplus.Pipelines;
using Xunit;

namespace Sharplus.Tests.Reactive
{
    public class ReactivePropertyTest
    {
        [Fact]
        public void ChangeValue()
        {
            A a = new A();
            a.Property.Value = 1;
        }

        [Fact]
        public void Equal()
        {
            A a = new A();
            Assert.Equal(0, a.Property.Value);
        }
    }

    public class A
    {
        public ReactiveProperty<int> Property = new ReactiveProperty<int>();
    }
}

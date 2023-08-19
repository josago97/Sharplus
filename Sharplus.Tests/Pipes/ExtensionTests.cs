using System;
using Sharplus.Pipelines;
using Xunit;

namespace Sharplus.Tests.Pipes;

public class ExtensionTests
{
    [Fact]
    public void PipeAction()
    {
        int num = 1;

        Action<int> duplicate = (n) => num = n * 2;
        num.Pipe(duplicate);

        Assert.Equal(2, num);
    }

    [Fact]
    public void PipeFunction()
    {
        int num = 1;

        Func<int, int> duplicate = (n) => n * 2;
        int result = num.Pipe(duplicate);

        Assert.Equal(2, result);
    }
}

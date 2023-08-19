using Sharplus.Pipelines;
using Xunit;

namespace Sharplus.Tests.Pipes;

public class ReactivePropertyTests
{
    [Fact]
    public void ChangeEvent()
    {
        bool isChanged = false;
        int newValue = -1;
        ReactiveProperty<int> property = new ReactiveProperty<int>(0);
        property.ValueChangedCallback += () => isChanged = true;
        property.ValueChanged += x => newValue = x;

        property.Value = 1;

        Assert.True(isChanged);
        Assert.True(property.Value == newValue);
        Assert.Equal(1, property.Value);
        Assert.Equal(0, property.LastValue);
    }

    [Theory]
    [InlineData(0, null, null)]
    [InlineData(-1, null, "")]
    [InlineData(1, "", null)]
    [InlineData(0, "", "")]
    [InlineData(0, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(-1, 1, 2)]
    public void CompareToMethod<T>(int expected, T propertyValue, T value)
    {
        ReactiveProperty<T> property = new ReactiveProperty<T>(propertyValue);

        Assert.Equal(expected, property.CompareTo(value));
        Assert.Equal(expected, property.CompareTo(new ReactiveProperty<T>(value)));
    }

    [Theory]
    [InlineData(false, null, "")]
    [InlineData(false, "", null)]
    [InlineData(false, 0, 1)]
    [InlineData(true, null, null)]
    [InlineData(true, 0, 0)]
    public void EqualsMethod<T>(bool expected, T propertyValue, T value)
    {
        ReactiveProperty<T> property = new ReactiveProperty<T>(propertyValue);

        Assert.Equal(expected, property.Equals(value));
        if (value != null) Assert.Equal(expected, value.Equals(property.Value));
        Assert.Equal(expected, property == value);
        Assert.Equal(expected, value == property);
        Assert.Equal(expected, property == new ReactiveProperty<T>(value));
    }

    [Theory]
    [InlineData(0, null)]
    [InlineData(1, 1)]
    public void GetHashCodeMethod<T>(int expected, T value)
    {
        ReactiveProperty<T> property = new ReactiveProperty<T>(value);

        Assert.Equal(expected, property.GetHashCode());
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("0", 0)]
    public void ToStringMethod<T>(string expected, T value)
    {
        ReactiveProperty<T> property = new ReactiveProperty<T>(value);

        Assert.Equal(expected, property.ToString());
    }

    /*
    [Theory]
    [InlineData()]
    public void Equals<T>(bool expected, T value)
    {
        ReactiveProperty<T> property = new ReactiveProperty<T>(value);
        Assert.True(property.Equals(value));
        Assert.True(property == value);
    }*/
}

# Sharplus 
[![license](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/josago97/Sharplus/blob/main/LICENSE) [![NuGet version (Sharplus)](https://img.shields.io/nuget/v/Sharplus.svg)](https://www.nuget.org/packages/Sharplus/) [![downloads](https://img.shields.io/nuget/dt/Sharplus.svg)](https://www.nuget.org/packages/Sharplus)

 A library with things that would come in handy for C#

## Collections

### Circular array
<img src="circular_buffer_animation.gif" width="400px">

```cs
// Create a CircularArray with a capacity of 3
CircularArray<int> circularArray = new CircularArray<int>(3);

for(int i = 0; i < 5; i++)
    circularArray.Add(i);

// Prints [2, 3, 4]
Console.WriteLine($"[{string.Join(", ", circularArray)}]")
```

## Pipelines

### Pipe
```cs
int num = 1;

// Prints 2
num.Pipe(x => x * 2).Pipe(Console.WriteLine);
```

### Reactive property
```cs
ReactiveProperty<int> property1 = new ReactiveProperty();
ReactiveProperty<int> property2 = new ReactiveProperty(10);

// Prints 2
property2.ValueChanged += x => Console.WriteLine(x);
property2.Value = 2;

// Prints False
Console.WriteLine(property1 == property2);
// Prints True
Console.WriteLine(property1 == 0);
```

## Sequence matching
A little port of [StringSimilarity.NET](https://github.com/feature23/StringSimilarity.NET) to work with any collection. The following algorithms are implemented in this library:

| Algorithm | Operations | Normalized? 	| Metric?<sup>1</sup> | Cost |
|-----------|------------|--------------|---------|------|
| [Levenshtein](#levenshtein) | Distance | No | Yes | O(m*n) |
| [Normalized Levenshtein](#normalized-levenshtein)	| Distance<br>Similarity	| Yes | No | O(m*n) |
| [Damerau](#damerau) | Distance | No | Yes | O(m*n) |
| [Longest Common Subsequence](#longest-common-subsequence) | Distance | No | No | O(m*n) |

[1] The ISequenceMetricDistance interface : A few of the distances are actually metric distances, which means that verify the triangle inequality d(x, y) <= d(x,z) + d(z,y). For example, Levenshtein is a metric distance, but NormalizedLevenshtein is not.

### Levenshtein
The Levenshtein distance between two sequences is the minimum number of single-item edits (insertions, deletions or substitutions) required to change one sequence into the other.
```cs
ISequenceDistance calculator = new Levenshtein();

// Prints 2
Console.WriteLine(calculator.Distance("hello", "elo"));

// Prints 3
Console.WriteLine(calculator.Distance(new[] { 1, 2, 3 }, new[] { 3, 4, 5 }));
```

### Normalized Levenshtein
This distance is computed as levenshtein distance divided by the length of the longest sequence. The resulting value is always in the interval [0.0, 1.0] but it is not a metric anymore!

The similarity is computed as 1 - normalized distance.

```cs
NormalizedLevenshtein calculator = new NormalizedLevenshtein();

// Prints 0.4
Console.WriteLine(calculator.Distance("hello", "elo"));
// Prints 0.6
Console.WriteLine(calculator.Similarity("hello", "elo"));
```

### Damerau
Similar to Levenshtein, Damerau-Levenshtein distance with transposition is the minimum number of operations needed to transform one sequence into the other, where an operation is defined as an insertion, deletion, or substitution of a single item, or a transposition of two adjacent items.

It does respect triangle inequality, and is thus a metric distance.

```cs
ISequenceDistance calculator = new Damerau();

// Prints 2
Console.WriteLine(calculator.Distance("hello", "elo"));
```

### Longest Common Subsequence
The longest common subsequence (LCS) problem consists in finding the longest subsequence common to two (or more) sequences. Subsequences are not required to occupy consecutive positions within the original sequences.

The LCS distance between sequences X (of length n) and Y (of length m) is n + m - 2 |LCS(X, Y)| min = 0 max = n + m

LCS distance is equivalent to Levenshtein distance when only insertion and deletion is allowed (no substitution), or when the cost of the substitution is the double of the cost of an insertion or deletion.

```cs
ISequenceDistance calculator = new LongestCommonSubsequence();

// Prints 2
Console.WriteLine(calculator.Distance("hello", "elo"));

// Prints 4
Console.WriteLine(calculator.Distance(new[] { 1, 2, 3 }, new[] { 3, 4, 5 }));
```

## System
Included without "using".

### LINQ

#### Statistics

- AllMax
<br>
Returns all maximum values in a generic sequence.

```cs
int[] numbers = new int[] {1, 2, 3, 4, 4};

// [4, 4]
IEnumerable<int> max = numbers.AllMax();
```

- AllMin
<br>
Returns all minimum values in a generic sequence.

```cs
int[] numbers = new int[] {1, 1, 2, 3, 4};

// [1, 1]
IEnumerable<int> min = numbers.AllMin();
```

- Median
<br>
Computes the median of a sequence

```cs
int[] numbers = new int[] {1, 2, 3, 4, 5};

// 2
double median = numbers.Median();
```

- Kurtosis
<br>
Computes the kurtosis of a sequence

```cs
int[] numbers = new int[] {1, 2, 3, 4, 5};

// -1.3
double kurtosis = numbers.Kurtosis();
```

- Mode
<br>
The most repeated element in the sequence.

```cs
int[] numbers = new int[] {1, 2, 3, 3, 4, 5};

// mode = 3, times = 2
int mode = numbers.Mode(out int times);
```

- Mode
<br>
The most repeated element in the sequence.

```cs
int[] numbers = new int[] {1, 2, 3, 3, 4, 5};

// mode = 3, times = 2
int mode = numbers.Mode(out int times);
```

- Skewness
<br>
Computes the skewness of a sequence

```cs
int[] numbers = new int[] {1, 2, 3, 4, 4, 5};

// -0.3
double skewness = numbers.Skewness();
```

- Standard deviation
<br>
Computes the standard deviation of a sequence

```cs
int[] numbers = new int[] {1, 2, 3, 4, 4, 5};

// 1.41
double standardDeviation = numbers.StandardDeviation();
```

- Variance
<br>
Computes the variance of a sequence

```cs
int[] numbers = new int[] {1, 2, 3, 4, 5};

// 2
double variance = numbers.Variance();
```

#### Other

- Batch
<br>
Batches the source sequence into sized sequences

```cs
int[] numbers = new int[] {1, 2, 3, 4};

// [[1, 2], [3, 4]]
IEnumerable<IEnumerable<int>> sequences = numbers.Batch(2);
```

- Concat
<br>
Concatenates sequences

```cs
int[] numbers1 = new int[] {1, 2};
int[] numbers2 = new int[] {3, 4};
int[] numbers3 = new int[] {5, 6};

// [1, 2, 3, 4, 5, 6]
IEnumerable<int> sequence = numbers.Concat(numbers2, numbers3);
```

- Dump
<br>
Shows the content of the sequence in a `string`.

```cs
int[] numbers = new int[] {1, 2, 3};

// Prints [1, 2, 3]
Console.WriteLine(numbers.Dump());
```

- FindAllIndexes
<br>
Searches for all elements that matches the conditions defined by the specified predicate, and returns the zero-based index of the all occurrences within the range of elements in these sequence that extends from the specified index to the last element.

```cs
int[] numbers = new int[] {1, 2, 3, 4, 5};

// [2, 3, 4]
IEnumerable<int> indexes = numbers.FindAllIndexes(2, x => x > 0);
```

- FindIndex
<br>
Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the sequence that starts at the specified index and contains the specified number of elements.

```cs
int[] numbers = new int[] {1, 2, 3, 4, 5};

// 2
int index = numbers.FindIndex(2, x => x > 0);
```
//TODO
- FirstOrDefault
<br>


```cs
int[] numbers = new int[] {1, 2, 3, 4, 5};

// 2
int index = numbers.FindIndex(2, x => x > 0);
```

### Array extensions

### MathPlus

### Array extensions

### String extensions

## Task
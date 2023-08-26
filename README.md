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

### LINQ

#### Statistics

- AllMax <br>
Returns all maximum values in a generic sequence.

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 4 };

// [4, 4]
IEnumerable<int> max = numbers.AllMax();
```

- AllMin <br>
Returns all minimum values in a generic sequence.

```cs
int[] numbers = new int[] { 1, 1, 2, 3, 4 };

// [1, 1]
IEnumerable<int> min = numbers.AllMin();
```

- Median <br>
Computes the median of a sequence

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

// 2
double median = numbers.Median();
```

- Kurtosis <br>
Computes the kurtosis of a sequence

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

// -1.3
double kurtosis = numbers.Kurtosis();
```

- Mode <br>
The most repeated element in the sequence.

```cs
int[] numbers = new int[] { 1, 2, 3, 3, 4, 5 };

// mode = 3, times = 2
int mode = numbers.Mode(out int times);
```

- Mode <br>
The most repeated element in the sequence.

```cs
int[] numbers = new int[] { 1, 2, 3, 3, 4, 5 };

// mode = 3, times = 2
int mode = numbers.Mode(out int times);
```

- Skewness <br>
Computes the skewness of a sequence

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 4, 5 };

// -0.3
double skewness = numbers.Skewness();
```

- Standard deviation <br>
Computes the standard deviation of a sequence

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 4, 5 };

// 1.41
double standardDeviation = numbers.StandardDeviation();
```

- Variance <br>
Computes the variance of a sequence

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

// 2
double variance = numbers.Variance();
```

#### Other

- Batch <br>
Batches the source sequence into sized sequences

```cs
int[] numbers = new int[] { 1, 2, 3, 4 };

// [[1, 2], [3, 4]]
IEnumerable<IEnumerable<int>> sequences = numbers.Batch(2);
```

- Concat <br>
Concatenates sequences

```cs
int[] numbers1 = new int[] { 1, 2 };
int[] numbers2 = new int[] { 3, 4 };
int[] numbers3 = new int[] { 5, 6 };

// [1, 2, 3, 4, 5, 6]
IEnumerable<int> sequence = numbers.Concat(numbers2, numbers3);
```

- Dump <br>
Shows the content of the sequence in a `string`.

```cs
int[] numbers = new int[] { 1, 2, 3 };

// Prints [1, 2, 3]
Console.WriteLine(numbers.Dump());
```

- FindAllIndexes <br>
Searches for all elements that matches the conditions defined by the specified predicate, and returns the zero-based index of the all occurrences within the range of elements in these sequence that extends from the specified index to the last element.

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

// [2, 3, 4]
IEnumerable<int> indexes = numbers.FindAllIndexes(2, x => x > 0);
```

- FindIndex <br>
Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the sequence that starts at the specified index and contains the specified number of elements.

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

// 2
int index = numbers.FindIndex(2, x => x > 0);
```

- FirstOrDefault <br>
Returns the first element of the sequence that satisfies a condition or the default value if no such element is found.

```cs
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

// 10
int number = numbers.FirstOrDefault(x => x > 6, 10);
```

- Flatten <br>
Returns the first element of the sequence that satisfies a condition or the default value if no such element is found.

```cs
int[][] numbers = new int[] { new int[] { 1, 2 }, new int[] { 3, 4 } };

// [1, 2, 3, 4]
int[] numbers = numbers.Flatten();
```

- Foreach <br>
Performs the specified action on each element of the sequence.

```cs
int[] numbers = new int[] {  1, 2, 3, 4 };

numbers.Foreach(x => Console.WriteLine(x));
```

- GetRandom <br>
Gets a random element of a sequence.

```cs
int[] numbers = new int[] {  1, 2, 3, 4 };

int randomNumber = numbers.GetRandom();
```

- IsNullOrEmpty <br>
Indicates whether the specified collection is null or does not contain elements.

```cs
int[] numbers = new int[0];

// True
bool result = numbers.IsNullOrEmpty();
```

- Repeat <br>
Repeats the sequence the specified number of times.

```cs
int[] numbers = new int[] { 1, 2 };

// { 1, 2, 1, 2, 1, 2}
IEnumerable<int> result = numbers.Repeat(3);
```

- SequenceEquals <br>
Determines whether the first sequence and the second sequence contain the same elements.

```cs
int[] numbers1 = new int[] { 1, 2 };
List<string> numbers2 = new List<string>() { "1", "2" };

// True
bool result = numbers1.SequenceEquals(numbers2, (int x, string y) => x == int.Parse(y));
```

- SubSequence <br>
Returns a subsequence that contains all elements within a range.

```cs
int[] numbers = new int[] { 1, 2, 3, 4 };

// { 2, 3 }
IEnumerable<int> subsequence = numbers.SubSequence(1, 2);
```

- Shuffle <br>
Shuffles a sequence randomly.

```cs
int[] numbers = new int[] { 1, 2, 3, 4 };

IEnumerable<int> shuffled = numbers.Shuffle();
```

- SymmetricExcept <br>
Gets a sequence that contains the elements that are present either in that sequence or in the other specified sequence, but not both.

```cs
int[] numbers1 = new int[] { 1, 2, 3 };
int[] numbers2 = new int[] { 2, 3, 4 };

// { 1, 4 }
IEnumerable<int> result = numbers.SymmetricExcept();
```

- Swap <br>
Swaps the positions of two elements in a list.

```cs
int[] numbers = new int[] { 1, 2, 3 };

// { 1, 3, 2 }
IEnumerable<int> result = numbers.Swap(1, 2);
```

### Array extensions

- SubArray <br>
Returns a subarray that contains all elements within a range.

```cs
int[] numbers = new int[] { 1, 2, 3 };

// [2, 3]
int[] subarray = numbers.SubArray(1, 2);
```

### Enumerable plus

- Range <br>
Generates a sequence within a specified range and with a determined increment.

```cs
// { 2, 2.5, 3, 3.5, 4 }
IEnumerable<float> sequene1 = EnumerablePlus.Range(2, 5, 0.5f);
IEnumerable<double> sequene1 = EnumerablePlus.Range(2, 5, 0.5);
```

### Environment utils

- LoadVariables <br>
Returns a subarray that contains all elements within a range.

```cs
/* settings.env
# This is a comment
MY_VAR_1 = value1
MY_VAR_2 = value2
*/

EnvironmentUtils.Load("settings.env");
```

### Equality comparer

```cs
IEqualityComparer<int, string> equalityComparer = new EqualityComparer((int x, string y) => x == int.Parse(y));

// True
bool result = equalityComparer.Equals(1, "1")
```

### MathPlus

- Factorial <br>
Returns the factorial of a number.

```cs
// 6
long factorial = MathPlus.Factorial(3);
```

- Fibonacci <br>
Returns the fibonacci sequence of a number.

```cs
// { 1, 1, 2, 3, 5 }
IEnumerablez<long> fibonacci = MathPlus.Fibonacci(6);
```

- InverseLerp <br>
Calculates the linear parameter that produces the interpolant value within a range.

```cs
// 0.5
double result = MathPlus.InverseLerp(1, 2, 1.5);
```

- IsErrorInRange <br>
Determines whether the absolute error between two numbers is inside a range.

```cs
// True
bool result = MathPlus.IsErrorInRange(1, 1.1, 0.2);
```

- Lerp <br>
Calculates the linear interpolation between a range by a interpolation value.

```cs
// 1.5
double result = MathPlus.Lerp(1, 2, 0.5);
```

- LerpUnclamped <br>
Calculates the linear interpolation between a range by a interpolation value.

```cs
// 1.5
double result = MathPlus.Lerp(1, 2, 0.5);
// 4
double result = MathPlus.Lerp(1, 2, 2);
```

- Max <br>
Returns the maximum value of a series of numbers.

```cs
// 4
double max = MathPlus.Max(1, 2, 3, 4);
```

- Min <br>
Returns the minimum value of a series of numbers.

```cs
// 1
double min = MathPlus.Min(1, 2, 3, 4);
```

- Mod <br>
Calculates the remainder or signed remainder of a division.

```cs
// 1
double result1 = MathPlus.Mod(10, 3);

// 1
double result1 = MathPlus.Mod(10, -3);
```

- GreatestCommonDivisor <br>
Calculates the greates common divisor of a series of numbers

```cs
// 2
double greatestCommonDivisor = MathPlus.GreatestCommonDivisor(2, 6, 8);
```

- LessCommonMultiple <br>
Calculates the less common multiple of a series ofnumbers

```cs
// 2
double lessCommonMultiple = MathPlus.LessCommonMultiple(2, 3);
```

- AbsoluteError <br>
Calculates the absolute error between two numbers.

```cs
// 1
double absoluteError = MathPlus.AbsoluteError(2, 3);
```

- RelativeError <br>
Calculates the relative error between two numbers.

```cs
// 0.5
double relativeError = MathPlus.RelativeError(2, 3);
```

### String extensions

- EndsWith <br>
Determines if the end of a string matches any of the specified strings.

```cs
string text = "This is a sample text";

// result = True, suffixIndex = 1
bool result = text.EndsWith(out int suffixIndex, new [] { "var", "ext" });
```

- FindOcurrences <br>
Searches all present occurrences of a pattern in a string and returns the indices where each occurrence starts.

```cs
string text = "This is a sample text";

// [2, 5]
int[] result = text.FindOcurrences("is");
```

- RemoveAccents <br>
Returns a copy of a string without accents.

```cs
string text = "España es un país";

// Espana es un pais
string result = text.RemoveAccents();
```

```cs
string text = "El murciélago es un animal mamífero";

// El murcielago es un animal mamífero
string result = text.RemoveAccents("í");
```

- Repeat <br>
Returns a new string which contains the specified number of copies of the string on which it was called, concatenated together.

```cs
string text = "test";

// testtest
string result = text.Repeat(2);
```

- StartsWith <br>
Determines if the start of a string matches any of the specified strings.

```cs
string text = "This is a sample text";

// result = True, suffixIndex = 1
bool result = text.StartsWith(out int suffixIndex, new [] { "var", "Thi" });
```

- ToBase64 <br>
Returns a copy of a string that is encoded with base-64 digits.

```cs
string text = "This is a sample text";

// VGhpcyBpcyBhIHNhbXBsZSB0ZXh0
string base64 = text.ToBase64();
```

- TryFromBase64 <br>
Tries to decode a string that is encoded with base-64 digits and encodes it to the default encoding.

```cs
string base64 = "VGhpcyBpcyBhIHNhbXBsZSB0ZXh0";

// result = True, text = "This is a sample text"
bool result = base64.TryFromBase64(out string text);
```

- ToTitleCase <br>
Returns a copy of a string converted to title case.

```cs
string text = "this is a sample text";

// This is a sample text
string result = text.ToTitleCase();
```

```cs
string text = "this is a sample text";

// This Is A Sample Text
string result = text.ToTitleCase(false);
```

- Translate <br>
Returns a copy of a string in which all occurrences of all specified replacements are replaced.

```cs
string text = "This is an example text";
Dictionary<string, string> replacements = new Dictionary<string, string>()
{
    { "is", "is not" },
    { "an", "a" }
    { "example", "sample" }
};

// This is not a sample text
string result = text.Translate(replacements);
```

### Other extensions

- Duplicate stream <br>
Duplicates a stream with its content.

```cs
Stream original = GetStream();
Stream copy = original.Duplicate();
```

- Read stream as byte array<br>
Writes the stream contents to a byte array, regardless of the stream.

```cs
Stream stream = GetStream();
byte[] content = stream.ReadAsByteArray();
```

- Next random double<br>
Returns a random floating-point number that is within a specified range.

```cs
Random random = new Random();

// Random number in range [1, 2]
double randomNumber = random.NextDouble(1, 2);
```

## Task

### Extensions

- ContinueWithResult<br>
Create a continuation that receives an Action or Function and will be executed when the task has been executed.

```cs
Task<int> task = GetNumberAsync();

// Multiply the number by 2 and then print it
await task.ContinueWithResult(x => x * 2).ContinueWithResult(x => Console.WriteLine(x));
```

- ForAwait<br>
Returns a completed task or a task with the deafult value if the task is null.

```cs
Task task = null;
await task.ForAwait();
```

```cs
Task<int> task = null;
int number = await task.ForAwait(10);
```

### Utils
// TODO terminar y revisar
- RunSync<br>
Executes an async Task method synchronously.

```cs
Task task = null;
await task.ForAwait();
```

## SharpUtils
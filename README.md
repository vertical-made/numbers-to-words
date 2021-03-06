# numbers-to-words

A collection of dotnet extension methods to convert numbers to their linguistic representations

- [nuget package](https://www.nuget.org/packages/numbers-to-words/)

## Use

```csharp
using VerticalMade.NumbersToWords;

var input = 19_000_000_001.01M;
var dollars = input.ToDollarsAndCents(); // Nineteen billion and one dollars and one cent
```

## Committing

Make sure to increment `<Version>` in `NumbersToWords.csproj` appropriately.

### Contributor Covenant

This project adheres to the Contributor Covenant [code of conduct](https://www.contributor-covenant.org/version/2/0/code_of_conduct.md).
By participating, you are expected to uphold this code. Please report unacceptable behavior to ops@verticalmade.com.

# Unit Testing Lab

## Overview

In this Lab, you will:

- start from a basic Calculator class and create a simple Unit Test,
- use a TDD approach to implement the divide method in the Calculator,
- add more tests with different Asserts, attributes and settings for the tests,
- to end with wiring an external MS SQL database for tests values.

The aim is to browse quickly from simple to advanced Unit Tests capabilities.
The technologies used in this Lab are:

- Visual Studio 2022, with its MSSQLLocalDB SQL Server instance
- C# .NET 6
- MSTest V2

> Note:
>
> Visual Studio Enterprise Edition brings many features for Unit Testing, such as:
>
> - [Live Unit testing](https://learn.microsoft.com/en-us/visualstudio/test/live-unit-testing-intro?view=vs-2022)
> - [IntelliTest](https://learn.microsoft.com/en-us/visualstudio/test/intellitest-manual/?view=vs-2022)
> - [Microsoft Fakes](https://learn.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes?view=vs-2022&tabs=csharp)
> - [Code coverage](https://learn.microsoft.com/en-us/visualstudio/test/using-code-coverage-to-determine-how-much-code-is-being-tested?view=vs-2022&tabs=csharp)
>
> The lab will not cover these features, but we will show some of them in the demos.

---

## Lab instructions

### Prepare the environment

- Download the starting project from here:

- Unzip the project in a folder

- Open the solution in Visual Studio

- See the solution content:

![Start solution](./img/StartSolution.png)

This ConsoleApp:

- Starts
- Displays a message
- Wait for an entry
- Reacts in 3 ways to the entry entered:
  1. Writes the text entered
  2. if `"Enter"` is typed, clears the screen
  3. if `"q"` is typed, exits the program


### Add a MSTest project to the solution


- Add a new Project to the solution:
  - `Project type: MSTest Test Project`
  - `Project name: ConsoleAppTests`
  - `Framework: .NET 6.0 (LTS)`

- Open project `ConsoleAppTests` properties (Alt + Enter) and:

  - Right click on `Dependencies` and `Add Project Reference` to the project `ConsoleApp`
    > This makes our ConsoleApp code available for our tests.

  - Select `Global Usings / General`
  - Uncheck `Implicit global usings | Enable implicit global usings to be declared by the project SDK`
  - Delete `Usings.cs` file
  - Add `using Microsoft.VisualStudio.TestTools.UnitTesting;` on the first line of the `UnitTest.cs` file
    > This allows to see all the dependencies explicitly in the code.

### 1st Unit Test








---

## References

To create this Lab for H&R Block's BAC team's DevOps continuous quality workshop, the following sources were used:

- [Testing in .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/)
- [Unit testing C# with MSTest and .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [First look at testing tools in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/test/improve-code-quality?view=vs-2022)
- [Pluralsight | Implementing C# 9 Unit Testing Using Visual Studio 2019 and .NET 5](https://app.pluralsight.com/library/courses/basic-unit-testing-csharp-developers/table-of-contents)
- [Pluralsight | Testing .NET Code with xUnit.net 2: Getting Started](https://app.pluralsight.com/library/courses/dotnet-core-testing-code-xunit-dotnet-getting-started/table-of-contents)
- [Tim Corey | Intro to Unit Testing in C# using xUnit](https://youtu.be/ub3P8c87cwk)

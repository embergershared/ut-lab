# Unit Testing Lab

## Overview

In this Lab, you will:

- start from a simple ConsoleApp implemented for testing and create simple Unit Test,
- Add a basic Calculator class and Unit Test its `Add` method,
- use a TDD approach to implement the `Divide` method in the Calculator,
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
> The lab will not cover these features, but we will show some of them during the demos.

---

## Lab instructions

---

### 0. Setup the lab environment

To get started:

- Clone the repo locally: `git clone https://github.com/embergershared/ut-lab.git`

- Open the solution `/0.Start/UnitTestLab.sln` in Visual Studio

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

- Start it to check it runs:

![Console](./img/ConsoleApp_display.png)

- Look at the way the `static Main()` in `Program.cs` initialize dependency injection then:
  - Start an instance of `IPogramMgr` with the `Run()` method,
  - And the `ProgramMgr` class gets an `IConsoleMgr` instance injected.

This allows to decouple the pieces with `seams` interfaces and enables easier testing.

### Add a MSTest project to the solution

You will create a Unit Test with MSTestV2 to test the `ConsoleMgr.WriteLine(string value)` method.

- Add a new Project to the solution:
  - `Project type: MSTest Test Project`
  - `Project name: ConsoleAppTests`
  - `Framework: .NET 6.0 (LTS)`

- In `ConsoleAppTests` project, right click on `Dependencies` and `Add Project Reference` to the project `ConsoleApp`
  > This makes our ConsoleApp code available for our tests.

- Open project `ConsoleAppTests` properties (Alt + Enter) and:

  - Select `Global Usings / General`
  - Uncheck `Implicit global usings | Enable implicit global usings to be declared by the project SDK`

  > This allows to see all the dependencies explicitly in the code.

- Delete `Usings.cs` file

- Add `using Microsoft.VisualStudio.TestTools.UnitTesting;` on the first line of the `UnitTest1.cs` file

- File / Save All.

---

### 1. 1st Unit Test

- Rename `UnitTest1.cs` file to `ConsoleMgrShould.cs`
- Accept Visual Studio rename all references by clicking `Yes`

  > It is recommended to name a test class `"<ClassUnderTestName>Should"`.

- Delete the method `TestMethod1()`

- Create the following Test method in `ConsoleMgrShould.cs`:

```cs
    [TestMethod]
    public void WriteLine_WritesToSystemConsole()
    {
        // Arrange

        // Assert

        // Act
        Assert.Inconclusive();
    }
```

  > This simple unit test method has the structure recommended for a Unit Test:
  >
  > - Its name starts by the method it tests, then a name related to the test
  >
  > - The 3 steps of a Unit Test are separated
  >
  > - Adding Assert.Inconclusive() will make the test `Skipped` but remind you that the test is not doing anything (yet)

- Run `Test / Run All Tests` from Visual Studio Menu and check in the Test Explorer the result:

![Skipped Test1](./img/Test1_Inconclusive.png)

- Replace the code in the test method for:

```cs
    [TestMethod]
    public void WriteLine_WritesToSystemConsole()
    {
        // Arrange
        const string expected = "Hello to the console";
        var sut = new ConsoleMgr();

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        sut.WriteLine(expected);
        var actual = sw.ToString();

        // Assert
        Assert.AreEqual(expected, actual);
    }
```

  > The variable `sut` stands for `System Under Test`. With this convention, we easily know what is tested in the unit test code.
  >
  > The `expected` and `actual` (or `result`) variables' names are usual names used in unit testing. It helps distinguish right away the elements for the `Assert`.

- Run `Test / Run All Tests`

- The test fails

- Let's debug the test:

  - Put a breakpoint (F9) on the line `var actual = ...`

  - In the Test Explorer, right-click on the test and launch `Debug`:

![Launch Debug](./img/Test1_LaunchDebug.png)

  - You can see in the breakpoint that we get an object that may not render back our expected variable as we don't control the formatting of the `.ToString()` extension

  ![Launch Debug](./img/Test1_SeeDebug.png)

  - To fix the test: add `.Trim()` after the `.ToString()` for the actual value

    > The line should now be: `var actual = sw.ToString().Trim();`

- Run `Test / Run All Tests`

- Check the test `Passed`.

  ![Launch Debug](./img/Test1_SeeResult.png)

---

### 2. Unit Testing a Calculator

In this exercise, we will create and test a very simple calculator to explore:

- Catching exceptions in Unit Tests
- Use multiple data in a test (Data-driven testing)
- Create a test before a Method implementation (TDD)












---

## References

To create this Lab for H&R Block's BAC team's DevOps continuous quality workshop, the following sources were used:

- [Testing in .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/)
- [Unit testing C# with MSTest and .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [First look at testing tools in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/test/improve-code-quality?view=vs-2022)
- [Pluralsight | Implementing C# 9 Unit Testing Using Visual Studio 2019 and .NET 5](https://app.pluralsight.com/library/courses/basic-unit-testing-csharp-developers/table-of-contents)
- [Pluralsight | Testing .NET Code with xUnit.net 2: Getting Started](https://app.pluralsight.com/library/courses/dotnet-core-testing-code-xunit-dotnet-getting-started/table-of-contents)
- [Tim Corey | Intro to Unit Testing in C# using xUnit](https://youtu.be/ub3P8c87cwk)

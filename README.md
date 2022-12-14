# Unit Testing Lab

- Overview
- Lab instructions
  - 0.Setup the lab environment
    - clone the lab repo
    - Add a MSTest project to the solution
  - 1.The first Unit Test
    - Create a "blank" Unit Test
    - Create a simple Unit Test
  - 2.Unit Testing a Calculator
    - Test for Exception(s)
    - Create an Inline Data-driven test
    - Create the Tests before the implementation (or TDD)
  - 3.MSTestV2
    - TestContext
    - Initialization & Cleanup
    - `runsettings`
    - Create a Dynamic from SQL Data-driven test
    - `dotnet test` CLI
- References
  - Lab creation support material
  - Assert reference
  - Unit Test Attributes reference
  - Dependencies & Mocks, Microsoft Fakes with Stubs & Shims

## Overview

In this Lab, you will:

- start from a simple ConsoleApp implemented for testing and create simple Unit Test,
- Add a basic `Calculator` class and Unit Test its `Add()` method,
- use a TDD approach to implement the `Divide()` method in the Calculator,
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

#### clone the lab repo

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
  - And the `ProgramMgr` class gets an `IConsoleMgr` - a wrapper for the platform's Console - instance injected.

This allows to decouple the pieces with `seams` and enables easier testing.

#### Add a MSTest project to the solution

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

### 1. The first Unit Test

#### Create a "blank" Unit Test

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

      // Act

      // Assert
      Assert.Inconclusive();
  }
  ```

  > This simple unit test method has the structure recommended for a Unit Test:
  >
  > - Its name starts by the method it tests, then a name related to the test
  >
  > - The 3 steps of a Unit Test are separated
  >
  > - Adding Assert.Inconclusive() will make the test `Skipped`. Typically, you add an `Assert.Inconclusive` statement to a test that you are still working on, to indicate it's not yet ready to be run.

- Run `Test / Run All Tests` from Visual Studio Menu and check in the Test Explorer the result:

  ![Skipped Test1](./img/Test1_Inconclusive.png)

#### Create a simple Unit Test

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

- Test for exception(s) in Unit Tests
- Use multiple data in a test (Data-driven testing)
- Create the tests before the Method implementation (TDD)

Let's get started:

#### Test for Exception(s)

- Create a new Class `Calculator.cs` in the `ConsoleApp` project's `Classes` folder

- Make the class `public`

- Create the `Add` method in the Calculator class:

  ```cs
    public double Add(double a, double b)
    {
        throw new NotImplementedException();
    }
  ```

- Create a new Class `CalculatorShould.cs` in the `ConsoleAppTests` project

- Make the class `public`

- Add the `[TestClass]` attribute to the class (just above the `public class` declaration)

  > This attribute tells MSTestV2 to include this class in its tests

- Now create the TestMethod:

  - From the template:

    ```cs
    [TestMethod]
    public void Add_TwoValues_ThrowAnException()
    {
        // Arrange

        // Act

        // Assert
        Assert.Inconclusive();
    }
    ```

  - Create this test:

    ```cs
    // [Ignore]
    [TestMethod]
    [Description("Check for a thrown NotImplementedException using TryCatch.")]
    public void Add_TwoValues_ThrowAnException()
    {
        // Arrange
        var sut = new Calculator();

        try
        {
          // Act
          sut.Add(2, 3);
        }
        // Assert
        catch (NotImplementedException)
        {
            // Test succeeded
            return;
        }

        // Test failed
        Assert.Fail("Call to Add(2, 3) did NOT throw an NotImplementedException");
    }
    ```

- `Test / Run All Tests`

- See that the test `Passed` as we expect an Exception of type `NotImplementedException` and receive it:

  ![ThrownException Pass](./img/Test2_PassThrownException.png)

    > The other way to test for an `Exception` is to use the attribute `[ExpectedException(typeof(NotImplementedException))]`
    >
    > For fun, replace the `throw new NotImplementedException();` by `throw new ArgumentNullException();` and see that the test `Fail`

#### Create an Inline Data-driven test

Let's implement the `Add(a, b)` method in `Calculator.cs`:

- Replace the line `"throw new NotImplementedException();"` with `"return a + b;"`

    ```cs
    public double Add(double a, double b)
    {
        return a + b;
    }
    ```

  > Note: Now the test `Add_TwoValues_ThrowAnException()` Fails

- In `CalculatorShould.cs` class, uncomment the attribute `// [Ignore]` for the `Add_TwoValues_ThrowAnException()` test

- When running the test, it is now `Skipped`

    > The `[Ignore]` attribute can be helpful, but should be used as a temporary approach while building the implementations and their tests. Skipped tests should not stay in code.

- Delete the method `Add_TwoValues_ThrowAnException()`

- Create a new method: `Add_TwoValues_Calculates()`:

    ```cs
    [TestMethod]
    [Description("Testing Calculator.Add() with multiple values.")]
    [Owner("Emmanuel")]
    [Priority(2)]
    [TestCategory("NormalValues")]
    [DataRow(6, 2, 8, DisplayName = "Test for (int) 6 + 2")]
    [DataRow(45.7, 12.89, 58.59, DisplayName = "Test for (double) 45.7 + 12.89")]
    [DataRow(double.MaxValue, 100, double.MaxValue, DisplayName = "Test for (double) MaxValue")]
    public void Add_TwoValues_Calculates(double a, double b, double expected)
    {
        // Arrange
        var sut = new Calculator();

        // Act
        var actual = sut.Add(a, b);

        // Assert
        Assert.AreEqual(expected, actual);
    }
    ```

- Run the Test and see few differences:

  ![Data-driven test](./img/Test2_DD-Test.png)

  > Notice:
  >
  > - The attributes `[Owner()]`, `[Priority()]` & `[TestCategory()]` generates `Traits` that can be used later to filter the tests, like in `dotnet test [] --filter "Priority=2"`.
  >
  > - The attribute `[Description()]` is added to the `TestContext` and we'll use it later
  >
  > - The `[DataRow()]` attribute allows us to pass data as an `object[]` type. Each attribute generates a test of the test method. The `DisplayName` argument allows to differentiate the tests instances in the display, as one or more can `Fail`
  >
  > - Notice that we also test the "edge" cases with the `double.MaxValue` value

#### Create the Tests before the implementation (or TDD)

This example is not an introduction to Test-Driven Development. It is just an awareness exercise.

We will implement a `Divide()` method for the `Calculator` class.

To do that, let's think about the tests that support the **behavior** we want to achieve:

- What should happen when we "divide `10` by `2`?" => we get `5`
- What should happen when we "divide `34.67` by `9.6`?" => we get `3.6114583333...`
- What should happen when we "divide `_anything_` by `0`?" => we decide we want to get `0`
- What should happen when we "divide `10` by `-5`?" => we get `-2`

We can create the tests for that:

  ```cs
  [TestMethod]
  [Description("Testing Calculator.Divide() with multiple values.")]
  [Owner("Emmanuel")]
  [Priority(2)]
  [TestCategory("NormalValues")]
  [DataRow(10, 2, 5, DisplayName = "Test for 10 / 5")]
  [DataRow(34.67, 9.6, 3.6114, DisplayName = "Test for 34.67 / 9.6")]
  [DataRow(56, 0, 0, DisplayName = "Test for 56 / 0")]
  [DataRow(10, -5, -2, DisplayName = "Test for 10 / -5")]
  public void Divide_TwoValues_Calculates(double a, double b, double expected)
  {
      // Arrange
      var sut = new Calculator();

      // Act
      var actual = sut.Divide(a, b);

      // Assert
      Assert.AreEqual(expected, actual);
  }
  ```

The build of `ConsoleAppTests` now fails. Because the `Calculator` class does not contain a definition for the `Divide()` method. Let's implement it:

- Create the member method in the `Calculator` class with the "normal" code one would put:

  ```cs
  public double Divide(double a, double b)
  {
      return a / b;
  }
  ```

- Run the Tests to see the results:

    ![Failing tests](./img/Test2_TDD-Step1.png)

- Fix the failing tests, based on their details:

  - Rounding errors

    ![Failing rounding](./img/Test2_TDD-Step2.png)

    For this test to pass, we have to set the `accuracy` of our test, by setting its `delta`.
    This change is done **in the test method**:

    - Add the `delta` argument to the Assert line: `Assert.AreEqual(expected, actual, 0.0001);`

    The accuracy should be adapted to the `Test data` provided to the test

  - Division by zero

    ![Failing divide by zero](./img/Test2_TDD-Step3.png)

    For this test to pass, we need to edit our `Divide()` method implementation:

    ```cs
    public double Divide(double a, double b)
    {
        if (b != 0)
        {
            return a / b;
        }

        // Arbitrary decision to handle division by 0
        return 0;
    }
    ```

- Implement the changes in code

- See all the tests `Pass`:

    ![Passing tests](./img/Test2_TDD-Step4.png)

  > Note:
  >
  > With Visual Studio Enterprise Edition:
  >
  > - You can see the code coverage of the classes (_Test / Analyze code coverage for All Tests_), in the `Code Coverage Results` view:
  >
  >    ![Passing tests](./img/Test2_TDD-Step5.png)
  >
  > - You can display colored lines and see which ones are covered by Unit Tests (blue ones being covered):
  >
  >   ![Passing tests](./img/Test2_TDD-Step6.png)
  >
  >   ![Passing tests](./img/Test2_TDD-Step7.png)
  >
  > - You can turn ON Live Unit Testing and see in real-time, in the code edited, the results of its associated tests:
  >
  >   ![Passing tests](./img/Test2_TDD-Step8.png)
  >

---

### 3. MSTestV2

In this exercise, we will use features from MSTestV2 that eases Unit Testing.

#### TestContext

The [TestContext Class](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.testcontext?view=visualstudiosdk-2022) is used to store information that is provided to unit tests.

We will use this Class to gather the `[Description]` attribute of a test and display it in the test message.

As this can be handy for all test classes, we will use the fact that unit tests are code:

- Create a new `Class` named `TestBase` in the `ConsoleAppTests` project

- Make it `public`

- Add a public property to it:

  ```cs
  public TestContext? TestContext { get; set; }
  ```

  > Note: You'll need the `using Microsoft.VisualStudio.TestTools.UnitTesting;` if Visual Studio didn't add it for you.

- Add to the `TestBase` class the following method:

  ```cs
  // Using Description Attribute
  protected void WriteDescription(Type type)
  {
      // Check for the required context
      if (TestContext == null) return;
      var testName = TestContext.TestName;
      if (testName == null) return;
      var method = type.GetMethod(testName);
      if (method == null) return;

      // Check for a description attribute
      var descAttribute = method.GetCustomAttribute<DescriptionAttribute>();
      if (descAttribute != null)
      {
          TestContext.WriteLine($"Test description: {descAttribute.Description}");
      }
  }
  ```

  > Note: You'll need the `using System;` & `using System.Reflection;` if Visual Studio didn't add it for you.

- Modify `CalculatorShould` class to inherit from `: TestBase`

- Add `WriteDescription(GetType());` at the beginning of the method `Add_TwoValues_Calculates()`. The method content should like this now:

  ```cs
  public void Add_TwoValues_Calculates(double a, double b, double expected)
  {
      WriteDescription(GetType());

      // Arrange
      var sut = new Calculator();

      // Act
      var actual = sut.Add(a, b);

      // Assert
      Assert.AreEqual(expected, actual);
  }
  ```

- `Test / Run All Tests`

- Look at the Test Detail Summary of one of the 3 tests and see the message, in the `Standard Output: TestContext Messages:` section:

  ![Passing tests](./img/Test3_Img1.png)

#### Initialization & Cleanup

Most Unit Tests frameworks provide features to reduce duplication of code.

To use this new method `WriteDescription()` in all our tests, we will use higher levels attributes from MSTest. It will allow to call the method in the code once, but all the tests methods will execute it.

MSTest has these attributes to Initialize at higher levels than the test methods:

Level | Attributes | Comment
---------|----------|----------
[Assembly](https://learn.microsoft.com/en-us/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests?view=vs-2022#assembly) | `[AssemblyInitialize()]` </p> `[AssemblyCleanup()]` | Linked to the Assembly lifecycle. See the documentation link for their declaration.
[Class](https://learn.microsoft.com/en-us/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests?view=vs-2022#class) | `[ClassInitialize()]` </p> `[ClassCleanup()]` | Linked to the Test Class lifecycle. See the documentation link for their declaration.
[Test](https://learn.microsoft.com/en-us/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests?view=vs-2022#test) | `[TestInitialize]` </p> `[TestCleanup]` | Linked to the Test lifecycle. See the documentation link for their declaration. Note the absence of `()` in the attribute declaration.

In the `CalculatorShould` class:

- Remove the line `WriteDescription(GetType());` added in previous step in the `Add_TwoValues_Calculates()` method

- Add this method in the class code:

  ```cs
  [TestInitialize]
  public void TestInitialize()
  {
      WriteDescription(GetType());
  }
  ```

- `Test / Run All Tests`

- See the `TestContext Messages` all showing the description from the test method attribute `[Description()]`

#### runsettings

Unit tests in Visual Studio can be configured by using a `.runsettings` file. For example, you can change the .NET version on which the tests are run, the directory for the test results, or the data that's collected during a test run. A common use of a `.runsettings` file is to customize [code coverage analysis](https://learn.microsoft.com/en-us/visualstudio/test/customizing-code-coverage-analysis?view=vs-2022).

Let's create a .runsettings file:

- In `ConsoleAppTests` project, add a new Item of type `XML File`, named `lab.runsettings`

- Add the following content in it:

```xml
<RunSettings>
  <TestRunParameters>
    <Parameter name="LabContext"
               value="H&amp;R Block BCA team" />
  </TestRunParameters>
</RunSettings>
```

- Save the file

- Set the `lab.runsettings` property `Copy to Output Directory` to `Copy if newer`

- There 4 ways to select a `.runsettings` file:

  - [Autodetect the run settings](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022#autodetect-the-run-settings-file)
  - [Manually select the run settings](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022#manually-select-the-run-settings-file)
  - [Set a build property](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022#set-a-build-property)
  - [dotnet test `--settings`](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022#specify-a-run-settings-file-from-the-command-line)

We'll use the manual selection:

- In `Test` menu select: `Configure Run Settings / Select Solution Wide runsettings File`

  ![Passing tests](./img/Test3_Img2.png)

- Select the file `lab.runsettings`

Let's use the parameter. It is available through the `TestContext.Properties["<Parameter name>"]`:

- In `TestBase.cs` add the following method:

  ```cs
  protected void WriteLabContext()
  {
      if (TestContext == null || string.IsNullOrEmpty((string?)TestContext.Properties["LabContext"])) return;
      TestContext.WriteLine($"Lab context: {TestContext.Properties["LabContext"]}");
  }
  ```

- In `CalculatorShould.cs`, add this line to the `TestInitialize()` method:

    ```cs
    WriteLabContext();
    ```

- `Test / Run All Tests` and check the new message:

  ![Passing tests](./img/Test3_Img3.png)

#### Create a Dynamic from SQL Data-driven test

We will create a SQL server table with the test data and use it for our unit tests.

- Open the `SQL Server Object Explorer` from Visual Studio View menu:

  ![Passing tests](./img/Test3_Img4.png)

- You should have `MSSQLLocalDB` instance. If you don't skip this exercise:

  ![Passing tests](./img/Test3_Img5.png)

- If you don't have a Database, create one (suggested name: `Sandbox`):

  ![Passing tests](./img/Test3_Img6.png)

- In the database, create a `New Query` and paste this T-SQL code in it:

```sql
CREATE TABLE [dbo].[CalculatorDivideTestsData](
 [Id] [int] NOT NULL PRIMARY KEY IDENTITY(1,1),
 [Arg1Value] [float] NULL,
 [Arg2Value] [float] NULL,
 [ExpectedValue] [float] NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (10, 2, 5);

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (34.67, 9.6, 3.6114);

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (56, 0, 0);

INSERT INTO [dbo].[CalculatorDivideTestsData] ( [Arg1Value], [Arg2Value], [ExpectedValue])
VALUES (10, -5, -2);
```

- Execute the command (Check you're connected to the `Sandbox` database):

  ![Passing tests](./img/Test3_Img7.png)

- Open the table (Right-click / View Data) to see the data:

  ![Passing tests](./img/Test3_Img8.png)

Now that we have the test data we can use it:

- First, check we didn't break anything - That's one benefit of Unit Tests - and `Test / Run All Tests`

- There is a trick: [**.NET Core does not support the DataSource attribute**](https://learn.microsoft.com/en-us/visualstudio/test/how-to-create-a-data-driven-unit-test?view=vs-2022#add-a-testcontext-to-the-test-class), so we will access the SQL data through a SQL client

- in `lab.runsettings` file, add the parameter for the connection string (you get the connection string from the Database property in the SQL Server Object explorer view):

  ```xml
  <Parameter name="ConnectionString"
              value="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Sandbox;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" />
  <Parameter name="DivideTestDataTableName"
              value="dbo.CalculatorDivideTestsData" />
  ```

- Add the NuGet packages: `System.Data.SqlClient`

- In the `TestBase.cs` class, add the protected method `LoadTestDataFromSql` with this code - it allows to fetch data from SQL Server:

  ```cs
  protected DataTable LoadTestDataFromSql(string connection, string query)
  {
      var dataTable = new DataTable();

      try
      {
          using var conn = new SqlConnection(connection);
          using var cmd = new SqlCommand(query, conn);
          using var da = new SqlDataAdapter(cmd);
          da.Fill(dataTable);
      }
      catch (SqlException ex)
      {
          TestContext?.WriteLine("Exception occurred in LoadTestDataFromSql: {0}", ex);
      }

      return dataTable;
  }
  ```

  > Note: You'll need the `using System.Data;` & `using System.Data.SqlClient;` if Visual Studio didn't add it for you.

  - Save the changes

- In the `CalculatorShould.cs` class:

  - Remove the `[DataRow()]` attributes from the `Divide_TwoValues_Calculates()` test method
  - Replace `Divide_TwoValues_Calculates()` method content for:

    ```cs
    public void Divide_TwoValues_Calculates()
    {
        // Arrange
        var sut = new Calculator();

        var sqlQuery = "SELECT * FROM ";
        sqlQuery += TestContext!.Properties["DivideTestDataTableName"]!.ToString();
        var sqlConnString = TestContext!.Properties["ConnectionString"]!.ToString();
        
        if (sqlConnString != null)
        {
            var dataTable = LoadTestDataFromSql(sqlConnString, sqlQuery);

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    // Get the test values
                    var a = Convert.ToDouble(row["Arg1Value"]);
                    var b = Convert.ToDouble(row["Arg2Value"]);
                    var expected = Convert.ToDouble(row["ExpectedValue"]);

                    // Act
                    TestContext.WriteLine($"Testing: {a} / {b} = {expected}");
                    var actual = sut.Divide(a, b);

                    // Assert
                    Assert.AreEqual(expected, actual, 0.0001);
                }
            }
        }
    }
    ```

  - `Test / Run All Tests`

  - Check the Results:

  ![Passing tests](./img/Test3_Img9.png)

With this technique, the tests can be automated, pulling different data-sets through the application release process.

#### `dotnet test` CLI

To leverage the Unit Tests in pipelines and other automation tools, the command-line interface is the base to use.

- Open a Terminal in the `ConsoleAppTests` directory

- Execute: `dotnet test`

  - Check that the tests fail (`- Failed:     1, Passed:     4`)

  > That is because our tests for `Divide_TwoValues_Calculates()` requires parameters from the `lab.runsettings`

- Execute: `dotnet test .\ConsoleAppTests.csproj --settings .\lab.runsettings --logger "console;verbosity=detailed"`

  - Check all the tests `Pass`:

    ![Passing tests](./img/Test3_Img10.png)

There is more options, like `--filter`, `--runtime`, `--environment` etc.

See the .NET test driver [reference](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test) for more information.

---

## References

### Lab creation support material

To create this Lab for H&R Block's BAC team's DevOps continuous quality workshop, the following sources were used:

- [Testing in .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/)
- [Unit testing C# with MSTest and .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [First look at testing tools in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/test/improve-code-quality?view=vs-2022)
- [Pluralsight | Implementing C# 9 Unit Testing Using Visual Studio 2019 and .NET 5](https://app.pluralsight.com/library/courses/basic-unit-testing-csharp-developers/table-of-contents)
- [Pluralsight | Testing .NET Code with xUnit.net 2: Getting Started](https://app.pluralsight.com/library/courses/dotnet-core-testing-code-xunit-dotnet-getting-started/table-of-contents)
- [Tim Corey | Intro to Unit Testing in C# using xUnit](https://youtu.be/ub3P8c87cwk)

### Assert reference

The use of "Asserts" is key to Unit Testing and presents many methods.

They are of 3 types:

1. Positive (like `AreEqual`, `AreSame`...)
2. Negative (like `AreNotEqual`, `IsFalse`...)
3. Other (like `Fail`, `Inconclusive`)

One can leverage these main "Asserts" classes:

- [`Assert`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.assert?view=visualstudiosdk-2022) to test various conditions.

- [`CollectionAssert`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.collectionassert?view=visualstudiosdk-2022), to test various conditions associated with collections of objects.

- [`StringAssert`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.stringassert?view=visualstudiosdk-2022) to compare and examine strings.

### Unit Test Attributes reference

As we saw, Attributes (or decorators) are widely used in MSTest to configure and improve the Unit Tests.

Few attributes were not covered in the lab, and worth mentioning:

- [DeploymentItem](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.deploymentitemattribute?view=visualstudiosdk-2022): Used to specify deployment item (file or directory) for per-test deployment.
- [Timeout](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.timeoutattribute?view=visualstudiosdk-2022): used to specify the timeout of a unit test.
- [DataSource](https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.datasourceattribute?view=visualstudiosdk-2022): Specifies connection string, table name and row access method for data driven testing.

A good reference to learn more on attributes is [Use the MSTest framework in unit tests](https://learn.microsoft.com/en-us/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests?view=vs-2022)

### Dependencies & Mocks, Microsoft Fakes with Stubs & Shims

All these concepts and techniques require specific labs and sessions.

Here are few starting points:

- [Microsoft Fakes](https://learn.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes?view=vs-2022&tabs=csharp): Allows to create Stubs and Shims:

  - A [stub](https://learn.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes?view=vs-2022&tabs=csharp#get-started-with-stubs) replaces a class with a small substitute that implements the same interface.
  - A [shim](https://learn.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes?view=vs-2022&tabs=csharp#get-started-with-shims) modifies the compiled code of your application at runtime so that instead of making a specified method call, it runs the shim code that your test provides.

- [Unit tests for generic methods](https://learn.microsoft.com/en-us/visualstudio/test/unit-tests-for-generic-methods?view=vs-2022)

- Mocking dependencies: the most popular Framework to do this in .NET is [Moq](https://github.com/moq/moq4)

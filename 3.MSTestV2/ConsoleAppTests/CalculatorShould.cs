using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp.Classes;
using System;
using System.Data;

namespace ConsoleAppTests
{
    [TestClass]
    public class CalculatorShould : TestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            WriteLabContext();
            WriteDescription(GetType());
        }

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

        [TestMethod]
        [Description("Testing Calculator.Divide() with multiple values.")]
        [Owner("Emmanuel")]
        [Priority(2)]
        [TestCategory("NormalValues")]
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
    }
}

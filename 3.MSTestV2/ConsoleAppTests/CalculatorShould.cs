using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleApp.Classes;

namespace ConsoleAppTests
{
    [TestClass]
    public class CalculatorShould
    {
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
            Assert.AreEqual(expected, actual, 0.0001);
        }
    }
}

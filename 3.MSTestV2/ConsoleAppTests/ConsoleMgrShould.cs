using System;
using System.IO;
using ConsoleApp.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleAppTests;

[TestClass]
public class ConsoleMgrShould
{
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
        var actual = sw.ToString().Trim();

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
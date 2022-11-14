using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace ConsoleAppTests
{
    public class TestBase
    {
        public TestContext? TestContext { get; set; }
        
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
    }
}

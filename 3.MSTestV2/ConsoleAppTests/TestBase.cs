using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Data;
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

        protected void WriteLabContext()
        {
            if (TestContext == null || string.IsNullOrEmpty((string?)TestContext.Properties["LabContext"])) return;
            TestContext.WriteLine($"Lab context: {TestContext.Properties["LabContext"]}");
        }

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
    }
}

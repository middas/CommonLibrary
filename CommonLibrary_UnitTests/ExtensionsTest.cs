using CommonLibrary.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace CommonLibrary_UnitTests
{
    
    
    /// <summary>
    ///This is a test class for ExtensionsTest and is intended
    ///to contain all ExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetDelimitedString
        ///</summary>
        [TestMethod()]
        public void GetDelimitedStringTest()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("Column1", typeof(string)));
            table.Columns.Add(new DataColumn("Column2", typeof(int)));
            table.Columns.Add(new DataColumn("Column3", typeof(DateTime)));

            DataRow row = table.NewRow();
            table.Rows.Add(row);

            row["Column1"] = "Cell1";
            row["Column2"] = 1;
            row["Column3"] = DateTime.MinValue;

            row = table.NewRow();
            table.Rows.Add(row);

            row["Column1"] = "Cell2";
            row["Column2"] = 2;
            row["Column3"] = DateTime.MaxValue;

            DataView source = table.DefaultView; // TODO: Initialize to an appropriate value
            string rowDelimiter = "\r\n"; // TODO: Initialize to an appropriate value
            string columnDelimiter = ","; // TODO: Initialize to an appropriate value
            bool includeHeaders = true; // TODO: Initialize to an appropriate value
            string expected = "Column1,Column2,Column3\r\nCell1,1,1/1/0001 12:00:00 AM\r\nCell2,2,12/31/9999 11:59:59 PM"; // TODO: Initialize to an appropriate value
            string actual;
            actual = Extensions.GetDelimitedString(source, rowDelimiter, columnDelimiter, includeHeaders);
            Assert.AreEqual(expected, actual);
        }
    }
}

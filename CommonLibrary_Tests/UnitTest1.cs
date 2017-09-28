using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLibrary.Utilities;

namespace CommonLibrary_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DllInjector i = new DllInjector();
            string error;
            bool success = i.InjectDll("notepad", @"C:\Users\JustinS\Dev\Common\CommonLibrary\CommonLibrary_Tests\bin\Debug\DLLInjectorHelper.dll", out error);

            Assert.AreEqual(true, success);
        }
    }
}

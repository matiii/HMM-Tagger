using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger.HMM;

namespace Tagger.Tests
{
    [TestClass]
    public class LoaderDataTest
    {
        private const string path = "..\\..\\..\\Data";

        [TestMethod]
        public void ProcessWithoutExceptions()
        {
            var loader = new LoaderData(path);
            loader.Process();
        }
    }
}

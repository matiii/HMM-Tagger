using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger.HMM;

namespace Tagger.Tests
{
    [TestClass]
    public class StatisticUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] sentences = new[] {"Ala/R ma/C kota/R"};
            var s = new Statistic(sentences, 0.34, 0.33, 0.33);
            s.Process();
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger.HMM;

namespace Tagger.Tests
{
    [TestClass]
    public class ViterbiAlgorithmTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] sentences = new[] { "Ala/RAR ma/CAC kota/RAR" , "Kot/RAR ma/CAC Ale/RAR", "Lesio/RAR jest/CAC zajebisty/PAP", "Zajebisty/PAP jest/CAC kot/RAR", "Ala/RAR ma/CAC kota/RAR i/SAS Lesia/RAR"};
            var s = new Statistic(sentences, 0.34, 0.33, 0.33);
            s.Process();

            var v = new ViterbiAlgorithm(s);
            string[] output = v.Process(new[] {"Zajebisty", "jest", "Lesio", "i", "kot", "i", "Ala"});

            Assert.AreEqual("PAP", output[0]);
            Assert.AreEqual("CAC", output[1]);
            Assert.AreEqual("RAR", output[2]);
            Assert.AreEqual("SAS", output[3]);
            Assert.AreEqual("RAR", output[4]);
            Assert.AreEqual("SAS", output[5]);
            Assert.AreEqual("RAR", output[6]);
        }
    }
}
 
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger.HMM;

namespace Tagger.Tests
{
    [TestClass]
    public class AllProcessTest
    {
        private const string path = "..\\..\\..\\Data";

        [TestMethod]
        public void Test()
        {
            var loader = new LoaderData(path);
            loader.Process();

            var s = new Statistic(loader.Sentences.ToArray(), 0.34, 0.33, 0.33);
            s.Process();

            var v = new ViterbiAlgorithm(s);
            string[] output = v.Process(new[] { "Życie", "studenta", "w", "Polsce", "jest", "bardzo", "trudne", "bez", "pieniędzy", "." });

            //Assert.AreEqual("prep", output[0]);
            //Assert.AreEqual("burk", output[1]);
            //Assert.AreEqual("RAR", output[2]);
            //Assert.AreEqual("SAS", output[3]);
            //Assert.AreEqual("RAR", output[4]);
            //Assert.AreEqual("SAS", output[5]);
            //Assert.AreEqual("RAR", output[6]);
            //Assert.AreEqual("RAR", output[6]);
            //Assert.AreEqual("RAR", output[6]);
            //Assert.AreEqual("RAR", output[6]);
        }
    }
}

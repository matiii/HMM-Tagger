using System;
using System.Collections.Generic;
using System.Linq;
using Tagger.HMM.Business;

namespace Tagger.HMM
{
    public class ViterbiAlgorithm
    {
        private readonly ITagStatistic tagStatistic;
        private readonly IDictionary<int, Tuple<string, double>> pi = new Dictionary<int, Tuple<string, double>> { { 0, new Tuple<string, double>("*,*", 1.0) } };

        public ViterbiAlgorithm(ITagStatistic tagStatistic)
        {
            this.tagStatistic = tagStatistic;
        }


        public string[] Process(string[] sentence)
        {
            int k = sentence.Length;
            var n = new string[k];

            for (int i = 1; i <= k; i++)
            {
                Tuple<string, double> maxPi = pi[i - 1];
                string tags = maxPi.Item1;

                var tag = GetMaxTag(tags, sentence[i-1]);
                pi.Add(i, new Tuple<string, double>(tags.Split(',')[1] + ","  + tag.Key, tag.Value));
                n[i-1] = tag.Key;
            }

            return n;
        }


        private KeyValuePair<string, double> GetMaxTag(string startTag, string word)
        {
            var d = new Dictionary<string, double>();

            foreach (string tag in tagStatistic.GetAllTags)
            {
                d.Add(tag, tagStatistic.Q(startTag + "," + tag));
                d[tag] *= tagStatistic.E(tag + "|" + word.ToLower());
            }

            return d.OrderByDescending(k => k.Value).First();
        }

        private string GetTag(string tag1, string tag2, string tag3)
        {
            return String.Format("{0},{1},{2}", tag1, tag2, tag3);
        }

        private string GetTag(int tag1, string tag2, string tag3)
        {
            return GetTag(tag1.ToString(), tag2, tag3);
        }

        private string GetTag(int tag1, string tag)
        {
            string[] t = GetTags(tag);
            return GetTag(tag1, t[0], t[1]);
        }

        private string[] GetTags(string tag)
        {
            return tag.Split(',');
        }
    }
}

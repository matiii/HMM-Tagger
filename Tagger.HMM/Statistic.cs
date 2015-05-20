using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tagger.HMM.Business;

namespace Tagger.HMM
{
    public class ContainerStatistic
    {
        public IDictionary<string, int> Tags { get; set; }    
        public IDictionary<string, int> ETags { get; set; }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public double L3 { get; set; }
    }


    public class Statistic : ITagStatistic
    {
        private readonly string[] sentences;
        private double l1;
        private double l2;
        private double l3;


        private IDictionary<string, int> tags = new Dictionary<string, int>();

        /// <summary>
        /// tag|word
        /// </summary>
        private IDictionary<string, int> etags = new Dictionary<string, int>();

        public Statistic(string[] sentences, double l1, double l2, double l3)
        {
            double sum = l1 + l2 + l3;
            if (sum != 1.0)
                throw new ArgumentException("Sum of all lamdas need to equal 1.0. Already equal " + sum);

            this.sentences = sentences;
            this.l1 = l1;
            this.l2 = l2;
            this.l3 = l3;
        }
        public async Task ProcessAsync()
        {
            await Task.Run(() => Process());
        }
        public void Process()
        {
            foreach (string sentence in sentences)
            {
                string[] wordsWithTags = sentence.Split(' ');
                string[] tags = GetWordsAndTags(wordsWithTags)[1];
                GenerateTags(tags.ToList());
            }
        }


        /// <summary>
        /// Get q(Vt|Dt,JJ), like Q("Dt,JJ,Vt")
        /// </summary>
        /// <param name="tags">tag1,tag2,tag3</param>
        /// <returns></returns>
        public double Q(string tags)
        {
            string[] t = tags.Split(',');

            double r1 = 0.0;
            double r2 = 0.0;
            double r3 = 0.0;

            string tagTmp = t[1] + "," + t[2];

            if (this.tags.ContainsKey(tagTmp) && this.tags.ContainsKey(tags))
            {
                r1 = l1 * this.tags[tags] / this.tags[t[1] + "," + t[2]];
            }

            if (this.tags.ContainsKey(tagTmp) && this.tags.ContainsKey(t[1]))
            {
                r2 = l2 * this.tags[tagTmp] / this.tags[t[1]];
            }

            if (this.tags.ContainsKey(t[2]))
            {
                r3 = l3 * this.tags[t[2]] / GetCountAllTags;
            }

            return r1 + r2 + r3;
        }


        /// <summary>
        /// Get e(base|Vt), like E("Vt|base")
        /// </summary>
        /// <param name="value">tag|word</param>
        /// <returns></returns>
        public double E(string value)
        {
            string[] t = value.Split('|');

            if (!etags.ContainsKey(value) || !tags.ContainsKey(t[0]))
            {
                return 0.0000000000000000000001;
            }

            return etags[value] / (double)tags[t[0]];
        }

        public string[] GetAllTags
        {
            get { return tags.Keys.Where(k => !k.Contains(",")).ToArray(); }
        }

        private int GetCountAllTags
        {
            get { return tags.Values.Sum(); }
        }
        private void GenerateTags(IList<string> tags)
        {
            if (tags.Last() != "STOP")
            {
                tags.Add("STOP");
                AddToTagsAndEtags("", "STOP");
            }

            for (int i = 0; i < tags.Count; i++)
            {
                for (int k = i; k < i + 2; k++)
                {
                    string tag = tags[i];
                    int j = i - 1;

                    while (j + 2 >= k)
                    {
                        if (j < 0)
                            tag = "*," + tag;
                        else
                            tag = tags[j] + "," + tag;

                        j--;
                    }

                    AddToDictionary(tag, this.tags);
                }
            }
        }
        private void AddToTagsAndEtags(string word, string tag)
        {
            AddToDictionary(tag, tags);
            string e = tag + "|" + word.ToLower();
            AddToDictionary(e, etags);
        }
        private void AddToDictionary(string key, IDictionary<string, int> dict)
        {
            if (!dict.Any(t => t.Key == key))
                dict.Add(key, 1);
            else
                dict[key]++;
        }
        private string[][] GetWordsAndTags(string[] wordsWithTags)
        {
            var tab = new string[2][];
            tab[0] = new string[wordsWithTags.Length];
            tab[1] = new string[wordsWithTags.Length];

            for (int i = 0; i < wordsWithTags.Length; i++)
            {
                string[] data = wordsWithTags[i].Split('/');

                if (data.Length == 2)
                {
                    tab[0][i] = data[0]; //word
                    tab[1][i] = data[1]; //tag

                    AddToTagsAndEtags(data[0], data[1]);
                }
                
            }

            return tab;
        }

        public bool Save(string path)
        {
            bool result = true;
            try
            {
                string json = JsonConvert.SerializeObject(new ContainerStatistic {Tags = tags, ETags = etags, L1 = l1, L2 = l2, L3 = l3});

                using (var sw = new StreamWriter(path))
                {
                    sw.Write(json);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public bool Load(string path)
        {
            bool result = true;

            try
            {
                using (var sr = new StreamReader(path))
                {
                    string json = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<ContainerStatistic>(json);

                    l1 = data.L1;
                    l2 = data.L2;
                    l3 = data.L3;
                    tags = data.Tags;
                    etags = data.ETags;
                }
                
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}

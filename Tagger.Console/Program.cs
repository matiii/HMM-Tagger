using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tagger.HMM;
using SC = System.Console;


namespace Tagger.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Compute(args[0], args[0]);
            }
            else if (args.Length == 2)
            {
                Compute(args[0], args[1]);
            }
            else
            {
                SC.WriteLine("[file input] [file output - optional]");
            }

            SC.WriteLine("Press any key to exit");
            SC.ReadKey();
        }

        static void Compute(string input, string output)
        {
            const string save = "data.json";
            string data = FileManager.Load(input);
            data = data.Replace("\r", " ").Replace("\n", " ");

            var s = new Statistic(null, 0.34, 0.33, 0.33);
            s.Load(save);

            var v = new ViterbiAlgorithm(s);

            string[] words = LoadWordsFromString(data);

            string[] tags = v.Process(words);

            IEnumerable<string> o = words.Zip(tags, (w, t) => w + "/" + t);

            FileManager.Save(GetStringFromArray(o), output);

            SC.WriteLine("Done");
        }


        static string GetStringFromArray(IEnumerable<string> input)
        {
            var sb = new StringBuilder();

            foreach (var s in input  )
            {
                sb.Append(s);
                sb.Append(" ");
            }

            return sb.ToString();
        }

        static string[] LoadWordsFromString(string words)
        {
            string[] data = words.Split(' ');
            var w = new List<string>();


            foreach (var word in data)
            {
                if (word.Contains(","))
                {
                    w.Add(word.Replace(",", ""));
                    w.Add(",");
                }
                else if (word.Contains("."))
                {
                    w.Add(word.Replace(".", ""));
                    w.Add(".");
                }
                else if (word.Contains("?"))
                {
                    w.Add(word.Replace("?", ""));
                    w.Add("?");
                }
                else if (word.Contains("!"))
                {
                    w.Add(word.Replace("!", ""));
                    w.Add("!");
                }
                else
                {
                    if (word != "")
                        w.Add(word);
                }
            }


            return w.ToArray();
        }
    }
}

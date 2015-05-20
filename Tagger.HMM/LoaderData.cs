using System;
using System.Collections.Generic;
using System.IO;

namespace Tagger.HMM
{
    public class LoaderData
    {
        private const string fileName = "ann_morphosyntax.xml";
        private readonly string path;
        private readonly Parser parser = new Parser();

        public LoaderData(string path)
        {
            this.path = path;
        }


        public IList<string> Sentences
        {
            get { return parser.Sentences; }
        }

        public void Process()
        {
            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                string file = Path.Combine(dir, fileName);

                if (File.Exists(file))
                {
                    using (var sr = new StreamReader(file))
                    {
                        string data = sr.ReadToEnd();

                        if (!String.IsNullOrEmpty(data))
                        {
                            parser.Parse(data);
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Tagger.HMM
{
    public class Parser
    {
        private readonly List<string> sentences = new List<string>();

        public IList<string> Sentences
        {
            get { return sentences; }
        }


        public void Parse(string text)
        {
            using (var reader = XmlReader.Create(new StringReader(text)))
            {
                while (reader.Read())
                {
                    if (!String.IsNullOrEmpty(reader.Name) && reader.Name == "s")
                        ReadNewSentence(reader);
                }
            }
        }

        private void ReadNewSentence(XmlReader reader)
        {
            var sentence = new StringBuilder();

            while (reader.Read())
            {
                if (reader.Name == "f")
                {
                    string attr = reader.GetAttribute("name");
                    if (!String.IsNullOrEmpty(attr))
                    {
                        if (attr == "orth")
                        {
                            string value = GetFromString(reader);

                            sentence.Append(value);
                        }
                        else if (attr == "interpretation")
                        {
                            string value = GetFromString(reader);
                            string tag = GetTag(value);
                            sentence.Append("/" + tag + " ");
                        }
                    }
                }
                else if (reader.Name == "s" && reader.NodeType == XmlNodeType.EndElement)
                    break;
            }

            sentences.Add(sentence.ToString());
        }


        private string GetFromString(XmlReader reader)
        {
            while (reader.NodeType != XmlNodeType.Text)
                reader.Read();
            return reader.Value;
        }
        protected virtual string GetTag(string disamb)
        {
            string[] data = disamb.Split(':');

            if (data.Length < 2)
                return "Undefined";

            return data[1];
        }
    }
}

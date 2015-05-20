using System.IO;
using System.Text;

namespace Tagger.Console
{
    class FileManager
    {
        public static string Load(string path)
        {
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public static void Save(string data, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(data);
            }
        }
    }
}

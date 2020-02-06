using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GetAllReadMoreUrls
{
    class Program
    {
        static void Main(string[] args)
        {

            var allFilesToProcess = new List<string>();
            var allReadMoreUrl = new HashSet<string>();

            {

                var path = @"R:\Labster\Simulations\Simulations\";

                DirectoryInfo directoryEngine = new DirectoryInfo(path);
                FileInfo[] filesEngine = directoryEngine.GetFiles("*.xml");
                foreach (var e in filesEngine)
                {
                    if (e.Name.Contains(@"Engine_") == true)
                    {
                        allFilesToProcess.Add(path + e.Name);
                    }
                }

                DirectoryInfo directoryQuizzblock = new DirectoryInfo(path + @"QuizBlocks\");
                FileInfo[] filesQuizblock = directoryQuizzblock.GetFiles("*.xml");
                foreach (var q in filesQuizblock)
                {
                    if (q.Name.Contains(@"QuizBlocks_") == true)
                    {
                        allFilesToProcess.Add(path + @"QuizBlocks\" + q.Name);
                    }
                }
            }


            foreach (var path in allFilesToProcess)
            {

                var text = File.ReadAllText(path);
                XDocument document = null;

                try
                {
                    document = XDocument.Parse(text, LoadOptions.None);
                }
                catch (XmlException exception)
                {
                    Console.WriteLine("Error: could not parse the XML, {0}", exception.Message);
                    Environment.Exit(1);
                }

                foreach (var tmp in document.Descendants())
                {
                    if (tmp.HasAttributes == true)
                    {
                        foreach (var attr in tmp.Attributes())
                        {
                            if (attr.Name.LocalName == "ReadMoreUrl")
                            {
                                allReadMoreUrl.Add(attr.Value);
                            }
                        }
                    }
                }

            }

            TextWriter tw = new StreamWriter("SavedList.txt");

            foreach (String s in allReadMoreUrl)
                tw.WriteLine(s);

            tw.Close();

        }
    }
}

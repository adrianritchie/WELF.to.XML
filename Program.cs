using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WELF.to.XML
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = getFiles();
            Parallel.ForEach(files, convertFile);

            Console.WriteLine("All completed, press any key to continue.");
            Console.ReadKey();
        }

        private static void convertFile(string inFile)
        {
            Console.WriteLine("Starting: {0}", Path.GetFileName(inFile));
            var outFile = Path.ChangeExtension(inFile, "xml");
            if (File.Exists(outFile))
                File.Delete(outFile);

            using (var inStream = new StreamReader(File.OpenRead(inFile)))
            using (var outStream = new StreamWriter(File.OpenWrite(outFile)))
            {
                outStream.AutoFlush = true;

                outStream.WriteLine("<?xml version=\"1.0\"?>");
                outStream.WriteLine("<log>");
                while (!inStream.EndOfStream)
                {
                    var inLine = inStream.ReadLine();
                    var outLine = convertLine(inLine);
                    outStream.WriteLine(outLine);
                }
                outStream.WriteLine("</log>");
            }

            Console.WriteLine("Finished: {0}", Path.GetFileName(inFile));
        }

        private static string convertLine(string inLine)
        {
            var line = new StringBuilder("<entry>");
            string element = string.Empty, value = string.Empty;
            var inQuotes = false;
            var inElement = true;

            foreach (var c in inLine)
            {
                if (Char.IsWhiteSpace(c) && !inQuotes)
                {
                    line.AppendFormat("<{0}>{1}</{0}>", element, WebUtility.HtmlEncode(value));
                    inElement = true;
                    element = string.Empty;
                    value = string.Empty;
                    continue;
                }

                if (c == '"' && (inQuotes || value == string.Empty))
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == '=' && inElement)
                {
                    inElement = false;
                    continue;
                }

                if (inElement)
                    element += c;
                else
                    value += c;
            }
            line.AppendFormat("<{0}>{1}</{0}>", element, WebUtility.HtmlEncode(value));
            line.Append("</entry>");
            return line.ToString();
        }

        private static IEnumerable<string> getFiles()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Directory.EnumerateFiles(path, "*.log");
        }
    }
}
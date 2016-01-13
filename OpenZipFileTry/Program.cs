using System;
using System.Linq;
using Ionic.Zip;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Configuration;

namespace OpenZipFileTry
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] listOfFiles;
            Stopwatch sWatch = new Stopwatch();
            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.IgnoreWhitespace = true;
            long counter = 0;

            string folderPath = ConfigurationManager.AppSettings["folderPath"];
            bool zipped = Convert.ToBoolean(ConfigurationManager.AppSettings["zipped"]);
            string elementToCount = ConfigurationManager.AppSettings["elementToCount"];
            string logPath = ConfigurationManager.AppSettings["logPath"];

            if (zipped)
            {
                listOfFiles = Directory.GetFiles(folderPath, "*.zip", SearchOption.TopDirectoryOnly);
                foreach (var item in listOfFiles)
                {
                    ZipFile file = new ZipFile(item);
                    var stream = file.Entries.ElementAt(0).OpenReader(); // zalozenie ze jest tylko jeden plik w zipie
                    using (stream)
                    using (XmlReader reader = XmlReader.Create(stream, xmlSettings))
                    {
                        sWatch.Restart();
                        try
                        {
                            while (reader.Read())
                            {
                                if (reader.IsStartElement())
                                {
                                    if (reader.Name == elementToCount)
                                    {
                                        counter++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(logPath + "exceptions.txt", ex.Message + " " + item + Environment.NewLine);
                            File.AppendAllText(logPath + "exceptions.txt", "The elements were NOT COUNTED to the end of this file.");
                        }
                        sWatch.Stop();
                        File.AppendAllText(logPath + "testLog.txt", "File " + item.ToString() + " contains "
                            + counter.ToString("N0") + " elements." + Environment.NewLine);
                        File.AppendAllText(logPath + "testLog.txt", "Counted in " + sWatch.Elapsed.TotalSeconds + " seconds." + Environment.NewLine);
                        counter = 0;
                    }
                }
            }
            else
            {
                listOfFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var item in listOfFiles)
                {
                    using (XmlReader reader = XmlReader.Create(item, xmlSettings))
                    {
                        sWatch.Restart();
                        try
                        {
                            while (reader.Read())
                            {
                                if (reader.IsStartElement())
                                {
                                    if (reader.Name == elementToCount)
                                    {
                                        counter++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(logPath + "exceptions.txt", ex.Message + " " + item + Environment.NewLine);
                            File.AppendAllText(logPath + "exceptions.txt", "The elements were NOT COUNTED to the end of this file.");
                            counter = 0;
                        }
                        sWatch.Stop();
                        File.AppendAllText(logPath + "testLog.txt", "File " + item.ToString() + " contains "
                            + counter.ToString("N0") + " elements." + Environment.NewLine);
                        File.AppendAllText(logPath + "testLog.txt", "Counted in " + sWatch.Elapsed.TotalSeconds + " seconds." + Environment.NewLine);
                        counter = 0;
                    }
                }
            }
        }
    }
}

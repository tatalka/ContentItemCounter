using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace ContentItemCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = ConfigurationManager.AppSettings["folderPath"];
            string logPath = ConfigurationManager.AppSettings["logPath"];
            string elementToCount = ConfigurationManager.AppSettings["elementToCount"];
            string[] fileList =  Directory.GetFiles(filePath);
            long counter = 0L;
            Stopwatch stopwatch = new Stopwatch();
            char[] buff = new char[200];
            foreach (var item in fileList)
            {
                // tym text readerem Rafal wyciagnal kawalek ktory mnie interesowal
                //using (TextReader reader = File.OpenText(item))
                //{
                //    for (int i = 0; i < 12; i++)
                //    {
                //        reader.ReadLine();
                //    }
                //    for(int i=0;i< 70589200; i++)
                //    {
                //        reader.Read();
                //    }
                //    reader.Read(buff, 0, 200);
                //}
                //string text = new string(buff);

                using (XmlReader reader = XmlReader.Create(item))
                {
                    stopwatch.Restart();
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
                        File.AppendAllText(logPath + "exceptions.txt", "The elements were NOT COUNTED to the end of file.");
                    }
                    stopwatch.Stop();
                    File.AppendAllText(logPath + "testLog.txt", "File " + item.ToString() + " contains "
                        + counter.ToString("N0") + " elements." + Environment.NewLine);
                    File.AppendAllText(logPath + "testLog.txt", "Counted in " + stopwatch.Elapsed.TotalSeconds + " seconds." + Environment.NewLine);
                    counter = 0;
                }
            }
        }
    }
}

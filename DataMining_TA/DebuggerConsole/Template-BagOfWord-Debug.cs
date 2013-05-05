using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Extension;
using Clustering;
using Clustering.Initialization;
using TFIDF;

namespace DebuggerConsole
{
    class Template_BagOfWord_Debug
    {
        public static void run()
        {
            List<Variables> inputVariables = new List<Variables>();
            List<Variables> outputVariables = new List<Variables>();
            List<Row> listRow = new List<Row>();

            //Read Vocab
            FileStream fileStream = null;
            StreamReader streamReader = null;
            string base_url = @"E:\5109100153 - Eric\tc\";
            try
            {
                fileStream = new FileStream(base_url + @"vocab.newsgroup.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);
                while (true)
                {
                    string line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;

                    char[] separator = new char[1] { ' ' };
                    string[] linex = line.Split(separator);
                    Variables news = new Variables(linex[0]);

                    try
                    {
                        news.TermFrequency = Convert.ToInt32(linex[1]);
                        news.RowFrequency = Convert.ToInt32(linex[2]);
                    }
                    catch (Exception ex)
                    {
 
                    }
                    inputVariables.Add(news);
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
            Console.WriteLine("Finish Read Vocab!");

            //Read Docs & output
            try
            {
                fileStream = new FileStream(base_url + @"doc.newsgroup.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);
                string line = streamReader.ReadLine();
                int numOutput = Convert.ToInt32(line);
                for (int i = 0; i < numOutput; i++)
                {
                    Variables news = new Variables();
                    outputVariables.Add(news);
                }

                while (true)
                {
                    line = streamReader.ReadLine();
                    Row newRow = new Row(line);
                    for (int i = 0; i < numOutput; i++)
                    {
                        line = streamReader.ReadLine();
                        if (line != "null")
                        {
                            Cell newCell = new Cell(outputVariables[i], line);
                            newRow.OutputValue.Add(outputVariables[i], newCell);
                        }
                    }
                    listRow.Add(newRow);
                    if (string.IsNullOrEmpty(line))
                        break;
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
            Console.WriteLine("Finish Read doc!");

            try
            {
                fileStream = new FileStream(base_url + @"docword.newsgroup.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);

                while (true)
                {
                    string line = streamReader.ReadLine();
                    //if (line == null) continue;

                    if (string.IsNullOrEmpty(line))
                        break;

                    char[] separator = new char[1] { ',' };

                    string[] linex = line.Split(separator);
                    int docId = Convert.ToInt32(linex[0]);
                    int wordId = Convert.ToInt32(linex[1]);
                    int countWord = Convert.ToInt32(linex[2]);
                    //Console.WriteLine(docId.ToString() + " " + wordId.ToString() + " " + countWord.ToString());
                    //System.Threading.Thread.Sleep(1000);

                    inputVariables[wordId-1].TermFrequency += countWord;
                    if (!listRow[docId-1].InputValue.ContainsKey(inputVariables[wordId-1]))
                    {
                        Cell news = new Cell(inputVariables[wordId-1], countWord);
                        listRow[docId-1].InputValue.Add(inputVariables[wordId-1], news);
                    }
                    else
                    {
                        listRow[docId-1].InputValue[inputVariables[wordId-1]].ValueCell = (int)listRow[docId-1].InputValue[inputVariables[wordId-1]].ValueCell + countWord;
                    }

                    inputVariables[wordId-1].RescaleLimitVariables(Convert.ToDouble(listRow[docId - 1].InputValue[inputVariables[wordId-1]].ValueCell));
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
            Console.WriteLine("Finish Read docword!");
            Random rnd = new Random();

            Dataset dataset = new Dataset("Newsgroup Dataset", listRow, inputVariables, outputVariables);
            TFIDF.TFIDF tfidf = new TFIDF.TFIDF();
            dataset = tfidf.Run(dataset);

            IClustering clusterMethod = new ClusteringKMeans(10, 1000, false, ref rnd, dataset);
            ClusteringResult clusters = clusterMethod.Run();
            List<string> report = clusters.PrintCompleteResult();
            for (int i = 0; i < report.Count; i++) Console.WriteLine(report[i]);
            System.IO.File.WriteAllLines(base_url + @"output.txt", report);


            IClustering clusterMethod2 = new ClusteringKMeans(10, 1000, false, ref rnd, dataset, new ForgyAlgorithm(10, dataset));
            ClusteringResult clusters2 = clusterMethod2.Run();
            List<string> report2 = clusters.PrintCompleteResult();
            for (int i = 0; i < report2.Count; i++) Console.WriteLine(report2[i]);
            System.IO.File.WriteAllLines(base_url + @"output2.txt", report2);
            string hold = Console.ReadLine();
        }
    }
}

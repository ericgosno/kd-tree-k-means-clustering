using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Extension;
using Clustering;
using Clustering.Initialization;

namespace DebuggerConsole
{
    class BagOfWord_Debug
    {
        public static void run()
        {
            List<Variables> listVariables = new List<Variables>();
            List<Row> listRow = new List<Row>();
            int D, W, NNZ;

            //Read Vocab
            FileStream fileStream = null;
            StreamReader streamReader = null;
            string base_url = @"E:\5109100153 - Eric\tc\";
            try
            {
                fileStream = new FileStream(base_url + @"vocab.kos.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);
                while (true)
                {
                    string line = streamReader.ReadLine();
                    Variables news = new Variables(line);
                    listVariables.Add(news);
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
            Console.WriteLine("Finish Read Vocab!");

            try
            {
                fileStream = new FileStream(base_url + @"docword.kos.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);
                D = Convert.ToInt32(streamReader.ReadLine());
                W = Convert.ToInt32(streamReader.ReadLine());
                NNZ = Convert.ToInt32(streamReader.ReadLine());

                while (true)
                {
                    string line = streamReader.ReadLine();
                    //if (line == null) continue;

                    if (string.IsNullOrEmpty(line))
                        break;

                    char[] separator = new char[1] { ' ' };

                    string[] linex = line.Split(separator);
                    int docId = Convert.ToInt32(linex[0]);
                    int wordId = Convert.ToInt32(linex[1]) - 1;
                    int countWord = Convert.ToInt32(linex[2]);
                    //Console.WriteLine(docId.ToString() + " " + wordId.ToString() + " " + countWord.ToString());
                    //System.Threading.Thread.Sleep(1000);

                    while (docId > listRow.Count)
                    {
                        Row news = new Row("docID#" + listRow.Count.ToString());
                        listRow.Add(news);
                        //Console.WriteLine("Row #" + listRow.Count.ToString());
                    }
                    listVariables[wordId].TermFrequency += countWord;
                    if (!listRow[docId - 1].InputValue.ContainsKey(listVariables[wordId]))
                    {
                        Cell news = new Cell(listVariables[wordId], countWord);
                        listRow[docId - 1].InputValue.Add(listVariables[wordId], news);
                    }
                    else
                    {
                        listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell = (int)listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell + countWord;
                    }
                    listVariables[wordId].RescaleLimitVariables(Convert.ToDouble(listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell));
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
            Console.WriteLine("Finish Read document!");
            Random rnd = new Random();

            Dataset dataset = new Dataset("Bag of Word Dataset", listRow, listVariables, new List<Variables>());
            IClustering clusterMethod = new ClusteringKMeans(10, 1000, false, ref rnd, dataset);
            ClusteringResult clusters = clusterMethod.Run();
            List<string> report = clusters.PrintCompleteResult();
            for (int i = 0; i < report.Count; i++) Console.WriteLine(report[i]);
            System.IO.File.WriteAllLines(base_url + @"output.txt", report);


            IClustering clusterMethod2 = new ClusteringKMeans(10, 1000, false, ref rnd, dataset, new ForgyAlgorithm(10, dataset));
            ClusteringResult clusters2 = clusterMethod2.Run();
            List<string> report2 = clusters2.PrintCompleteResult();
            for (int i = 0; i < report.Count; i++) Console.WriteLine(report2[i]);
            System.IO.File.WriteAllLines(base_url + @"output2.txt", report2);
            string hold = Console.ReadLine();
        }
    }
}

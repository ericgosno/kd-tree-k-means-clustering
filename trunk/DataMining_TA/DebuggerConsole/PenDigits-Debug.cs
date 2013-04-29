﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Extension;
using Clustering;
using Clustering.Initialization;

namespace DebuggerConsole
{
    class PenDigits_Debug
    {
        public static void run()
        {
            List<Variables> listVariables = new List<Variables>();
            List<Row> listRow = new List<Row>();
            int D, W, NNZ;

            //Read Vocab
            FileStream fileStream = null;
            StreamReader streamReader = null;
            string base_url = @"D:\tc\";

            try
            {
                fileStream = new FileStream(base_url + @"pendigits.tra", FileMode.Open);
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
                    int wordId = Convert.ToInt32(linex[1]);
                    int countWord = Convert.ToInt32(linex[2]);
                    //Console.WriteLine(docId.ToString() + " " + wordId.ToString() + " " + countWord.ToString());
                    //System.Threading.Thread.Sleep(1000);

                    while (docId > listRow.Count)
                    {
                        Row news = new Row("docID#" + listRow.Count.ToString());
                        listRow.Add(news);
                        //Console.WriteLine("Row #" + listRow.Count.ToString());
                    }
                    listVariables[wordId].Frequency += countWord;
                    if (!listRow[docId - 1].InputValue.ContainsKey(listVariables[wordId]))
                    {
                        Cell news = new Cell(listVariables[wordId], countWord);
                        listRow[docId - 1].InputValue.Add(listVariables[wordId], news);
                    }
                    else
                    {
                        listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell = (int)listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell + countWord;
                    }
                    double newMin = Math.Min(listVariables[wordId].LimitVariables.Key, Convert.ToDouble(listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell));
                    double newMax = Math.Max(listVariables[wordId].LimitVariables.Value, Convert.ToDouble(listRow[docId - 1].InputValue[listVariables[wordId]].ValueCell));
                    listVariables[wordId].LimitVariables = new KeyValuePair<double, double>(newMin, newMax);
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

            IClustering clusterMethod = new ClusteringKMeans(10, 1000, false, ref rnd, listRow, listVariables);
            ClusteringResult clusters = clusterMethod.Run();
            List<string> report = clusterMethod.PrintClusterResult(clusters);
            for (int i = 0; i < report.Count; i++) Console.WriteLine(report[i]);
            System.IO.File.WriteAllLines(base_url + @"output.txt", report);


            IClustering clusterMethod2 = new ClusteringKMeans(10, 1000, false, ref rnd, listRow, listVariables, new ForgyAlgorithm(10, listRow));
            ClusteringResult clusters2 = clusterMethod2.Run();
            List<string> report2 = clusterMethod2.PrintClusterResult(clusters2);
            for (int i = 0; i < report.Count; i++) Console.WriteLine(report2[i]);
            System.IO.File.WriteAllLines(base_url + @"output2.txt", report2);
            string hold = Console.ReadLine();
        }

    }
}

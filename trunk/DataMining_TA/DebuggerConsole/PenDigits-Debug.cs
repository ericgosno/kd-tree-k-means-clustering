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
    class PenDigits_Debug
    {
        public static Dataset run()
        {
            List<Variables> inputVariables = new List<Variables>();
            List<Variables> outputVariables = new List<Variables>();
            List<Row> listRow = new List<Row>();

            //Read Vocab
            FileStream fileStream = null;
            StreamReader streamReader = null;
            string base_url = @"E:\5109100153 - Eric\tc\";

            for (int i = 0; i < 16; i++)
            {
                Variables news = new ContinueVariable("Var#" + (i + 1).ToString());
                inputVariables.Add(news);
            }
            CategoricalVariable outputVar = new CategoricalVariable("Output1");
            for (int i = 0; i < 9; i++) outputVar.ParamVariables.Add(i, i);
            outputVariables.Add(outputVar as Variables);

            try
            {
                fileStream = new FileStream(base_url + @"pendigits.all", FileMode.Open);
                streamReader = new StreamReader(fileStream);

                while (true)
                {
                    string line = streamReader.ReadLine();
                    //if (line == null) continue;

                    if (string.IsNullOrEmpty(line))
                        break;

                    char[] separator = new char[1] { ',' };

                    string[] linex = line.Split(separator);
                    Row newRow = new Row();
                    for (int i = 0; i < 17; i++)
                    {
                        int ang = Convert.ToInt32(linex[i]);

                        if (i != 16)
                        {
                            Cell newCell = new Cell(inputVariables[i], ang);
                            inputVariables[i].RescaleLimitVariables(Convert.ToDouble(ang));
                            newRow.InputValue.Add(inputVariables[i], newCell);
                        }
                        else
                        {
                            newRow.RowIdentificator = ang.ToString();
                            newRow.OutputValue.Add(outputVariables[0], new Cell(outputVariables[0], ang));
                        }
                    }
                    listRow.Add(newRow);
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


            Dataset dataset = new Dataset("Pen Digits Dataset",listRow, inputVariables, outputVariables);
            return dataset;

            /*
            IClustering clusterMethod = new ClusteringKMeans(10, 1000, false, ref rnd, dataset);
            ClusteringResult clusters = clusterMethod.Run();
            List<string> report = clusters.PrintCompleteResult();
            for (int i = 0; i < report.Count; i++) Console.WriteLine(report[i]);
            System.IO.File.WriteAllLines(base_url + @"output.txt", report);


            IClustering clusterMethod2 = new ClusteringKMeans(10, 1000, false, ref rnd, dataset, new ForgyAlgorithm(10, dataset));
            ClusteringResult clusters2 = clusterMethod2.Run();
            List<string> report2 = clusters2.PrintCompleteResult();
            for (int i = 0; i < report2.Count; i++) Console.WriteLine(report2[i]);
            System.IO.File.WriteAllLines(base_url + @"output2.txt", report2);
            string hold = Console.ReadLine();
             */
        }

    }
}

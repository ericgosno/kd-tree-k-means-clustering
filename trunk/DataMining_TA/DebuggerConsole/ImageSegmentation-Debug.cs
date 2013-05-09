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
    public class ImageSegmentation_Debug
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

            try
            {
                fileStream = new FileStream(base_url + @"segmentation.all.txt", FileMode.Open);
                streamReader = new StreamReader(fileStream);

                string line = streamReader.ReadLine();
                string[] linex = line.Split(new char[1] { ',' });
                bool yes = true;
                foreach (string st in linex)
                {
                    if (yes)
                    {
                        CategoricalVariable outputVar = new CategoricalVariable(st);
                        outputVar.ParamVariables["BRICKFACE"] = 1;
                        outputVar.ParamVariables["SKY"] = 2;
                        outputVar.ParamVariables["FOLIAGE"] = 3;
                        outputVar.ParamVariables["CEMENT"] = 4;
                        outputVar.ParamVariables["WINDOW"] = 5;
                        outputVar.ParamVariables["PATH"] = 6;
                        outputVar.ParamVariables["GRASS"] = 7;
                        outputVariables.Add(outputVar as Variables);
                        yes = false;
                    }
                    else
                    {
                        Variables news = new ContinueVariable(st);
                        inputVariables.Add(news);                        
                    }
                }

                while (true)
                {
                    line = streamReader.ReadLine();
                    //if (line == null) continue;

                    if (string.IsNullOrEmpty(line))
                        break;

                    char[] separator = new char[1] { ',' };
                    linex = null;
                    linex = line.Split(separator);
                    Row newRow = new Row();
                    yes = true;
                    int numVar = 0;

                    foreach(string st in linex)
                    {
                        if (!yes)
                        {
                            double ang = Convert.ToDouble(st);
                            Cell newCell = new Cell(inputVariables[numVar], ang);
                            inputVariables[numVar].RescaleLimitVariables(ang);
                            newRow.InputValue.Add(inputVariables[numVar], newCell);
                            numVar++;
                        }
                        else
                        {
                            newRow.RowIdentificator = st;
                            newRow.OutputValue.Add(outputVariables[0], new Cell(outputVariables[0], (outputVariables[0] as CategoricalVariable).ParamVariables[st]));
                            yes = false;
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


            Dataset dataset = new Dataset("Image Segmentation Dataset", listRow, inputVariables, outputVariables);
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

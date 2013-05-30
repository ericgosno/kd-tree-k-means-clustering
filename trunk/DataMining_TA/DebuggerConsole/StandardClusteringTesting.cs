using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;
using Clustering;
using Clustering.Initialization;

namespace DebuggerConsole
{
    public class StandardClusteringTesting
    {
        public static void run(Dataset dataset, int numCluster)
        {
            Random rnd = new Random();
            string datasetName = dataset.TitleDataset.Replace(' ','_').ToLower();
            string base_url = @"E:\5109100153 - Eric\tc\";
            List<string> totalReport = new List<string>();
            double minDistortionKDTree = double.MaxValue;
            double minDistortionForgy = double.MaxValue;
            double minKDTreeTime = double.MaxValue;
            double maxKDTreeTime = double.MinValue;
            double minForgyTime = double.MaxValue;
            double maxForgyTime = double.MinValue;

            List<double> listSSEForgy = new List<double>();
            List<double> maxNIGKDTree = new List<double>();
            List<double> maxNIGForgy = new List<double>();
            double meanDistortionForgy = 0.0;
            double sdDistortionForgy = 0.0;

            int ForgyLose = 0;
            int ForgyWin = 0;
            int ForgyDraw = 0;

            for(int i = 0;i < dataset.OutputVariables.Count;i++)
            {
                maxNIGKDTree.Add(double.MinValue);
                maxNIGForgy.Add(double.MinValue);
            }

            List<string> reportKDTree = new List<string>();
            for (int j = 0; j < 2; j++)
            {
                IClustering clusterMethod = new ClusteringKMeans(numCluster, 1000, false, ref rnd, dataset, new KDTreeAlgorithm(numCluster, dataset, Convert.ToBoolean(j), true));
                ClusteringResult clusters = clusterMethod.Run();
                minKDTreeTime = Math.Min(minKDTreeTime, clusters.RunningTime);
                maxKDTreeTime = Math.Max(maxKDTreeTime, clusters.RunningTime);
                minDistortionKDTree = Math.Min(minDistortionKDTree, clusters.calculateSSE());
                for (int i = 0; i < dataset.OutputVariables.Count; i++)
                {
                    maxNIGForgy[i] = Math.Max(maxNIGForgy[i], clusters.CalculateNIG(dataset.OutputVariables[i]));
                }

                List<string> report = clusters.PrintCompleteResult();
                for (int i = 0; i < report.Count; i++) Console.WriteLine(report[i]);
                reportKDTree.AddRange(report);
            }
            System.IO.File.WriteAllLines(base_url + datasetName + @".KDTree.output.txt", reportKDTree);

            List<string> reportForgy = new List<string>();
            for (int j = 0; j < 15; j++)
            {
                IClustering clusterMethod2 = new ClusteringKMeans(numCluster, 1000, false, ref rnd, dataset, new ForgyAlgorithm(numCluster, dataset));
                ClusteringResult clusters2 = clusterMethod2.Run();
                double SSENow = clusters2.calculateSSE();
                listSSEForgy.Add(SSENow);
                minForgyTime = Math.Min(minForgyTime, clusters2.RunningTime);
                maxForgyTime = Math.Max(maxForgyTime, clusters2.RunningTime);
                minDistortionForgy = Math.Min(minDistortionForgy, SSENow);
                meanDistortionForgy += SSENow;
                for (int i = 0; i < dataset.OutputVariables.Count; i++)
                {
                    maxNIGKDTree[i] = Math.Max(maxNIGKDTree[i], clusters2.CalculateNIG(dataset.OutputVariables[i]));
                }

                if (Math.Abs(SSENow - minDistortionKDTree) < 1e-3) ForgyDraw++;
                else if (SSENow < minDistortionKDTree) ForgyWin++;
                else if (SSENow > minDistortionKDTree) ForgyLose++;

                List<string> report2 = clusters2.PrintCompleteResult();
                for (int i = 0; i < report2.Count; i++) Console.WriteLine(report2[i]);
                reportForgy.AddRange(report2);
            }
            System.IO.File.WriteAllLines(base_url + datasetName + @".Forgy.output.txt", reportForgy);

            meanDistortionForgy = meanDistortionForgy / 15;
            for (int i = 0; i < 15; i++)
            {
                double dist = listSSEForgy[i] - meanDistortionForgy;
                sdDistortionForgy += (dist * dist);
            }
            sdDistortionForgy = sdDistortionForgy / 15.0;
            sdDistortionForgy = Math.Sqrt(sdDistortionForgy);

            totalReport.Add("DATASET : ");
            totalReport.AddRange(dataset.PrintDatasetDetail());
            totalReport.Add("Min KD-Tree Time : " + minKDTreeTime);
            totalReport.Add("Max KD-Tree Time : " + maxKDTreeTime);
            totalReport.Add("Min Forgy Time : " + minForgyTime);
            totalReport.Add("Max Forgy Time : " + maxForgyTime);
            totalReport.Add("Min SSE KD-Tree : " + minDistortionKDTree);
            totalReport.Add("Min SSE Forgy : " + minDistortionForgy);
            totalReport.Add("Mean SSE Forgy : " + meanDistortionForgy);
            totalReport.Add("Standard Deviation SSE Forgy : " + sdDistortionForgy);
            totalReport.Add("Forgy vs KDTree (w-d-l) : " + ForgyWin + "-" + ForgyDraw + "-" + ForgyLose);
            for (int i = 0; i < dataset.OutputVariables.Count; i++)
            {
                totalReport.Add("Max " + dataset.OutputVariables[i].NameVariables + " NIG KD-Tree: " + maxNIGKDTree[i]);
                totalReport.Add("Max " + dataset.OutputVariables[i].NameVariables + " NIG Forgy: " + maxNIGForgy[i]);
            }
            System.IO.File.WriteAllLines(base_url + datasetName + @".total.output.txt", totalReport);
        }
    }
}

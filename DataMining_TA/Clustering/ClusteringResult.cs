// <copyright file="ClusteringResult.cs">
// Copyright (c) 05-04-2013 All Right Reserved
// </copyright>

// This script is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.   

// The GNU General Public License can be found at 
// http://www.gnu.org/copyleft/gpl.html

// This script is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU General Public License for more details.

// <author>Eric Budiman Gosno <eric.gosno@gmail.com></author>
// <date>05-04-2013</date>
// <summary>Class representing a ClusteringResult.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Clustering
{
    /// <summary>
    /// Contains Result of Clustering Method
    /// Including Running Time, List of Clusters, Number Repetition, Dataset, and Clustering Method
    /// </summary>
    
    public class ClusteringResult
    {
        #region private_or_protected_properties
        private Dataset dataset;
        private List<Cluster> clusters;
        private int numRep;
        private long runningTime;
        private IClustering clusteringMethod;
        #endregion

        #region public_properties
        public List<Cluster> Clusters
        {
            get { return clusters; }
            set { clusters = value; }
        }
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }

        public int NumRep
        {
            get { return numRep; }
            set { numRep = value; }
        }

        public long RunningTime
        {
            get { return runningTime; }
            set { runningTime = value; }
        }
        public IClustering ClusteringMethod
        {
            get { return clusteringMethod; }
            set { clusteringMethod = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringResult"/> class.
        /// </summary>
        public ClusteringResult()
        {
            this.clusters = new List<Cluster>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringResult"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="clusters">The clusters.</param>
        /// <param name="numRep">The num repetition</param>
        /// <param name="clusteringMethod">The clustering method.</param>
        public ClusteringResult(Dataset dataset,List<Cluster> clusters, int numRep, IClustering clusteringMethod)
        {
            this.clusters = clusters;
            this.numRep = numRep;
            this.dataset = dataset;
            this.clusteringMethod = clusteringMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringResult"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="clusters">The clusters.</param>
        /// <param name="numRep">The num repetition</param>
        /// <param name="runningTime">The running time.</param>
        public ClusteringResult(Dataset dataset,List<Cluster> clusters, int numRep,long runningTime)
        {
            this.clusters = clusters;
            this.numRep = numRep;
            this.runningTime = runningTime;
            this.dataset = dataset;
        }

        #endregion

        #region public_function
        /// <summary>
        /// Prints the complete result.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintCompleteResult()
        {
            List<string> report = new List<string>();
            report.AddRange(this.clusteringMethod.PrintDetail());
            report.AddRange(this.dataset.PrintDatasetDetail());
            report.AddRange(this.PrintDetail());
            report.AddRange(this.PrintClusterDetail());
            report.AddRange(this.PrintOutputDetail());
            return report;
        }

        /// <summary>
        /// Prints Clustering Result detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("Found " + clusters.Count + " Clusters");
            ans.Add("In " + numRep + " Repetitions");
            ans.Add("In " + runningTime + " ms");
            ans.Add("Total Distortion : " + calculateSSE().ToString());
            if (this.dataset.OutputVariables.Count > 0)
            {
                ans.Add("Normalized Information Gain : ");
                for (int i = 0; i < this.dataset.OutputVariables.Count; i++)
                {
                    ans.Add("NIG " + this.dataset.OutputVariables[i].NameVariables + " : " + CalculateNIG(this.dataset.OutputVariables[i]));
                }
            }
            return ans;
        }

        /// <summary>
        /// Prints the cluster detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintClusterDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("========= Start Cluster Detail ======");
            for (int i = 0; i < clusters.Count; i++)
            {
                ans.Add("Cluster#" + (i + 1).ToString() + " :");
                ans.Add("SSE : " + clusters[i].calculateSSE().ToString());
                ans.Add("Total Member : " + clusters[i].MemberCluster.Count.ToString());
                Dictionary<string, int> clusterByOutput = new Dictionary<string, int>();
                String nows = "Member : ";
                for (int j = 0; j < clusters[i].MemberCluster.Count; j++)
                {
                    //if (j != 0) nows += ";";
                    //nows += clusters[i].MemberCluster[j].RowIdentificator;
                    if (!clusterByOutput.ContainsKey(clusters[i].MemberCluster[j].RowIdentificator))
                    {
                        clusterByOutput[clusters[i].MemberCluster[j].RowIdentificator] = 0;
                    }
                    clusterByOutput[clusters[i].MemberCluster[j].RowIdentificator]++;
                }
                /*
                ans.Add(nows);
                ans.Add("Classified by output : ");
                foreach (string output in clusterByOutput.Keys)
                {
                    ans.Add(output + " : " + clusterByOutput[output].ToString());
                }
                 */
            }
            ans.Add("========= End Cluster Detail ======");
            return ans;
        }

        /// <summary>
        /// Prints the output detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintOutputDetail()
        {
            List<string> report = new List<string>();
            return report;
        }

        /// <summary>
        /// Calculates the Distortion/SSE.
        /// </summary>
        /// <returns></returns>
        public double calculateSSE()
        {
            double ans = 0.0;
            for (int i = 0; i < clusters.Count; i++)
            {
                ans += clusters[i].calculateSSE();
            }
            return ans;
        }

        /// <summary>
        /// Calculates the NIG(Normalized Information Gain).
        /// </summary>
        /// <param name="outputVariables">The output variables.</param>
        /// <returns></returns>
        public double CalculateNIG(Variables outputVariables)
        {
            double ENTotal = dataset.CalculateENTotal(outputVariables);
            double weightedEN = 0.0;
            foreach (Cluster cluster in clusters)
            {
                double percent = (Convert.ToDouble(cluster.MemberCluster.Count) / Convert.ToDouble(this.dataset.ListRow.Count));
                double ENCluster = cluster.CalculateENCluster(outputVariables);
                double singleWEN =  percent * ENCluster;
                weightedEN += singleWEN;
            }
            double NIG = (ENTotal - weightedEN) / ENTotal;
            return NIG;
        }
        #endregion
    }
}

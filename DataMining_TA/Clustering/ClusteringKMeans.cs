// <copyright file="ClusteringKMeans.cs">
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
// <summary>Class representing a ClusteringKMeans.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;
using Clustering.Initialization;
namespace Clustering
{
    /// <summary>
    /// Clustering Method using K-Means Clustering
    /// </summary>
    public class ClusteringKMeans : IClustering
    {
        #region private_or_protected_properties
        private int numCluster;
        private List<Cluster> clusters;
        private Dataset dataset;
        private int numRep;
        private Random rnd;
        private const double EPSILON = 1e-6;
        private bool isNormalize;
        private IClusteringInitialization initializationMethod;
        #endregion

        #region public_properties
        public int NumCluster
        {
          get { return numCluster; }
          set { numCluster = value; }
        }

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

        public bool IsNormalize
        {
          get { return isNormalize; }
          set { isNormalize = value; }
        }

        public int NumRep
        {
          get { return numRep; }
          set { numRep = value; }
        }

        public Random Rnd
        {
            get { return rnd; }
            set { rnd = value; }
        }
        public IClusteringInitialization InitializationMethod
        {
            get { return initializationMethod; }
            set { initializationMethod = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringKMeans"/> class.
        /// </summary>
        public ClusteringKMeans()
        {
            this.numCluster = 10;
            this.numRep = 1000;
            this.clusters = new List<Cluster>();
            this.rnd = new Random();
            isNormalize = false;
            initializationMethod = new KDTreeAlgorithm(numCluster, false,false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringKMeans"/> class.
        /// </summary>
        /// <param name="numCluster">The num cluster.</param>
        /// <param name="numRep">The num rep.</param>
        /// <param name="isNormalized">if set to <c>true</c> [is normalized].</param>
        /// <param name="rnd">The RND.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="initializationMethod">The initialization method.</param>
        public ClusteringKMeans(int numCluster, int numRep, bool isNormalized, ref Random rnd, Dataset dataset,IClusteringInitialization initializationMethod)
        {
            this.numCluster = numCluster;
            this.numRep = numRep;
            this.clusters = new List<Cluster>();
            this.rnd = rnd;
            this.isNormalize = isNormalized;
            this.dataset = dataset;
            this.initializationMethod = initializationMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringKMeans"/> class.
        /// </summary>
        /// <param name="numCluster">The num cluster.</param>
        /// <param name="numRep">The num rep.</param>
        /// <param name="isNormalized">if set to <c>true</c> [is normalized].</param>
        /// <param name="rnd">The RND.</param>
        /// <param name="dataset">The dataset.</param>
        public ClusteringKMeans(int numCluster, int numRep, bool isNormalized, ref Random rnd, Dataset dataset)
        {
            this.numCluster = numCluster;
            this.numRep = numRep;
            this.clusters = new List<Cluster>();
            this.rnd = rnd;
            this.isNormalize = isNormalized;
            this.dataset = dataset;
            this.initializationMethod = new KDTreeAlgorithm(numCluster, dataset, false,false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringKMeans"/> class.
        /// </summary>
        /// <param name="numCluster">The num cluster.</param>
        /// <param name="numRep">The num rep.</param>
        /// <param name="isNormalized">if set to <c>true</c> [is normalized].</param>
        /// <param name="rnd">The RND.</param>
        /// <param name="initializationMethod">The initialization method.</param>
        public ClusteringKMeans(int numCluster, int numRep, bool isNormalized, ref Random rnd,IClusteringInitialization initializationMethod)
        {
            this.numCluster = numCluster;
            this.numRep = numRep;
            this.clusters = new List<Cluster>();
            this.rnd = rnd;
            this.isNormalize = isNormalized;
            this.initializationMethod = initializationMethod;
        }
        #endregion

        #region iClustering_implementation
        /// <summary>
        /// Prints the cluster result.
        /// </summary>
        /// <param name="ClusterResult">The cluster result.</param>
        /// <returns></returns>
        public List<string> PrintClusterResult()
        {
            List<string> ans = new List<string>();
            ans.Add("K-Means Clustering Algorithm");
            ans.Add("Number of Cluster = " + clusters.Count);
            ans.AddRange(initializationMethod.PrintDetail());
            return ans;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public ClusteringResult Run()
        {
            var sw = Stopwatch.StartNew();
            ClusteringResult ans = this.RunKMeansClustering();
            long elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
            ans.RunningTime = elapsedTime;
            return ans;
        }

        /// <summary>
        /// Runs the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="numCluster">The num cluster.</param>
        /// <param name="isNormalize">if set to <c>true</c> [is normalize].</param>
        /// <returns></returns>
        public ClusteringResult Run(Dataset dataset, int numCluster, bool isNormalize)
        {
            this.Dataset = dataset;
            this.numCluster = numCluster;
            this.isNormalize = isNormalize;
            return this.Run();
        }
        #endregion

        #region private_function
        /// <summary>
        /// Repetition K-Means Clustering
        /// </summary>
        /// <param name="listRow">List Row</param>
        /// <returns></returns>
        private bool KMeanRep(List<Row> listRow)
        {
            bool isRepeatAgain = false;
            for (int i = 0; i < clusters.Count; i++)
            {
                clusters[i].MemberCluster.Clear();
            }

            for (int i = 0; i < listRow.Count; i++)
            {
                KeyValuePair<int, double> mini = new KeyValuePair<int, double>(-1, 2000000000);
                for (int j = 0; j < clusters.Count; j++)
                {
                    double euclid = listRow[i].EuclideanDistance(clusters[j].Centroid);
                    if (euclid < mini.Value)
                    {
                        mini = new KeyValuePair<int, double>(j, euclid);
                    }
                }
                clusters[mini.Key].MemberCluster.Add(listRow[i]);
            }

            for (int i = 0; i < clusters.Count; i++)
            {
                Row news = new Row();
                foreach (Variables j in clusters[i].Centroid.InputValue.Keys)
                {
                    double newsVal = 0.0;
                    for (int k = 0; k < clusters[i].MemberCluster.Count; k++)
                    {
                        if (clusters[i].MemberCluster[k].InputValue.ContainsKey(j))
                            newsVal += Convert.ToDouble(clusters[i].MemberCluster[k].InputValue[j].ValueCell);
                    }
                    newsVal = newsVal / clusters[i].MemberCluster.Count;
                    news.InputValue.Add(clusters[i].Centroid.InputValue[j].VarCell, new Cell(clusters[i].Centroid.InputValue[j].VarCell, newsVal));
                    if (newsVal - Convert.ToDouble(clusters[i].Centroid.InputValue[j].ValueCell) > EPSILON)
                    {
                        isRepeatAgain = true;
                    }
                }
                clusters[i].Centroid = news;
            }
            return isRepeatAgain;
        }


        /// <summary>
        /// Runs the K means clustering.
        /// </summary>
        /// <returns></returns>
        private ClusteringResult RunKMeansClustering()
        {
            if (dataset.ListRow.Count <= 0)
            {
                return null;
            }
            clusters = new List<Cluster>();

            Dataset tmpDataset = this.dataset; //dataset.Copy();

            if (isNormalize)
            {
                // Normalize
                for (int i = 0; i < tmpDataset.ListRow.Count; i++)
                {
                    foreach(Variables j in tmpDataset.ListRow[i].InputValue.Keys)
                    {
                        KeyValuePair<double, double> limit = tmpDataset.ListRow[i].InputValue[j].VarCell.LimitVariables;
                        tmpDataset.ListRow[i].InputValue[j].ValueCell = (Convert.ToDouble(tmpDataset.ListRow[i].InputValue[j].ValueCell) - limit.Key) / (limit.Value - limit.Key);
                    }
                }
            }

            //Console.WriteLine("Number Cluster = " + numCluster.ToString());
            //Console.WriteLine("Number Epoch = " + numRep.ToString());

            List<Row> listCentroid = initializationMethod.Run(tmpDataset, numCluster);
            for (int i = 0; i < listCentroid.Count; i++)
            {
                clusters.Add(new Cluster(listCentroid[i]));
            }

            int lastRep = numRep;
            for (int i = 0; i < numRep; i++)
            {
                Console.WriteLine("Repetition #" + (i + 1).ToString());
                if (!KMeanRep(tmpDataset.ListRow))
                {
                    lastRep = i+1;
                    //Console.WriteLine("Finished at Repetition #" + (i + 1).ToString());
                    break;
                }
            }
            
            List<Cluster> anews = new List<Cluster>();
            foreach (Cluster x in clusters)
            {
                if (x.MemberCluster.Count != 0)
                {
                    anews.Add(x);
                }
            }
            clusters = anews;

            ClusteringResult result = new ClusteringResult(dataset,clusters, lastRep,this);
            
            return result;
        }
        #endregion
    }
}

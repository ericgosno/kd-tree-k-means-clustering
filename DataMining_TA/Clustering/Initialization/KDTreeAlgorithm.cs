// <copyright file="KDTreeAlgorithm.cs">
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
// <summary>Class representing a KDTreeAlgorithm.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;
using K_D_Tree;
using K_D_Tree.Separator;
namespace Clustering.Initialization
{
    /// <summary>
    /// Initialisation K-Means Clustering using KD-Tree K-Means Algorithm
    /// Implemented from journal "A Method for initialising the K-Means Clustering Algorithm using KD-Tree" (Redmond, Heneghan 2007)
    /// </summary>
    public class KDTreeAlgorithm : IClusteringInitialization
    {
        #region private_or_protected_properties
        private int numK;
        private Dataset dataset;
        private bool useOutlierRemoval;
        private bool useDensityRank;
        #endregion

        #region public_properties
        public bool UseOutlierRemoval
        {
            get { return useOutlierRemoval; }
            set { useOutlierRemoval = value; }
        }
        public bool UseDensityRank
        {
            get { return useDensityRank; }
            set { useDensityRank = value; }
        }
        public int NumK
        {
          get { return numK; }
          set { numK = value; }
        }
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="KDTreeAlgorithm"/> class.
        /// </summary>
        public KDTreeAlgorithm()
        {
            this.numK = 0;
            this.dataset = new Dataset();
            this.useOutlierRemoval = false;
            this.useDensityRank = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KDTreeAlgorithm"/> class.
        /// </summary>
        /// <param name="numK">Number of Cluster</param>
        /// <param name="useOutlierRemoval">if set to <c>true</c> [use outlier removal].</param>
        /// <param name="useDensityRank">if set to <c>true</c> [use density rank].</param>
        public KDTreeAlgorithm(int numK, bool useOutlierRemoval,bool useDensityRank)
        {
            this.numK = numK;
            this.dataset = new Dataset();
            this.useOutlierRemoval = useOutlierRemoval;
            this.useDensityRank = useDensityRank;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KDTreeAlgorithm"/> class.
        /// </summary>
        /// <param name="numK">Number of Cluster</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="useOutlierRemoval">if set to <c>true</c> [use outlier removal].</param>
        /// <param name="useDensityRank">if set to <c>true</c> [use density rank].</param>
        public KDTreeAlgorithm(int numK, Dataset dataset,bool useOutlierRemoval, bool useDensityRank)
        {
            this.numK = numK;
            this.dataset = dataset;
            this.useOutlierRemoval = useOutlierRemoval;
            this.useDensityRank = useDensityRank;
        }
        #endregion

        #region iClusteringInitialization_Implementation
        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public List<Row> Run()
        {
            if (numK <= 0 || dataset.ListRow.Count <= 0 || dataset.ListRow.Count < numK)
            {
                return null;
            }


            Dataset tmpDataset = this.dataset; //dataset.Copy();
            List<Row> centroid = new List<Row>();
            // Algorithm start here
            // Build KD-Tree
            KDTree kdtree = new KDTree(tmpDataset, tmpDataset.ListRow.Count / (10 * numK));
            kdtree.Run();
            List<Leaf> leafBucket = kdtree.ListBucketLeaf;
            //List<Leaf> leafBucket = kdtree.TraceLeafBucket();
            
            // Use Density Rank Instead of using Density Estimate
            leafBucket.Sort((t1, t2) => t2.Density.CompareTo(t1.Density));
            Dictionary<Leaf, double> rank = new Dictionary<Leaf, double>();
            if(useDensityRank)
            {
            for (int i = 0; i < leafBucket.Count; i++)
            {
                rank[leafBucket[i]] = leafBucket.Count - i;
            }
                }
            int x = leafBucket.Count;
            // Outlier Removal(if useOutlierRemoval == true)
            int NumRemoval = leafBucket.Count / 5;
            int RemainingBucket = leafBucket.Count - NumRemoval;
            if (useOutlierRemoval && RemainingBucket >= numK)
            {                
                leafBucket.RemoveRange(RemainingBucket, NumRemoval);
            }

            /* Try use DP  to optimized */
            Dictionary<Leaf, double> CentroidDistance = new Dictionary<Leaf, double>();

            // Choose Centroid based density rank
            for (int i = 0; i < numK; i++)
            {
                if (centroid.Count == 0)
                {
                    centroid.Add(leafBucket[0].MidPoint.Copy());
                    leafBucket.Remove(leafBucket[0]);
                }
                else
                {
                    KeyValuePair<double,Leaf> MaxLeafValue = new KeyValuePair<double,Leaf>(double.MinValue,null);
                    for (int j = 0; j < leafBucket.Count; j++)
                    {
                        double minDistance = double.MaxValue;
                        /*
                        for (int k = 0;k < centroid.Count; k++)
                        {
                            double distNow = centroid[k].EuclideanDistance(leafBucket[j].MidPoint);
                            minDistance = Math.Min(minDistance, distNow);
                        }
                        */
                        /* Optimized using DP */
                        double distnow = centroid[centroid.Count - 1].EuclideanDistance(leafBucket[j].MidPoint);
                        if (CentroidDistance.ContainsKey(leafBucket[j]))
                        {
                            double prevDist = CentroidDistance[leafBucket[j]];
                            if (prevDist > distnow)
                            {
                                CentroidDistance[leafBucket[j]] = distnow;
                                minDistance = distnow;
                            }
                            else minDistance = prevDist;
                        }
                        else
                        {
                            CentroidDistance.Add(leafBucket[j], distnow);
                            minDistance = distnow;
                        }

                        // use rank instead of density estimates
                        double density = useDensityRank ? rank[leafBucket[j]] : leafBucket[j].Density;//rank[leafBucket[j]];
                        double valG = minDistance * density;
                        if (MaxLeafValue.Key < valG)
                        {
                            MaxLeafValue = new KeyValuePair<double, Leaf>(valG, leafBucket[j]);
                        }
                    }
                    centroid.Add(MaxLeafValue.Value.MidPoint.Copy());
                    leafBucket.Remove(MaxLeafValue.Value);
                }
            }
            return centroid;
        }

        /// <summary>
        /// Runs the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="K">Number of Cluster</param>
        /// <returns></returns>
        public List<Row> Run(Dataset dataset, int K)
        {
            this.numK = K;
            this.dataset = dataset;
            return this.Run();
        }

        /// <summary>
        /// Runs with running time calculation
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<List<Row>, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            List<Row> ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            return new KeyValuePair<List<Row>, long>(ans, elapsedTime); 
        }

        /// <summary>
        /// Runs with running time calculation
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="K">Number of Cluster</param>
        /// <returns></returns>
        public KeyValuePair<List<Row>, long> RunWithTime(Dataset dataset, int K)
        {
            this.dataset = dataset;
            this.numK = K;
            return this.RunWithTime();
        }

        /// <summary>
        /// Prints the detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("Initialization Method : KD-Tree Algorithm");
            ans.Add("Use Outlier Removal : " + (useOutlierRemoval ? "true" : "false"));
            return ans;
        }
        #endregion
    }
}

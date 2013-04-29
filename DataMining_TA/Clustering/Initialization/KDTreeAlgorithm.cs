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
    public class KDTreeAlgorithm : IClusteringInitialization
    {
        int numK;
        List<Row> dataset;
        bool useOutlierRemoval;
        List<Variables> listVariables;

        #region public_properties
        public List<Variables> ListVariables
        {
            get { return listVariables; }
            set { listVariables = value; }
        }

        public bool UseOutlierRemoval
        {
            get { return useOutlierRemoval; }
            set { useOutlierRemoval = value; }
        }

        public int NumK
        {
          get { return numK; }
          set { numK = value; }
        }
        public List<Row> Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        #endregion

        public KDTreeAlgorithm()
        {
            this.numK = 0;
            this.dataset = new List<Row>();
            this.useOutlierRemoval = false;
            this.listVariables = new List<Variables>();
        }

        public KDTreeAlgorithm(int numK, bool useOutlierRemoval)
        {
            this.numK = numK;
            this.dataset = new List<Row>();
            this.listVariables = new List<Variables>();
            this.useOutlierRemoval = useOutlierRemoval;
        }

        public KDTreeAlgorithm(int numK, List<Row> dataset,bool useOutlierRemoval, List<Variables> listVariables)
        {
            this.numK = numK;
            this.dataset = dataset;
            this.useOutlierRemoval = useOutlierRemoval;
            this.listVariables = listVariables;
        }

        public List<Row> Run()
        {
            /*if (numK <= 0 || dataset.Count <= 0 || dataset.Count < numK)
            {
                return null;
            }*/
            List<Row> tmpRow = new List<Row>();
            for(int i = 0;i < dataset.Count;i++)
            {
                tmpRow.Add(dataset[i].Copy());
            }
            List<Row> centroid = new List<Row>();
            // Algorithm start here
            // Build KD-Tree
            KDTree kdtree = new KDTree(dataset, listVariables, dataset.Count / (10 * numK));
            kdtree.Run();
            List<Leaf> leafBucket = kdtree.TraceLeafBucket();
            
            // Use Density Rank Instead of using Density Estimate
            leafBucket.Sort((t1, t2) => t2.Density.CompareTo(t1.Density));
            Dictionary<Leaf, double> rank = new Dictionary<Leaf, double>();
            for (int i = 0; i < leafBucket.Count; i++)
            {
                rank[leafBucket[i]] = leafBucket.Count - i;
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
                        double density = rank[leafBucket[j]];//leafBucket[j].Density;
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

        public List<Row> Run(List<Row> dataset, int K)
        {
            this.numK = K;
            this.dataset = dataset;
            return this.Run();
        }

        public KeyValuePair<List<Row>, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            List<Row> ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            return new KeyValuePair<List<Row>, long>(ans, elapsedTime); 
        }
        public KeyValuePair<List<Row>, long> RunWithTime(List<Row> dataset, int K)
        {
            this.dataset = dataset;
            this.numK = K;
            return this.RunWithTime();
        }


        public List<string> PrintDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("Initialization Method : KD-Tree Algorithm");
            ans.Add("Use Outlier Removal : " + (useOutlierRemoval ? "true" : "false"));
            return ans;            
        }
    }
}

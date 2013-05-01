using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Clustering
{
    public class ClusteringResult
    {
        private Dataset dataset;
        private List<Cluster> clusters;
        private int numRep;
        private long runningTime;

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
        #endregion

        #region Constructor
        public ClusteringResult()
        {
            this.clusters = new List<Cluster>();
        }

        public ClusteringResult(Dataset dataset,List<Cluster> clusters, int numRep)
        {
            this.clusters = clusters;
            this.numRep = numRep;
            this.dataset = dataset;
        }

        public ClusteringResult(Dataset dataset,List<Cluster> clusters, int numRep,long runningTime)
        {
            this.clusters = clusters;
            this.numRep = numRep;
            this.runningTime = runningTime;
            this.dataset = dataset;
        }

        #endregion

        public List<string> PrintDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("Found " + clusters.Count + " Clusters");
            ans.Add("In " + numRep + " Repetitions");
            ans.Add("In " + runningTime + " ms");
            ans.Add("Total Distortion : " + calculateSSE().ToString());
            return ans;
        }

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
                ans.Add(nows);
                ans.Add("Classified by output : ");
                foreach (string output in clusterByOutput.Keys)
                {
                    ans.Add(output + " : " + clusterByOutput[output].ToString());
                }
            }
            ans.Add("========= End Cluster Detail ======");
            return ans;
        }

        public double calculateSSE()
        {
            double ans = 0.0;
            for (int i = 0; i < clusters.Count; i++)
            {
                ans += clusters[i].calculateSSE();
            }
            return ans;
        }
    }
}

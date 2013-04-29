﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Clustering
{
    public class ClusteringResult
    {
        private List<Cluster> clusters;
        private int numRep;
        private long runningTime;

        #region public_properties
        public List<Cluster> Clusters
        {
            get { return clusters; }
            set { clusters = value; }
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

        public ClusteringResult(List<Cluster> clusters, int numRep)
        {
            this.clusters = clusters;
            this.numRep = numRep;
        }

        public ClusteringResult(List<Cluster> clusters, int numRep,long runningTime)
        {
            this.clusters = clusters;
            this.numRep = numRep;
            this.runningTime = runningTime;
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
                String nows = "Member : ";
                for (int j = 0; j < clusters[i].MemberCluster.Count; j++)
                {
                    if (j != 0) nows += ";";
                    nows += clusters[i].MemberCluster[j].RowIdentificator;
                }
                ans.Add(nows);
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

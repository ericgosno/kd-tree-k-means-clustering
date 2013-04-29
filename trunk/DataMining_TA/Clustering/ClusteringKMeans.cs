using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;
using Clustering.Initialization;
namespace Clustering
{
    public class ClusteringKMeans : IClustering
    {
        private int numCluster;
        private List<Cluster> clusters;
        private List<Variables> listVariables;
        private List<Row> listRow;
        private int numRep;
        private Random rnd;
        private const double EPSILON = 1e-6;
        private bool isNormalize;
        private IClusteringInitialization initializationMethod;

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


        public List<Row> ListRow
        {
          get { return listRow; }
          set { listRow = value; }
        }

        public List<Variables> ListVariables
        {
          get { return listVariables; }
          set { listVariables = value; }
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
        public ClusteringKMeans()
        {
            this.numCluster = 10;
            this.numRep = 1000;
            this.clusters = new List<Cluster>();
            this.listVariables = new List<Variables>();
            this.listRow = new List<Row>();
            this.rnd = new Random();
            isNormalize = false;
            initializationMethod = new KDTreeAlgorithm(numCluster, false);
        }
        public ClusteringKMeans(int numCluster, int numRep, bool isNormalized, ref Random rnd, List<Row> listRow, List<Variables> listVariables,IClusteringInitialization initializationMethod)
        {
            this.numCluster = numCluster;
            this.numRep = numRep;
            this.clusters = new List<Cluster>();
            this.rnd = rnd;
            this.isNormalize = isNormalized;
            this.listVariables = listVariables;
            this.listRow = listRow;
            this.initializationMethod = initializationMethod;
        }

        public ClusteringKMeans(int numCluster, int numRep, bool isNormalized, ref Random rnd, List<Row> listRow, List<Variables> listVariables)
        {
            this.numCluster = numCluster;
            this.numRep = numRep;
            this.clusters = new List<Cluster>();
            this.rnd = rnd;
            this.isNormalize = isNormalized;
            this.listVariables = listVariables;
            this.listRow = listRow;
            this.initializationMethod = new KDTreeAlgorithm(numCluster, listRow, false, listVariables);
        }
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

        public List<string> PrintClusterResult(object ClusterResult)
        {
            List<string> ans = new List<string>();
            ans.Add("K-Means Clustering Algorithm");
            ClusteringResult result = (ClusteringResult)ClusterResult;
            ans.Add("Number of Cluster = " + clusters.Count);
            ans.AddRange(initializationMethod.PrintDetail());
            ans.AddRange(result.PrintDetail());
            return ans;
        }



        private bool KMeanRep(List<Row> Examples)
        {
            bool isRepeatAgain = false;
            for (int i = 0; i < clusters.Count; i++)
            {
                clusters[i].MemberCluster.Clear();
            }

            for (int i = 0; i < Examples.Count; i++)
            {
                KeyValuePair<int, double> mini = new KeyValuePair<int, double>(-1, 2000000000);
                for (int j = 0; j < clusters.Count; j++)
                {
                    double euclid = Examples[i].EuclideanDistance(clusters[j].Centroid);
                    if (euclid < mini.Value)
                    {
                        mini = new KeyValuePair<int, double>(j, euclid);
                    }
                }
                clusters[mini.Key].MemberCluster.Add(Examples[i]);
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

        private ClusteringResult RunKMeansClustering()
        {
            if (listRow.Count <= 0)
            {
                return null;
            }
            clusters = new List<Cluster>();
            List<Row> Examples = new List<Row>();
            for (int i = 0; i < listRow.Count; i++)
            {
                Examples.Add(listRow[i].Copy());
            }

            if (isNormalize)
            {
                // Normalize
                for (int i = 0; i < Examples.Count; i++)
                {
                    foreach(Variables j in Examples[i].InputValue.Keys)
                    {
                        KeyValuePair<double, double> limit = Examples[i].InputValue[j].VarCell.LimitVariables;
                        Examples[i].InputValue[j].ValueCell = (Convert.ToDouble(Examples[i].InputValue[j].ValueCell) - limit.Key) / (limit.Value - limit.Key);
                    }
                }
            }

            //Console.WriteLine("Number Cluster = " + numCluster.ToString());
            //Console.WriteLine("Number Epoch = " + numRep.ToString());

            List<Row> listCentroid = initializationMethod.Run(Examples, numCluster);
            for (int i = 0; i < listCentroid.Count; i++)
            {
                clusters.Add(new Cluster(listCentroid[i]));
            }

            int lastRep = numRep;
            for (int i = 0; i < numRep; i++)
            {
                if (!KMeanRep(Examples))
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

            ClusteringResult result = new ClusteringResult(clusters, lastRep);
            
            return result;
        }


        public ClusteringResult Run()
        {
            var sw = Stopwatch.StartNew();
            ClusteringResult ans = this.RunKMeansClustering();
            long elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
            ans.RunningTime = elapsedTime;
            return ans;
        }

        public ClusteringResult Run(List<Row> listRow, List<Variables> listVariables, int numCluster, bool isNormalize)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.numCluster = numCluster;
            this.isNormalize = isNormalize;
            return this.Run();
        }
    }
}

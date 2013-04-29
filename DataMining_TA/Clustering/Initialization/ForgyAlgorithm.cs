using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;

namespace Clustering.Initialization
{
    public class ForgyAlgorithm : IClusteringInitialization
    {
        int numK;
        List<Row> dataset;

        public List<Row> Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        public int NumK
        {
          get { return numK; }
          set { numK = value; }
        }
        public ForgyAlgorithm()
        {
            numK = 0;
            dataset = new List<Row>();
        }

        public ForgyAlgorithm(int numK)
        {
            this.numK = numK;
            this.dataset = new List<Row>();
        }

        public ForgyAlgorithm(int numK, List<Row> dataset)
        {
            this.numK = numK;
            this.dataset = dataset;
        }

        public List<Row> Run()
        {
            if (numK <= 0 || dataset.Count <= 0 || dataset.Count < numK)
            {
                return null;
            }
            List<Row> tmpRow = new List<Row>();
            for(int i = 0;i < dataset.Count;i++)
            {
                tmpRow.Add(dataset[i].Copy());
            }
            List<Row> centroid = new List<Row>();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < numK; i++)
            {
                int nows = rnd.Next(tmpRow.Count);
                centroid.Add(tmpRow[nows].Copy());
                tmpRow.Remove(tmpRow[nows]);
            }
            return centroid;
        }
        public List<Row> Run(List<Row> dataset, int K)
        {
            this.numK = K;
            this.dataset = dataset;
            return this.Run();
        }

        public KeyValuePair<List<Row>, long> RunWithTime(List<Row> dataset, int K)
        {
            this.numK = K;
            this.dataset = dataset;
            return this.RunWithTime();
        }

        public KeyValuePair<List<Row>, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            List<Row> ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
            return new KeyValuePair<List<Row>, long>(ans, elapsedTime);
        }


        public List<string> PrintDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("Initialization Method : Forgy's Algorithm");
            return ans;
        }
    }
}

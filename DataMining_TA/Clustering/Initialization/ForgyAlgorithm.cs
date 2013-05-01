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
        Dataset dataset;

        #region public_properties
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        public int NumK
        {
          get { return numK; }
          set { numK = value; }
        }
        #endregion

        #region constructor
        public ForgyAlgorithm()
        {
            numK = 0;
            dataset = new Dataset();
        }

        public ForgyAlgorithm(int numK)
        {
            this.numK = numK;
            this.dataset = new Dataset();
        }

        public ForgyAlgorithm(int numK, Dataset dataset)
        {
            this.numK = numK;
            this.dataset = dataset;
        }
        #endregion

        public List<Row> Run()
        {
            if (numK <= 0 || dataset.ListRow.Count <= 0 || dataset.ListRow.Count < numK)
            {
                return null;
            }

            Dataset tmpDataset = dataset.Copy();
            List<Row> centroid = new List<Row>();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < numK; i++)
            {
                int nows = rnd.Next(tmpDataset.ListRow.Count);
                centroid.Add(tmpDataset.ListRow[nows].Copy());
                tmpDataset.ListRow.Remove(tmpDataset.ListRow[nows]);
            }
            return centroid;
        }
        public List<Row> Run(Dataset dataset, int K)
        {
            this.numK = K;
            this.dataset = dataset;
            return this.Run();
        }

        public KeyValuePair<List<Row>, long> RunWithTime(Dataset dataset, int K)
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

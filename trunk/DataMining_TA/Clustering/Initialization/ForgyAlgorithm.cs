// <copyright file="ForgyAlgorithm.cs">
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
// <summary>Class representing a ForgyAlgorithm.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;

namespace Clustering.Initialization
{
    /// <summary>
    /// Initialisation K-Means Clustering using Forgy's Algorithm
    /// Implemented from journal "Some Method for Classification and Analysis for Multivariate" (MacQueen, 1967)
    /// </summary>
    public class ForgyAlgorithm : IClusteringInitialization
    {
        #region private_or_protected_properties
        private int numK;
        private Dataset dataset;
        #endregion

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
        /// <summary>
        /// Initializes a new instance of the <see cref="ForgyAlgorithm"/> class.
        /// </summary>
        public ForgyAlgorithm()
        {
            numK = 0;
            dataset = new Dataset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgyAlgorithm"/> class.
        /// </summary>
        /// <param name="numK">The num K.</param>
        public ForgyAlgorithm(int numK)
        {
            this.numK = numK;
            this.dataset = new Dataset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgyAlgorithm"/> class.
        /// </summary>
        /// <param name="numK">The num K.</param>
        /// <param name="dataset">The dataset.</param>
        public ForgyAlgorithm(int numK, Dataset dataset)
        {
            this.numK = numK;
            this.dataset = dataset;
        }
        #endregion

        #region iClusteringInitialization Implementation
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

            Dataset tmpDataset = this.dataset;//dataset.Copy();
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
        /// Runs the with time.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="K">Number of Cluster</param>
        /// <returns></returns>
        public KeyValuePair<List<Row>, long> RunWithTime(Dataset dataset, int K)
        {
            this.numK = K;
            this.dataset = dataset;
            return this.RunWithTime();
        }

        /// <summary>
        /// Runs the with time.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<List<Row>, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            List<Row> ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
            return new KeyValuePair<List<Row>, long>(ans, elapsedTime);
        }


        /// <summary>
        /// Prints the detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDetail()
        {
            List<string> ans = new List<string>();
            ans.Add("Initialization Method : Forgy's Algorithm");
            return ans;
        }
        #endregion
    }
}

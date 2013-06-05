// <copyright file="MeanMedianFS.cs">
// Copyright (c) 05-31-2013 All Right Reserved
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
// <date>05-31-2013</date>
// <summary>Class representing a MeanMedianFS.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;
using K_D_Tree.Separator;

namespace FeatureSelection.Unsupervised.DispersionMeasure
{
    /// <summary>
    /// Dispersion Measure for Relevance Filter using Mean-Median
    /// This is a Unsupervised Method so it's don't need a class/label in the process
    /// Directly Implemented from journal 
    /// "Efficient Feature Selection Filters for High-Dimensional Data" 
    /// (Ferreira et all, 2012)
    /// </summary>
    public class MeanMedianFS : IDispersionMeasure
    {
        #region private_or_protected_properties
        /// <summary>
        /// The Maximum Feature to keep
        /// Set default to Integer Maximum
        /// (means that this method only rank dataset's features)
        /// </summary>
        private int maxFeature;
        private Dataset dataset;
        #endregion

        #region public_properties
        public int MaxFeature
        {
            get { return maxFeature; }
            set { maxFeature = value; }
        }
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MeanMedianFS"/> class.
        /// </summary>
        public MeanMedianFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeanMedianFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public MeanMedianFS(Dataset dataset)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeanMedianFS"/> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public MeanMedianFS(int maxFeature)
        {
            this.dataset = null;
            this.maxFeature = maxFeature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeanMedianFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public MeanMedianFS(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
        }
        #endregion

        #region private_function
        private Dictionary<Variables, double> CalculateTermMark(Dataset tmpDataset)
        {
            int numRow = tmpDataset.ListRow.Count;
            Dictionary<Variables, double> termMark = new Dictionary<Variables, double>();
            Dictionary<Variables, double> meanTerm = new Dictionary<Variables, double>();
            Dictionary<Variables, List<double>> listValue = new Dictionary<Variables, List<double>>();

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach (Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    double valnow = Convert.ToDouble(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                    if (!meanTerm.ContainsKey(var)) meanTerm[var] = 0.0;
                    meanTerm[var] += valnow;
                    if (!listValue.ContainsKey(var)) listValue[var] = new List<double>();
                    listValue[var].Add(valnow);
                }
            }

            for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                List<double> listVal = new List<double>();
                listVal.AddRange(listValue[tmpDataset.InputVariables[i]]);
                while (listVal.Count != tmpDataset.ListRow.Count) listVal.Add(0.0);
                MedianBFPRTSeparator medianMethod = new MedianBFPRTSeparator();
                listVal.Clear();
                double median = medianMethod.Run(listValue[tmpDataset.InputVariables[i]]);
                meanTerm[tmpDataset.InputVariables[i]] /= Convert.ToDouble(tmpDataset.ListRow.Count);
                termMark[tmpDataset.InputVariables[i]] = Math.Abs(meanTerm[tmpDataset.InputVariables[i]] - median);
                listValue[tmpDataset.InputVariables[i]].Clear();
            }

            return termMark;
        }
        #endregion

        #region public_function
        public override string ToString()
        {
            return "MM";
        }
        public Dictionary<Variables, double> Run()
        {
            if (this.dataset == null)
            {
                return null;
            }

            Dataset tmpDataset = this.dataset.Copy();
            int numRow = tmpDataset.ListRow.Count;
            Dictionary<Variables, double> termMark = CalculateTermMark(tmpDataset);
            return termMark;
        }
        /// <summary>
        /// Runs with running time calculation
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<Dictionary<Variables, double>, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            Dictionary<Variables, double> ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            return new KeyValuePair<Dictionary<Variables, double>, long>(ans, elapsedTime);
        }
        #endregion

        #region Implementation IDispersionMeasure
        public Dictionary<Variables, double> Run(Dataset dataset)
        {
            this.dataset = dataset;
            return this.Run();
        }

        public Dictionary<Variables, double> Run(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            return this.Run();
        }
        public KeyValuePair<Dictionary<Variables, double>, long> RunWithTime(Dataset dataset)
        {
            this.dataset = dataset;
            return this.RunWithTime();
        }

        public KeyValuePair<Dictionary<Variables, double>, long> RunWithTime(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            return this.RunWithTime();
        }
        #endregion
    }
}

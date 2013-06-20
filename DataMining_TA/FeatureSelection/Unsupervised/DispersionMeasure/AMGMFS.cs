// <copyright file="AMGMFS.cs">
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
// <summary>Class representing a AMGMFS.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;

namespace FeatureSelection.Unsupervised.DispersionMeasure
{
    /// <summary>
    /// Dispersion Measure for Relevance Filter using AM-GM
    /// This is a Unsupervised Method so it's don't need a class/label in the process
    /// Directly Implemented from journal 
    /// "Efficient Feature Selection Filters for High-Dimensional Data" 
    /// (Ferreira et all, 2012)
    /// </summary>
    public class AMGMFS : IDispersionMeasure
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
        /// Initializes a new instance of the <see cref="AMGMFS"/> class.
        /// </summary>
        public AMGMFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMGMFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public AMGMFS(Dataset dataset)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMGMFS"/> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public AMGMFS(int maxFeature)
        {
            this.dataset = null;
            this.maxFeature = maxFeature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMGMFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public AMGMFS(Dataset dataset, int maxFeature)
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
            Dictionary<Variables, double> totalExp = new Dictionary<Variables, double>();

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach (Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    double valnow = Convert.ToDouble(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                    if (!meanTerm.ContainsKey(var)) meanTerm[var] = 0.0;
                    meanTerm[var] += valnow;

                    if (!totalExp.ContainsKey(var)) totalExp[var] = 0.0;
                    totalExp[var] += Math.Exp(valnow);
                }
            }

            for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                //totalExp[tmpDataset.InputVariables[i]] += (Math.Exp(0.0) * (tmpDataset.ListRow.Count - tmpDataset.InputVariables[i].RowFrequency));
                meanTerm[tmpDataset.InputVariables[i]] /= Convert.ToDouble(tmpDataset.ListRow.Count);
                double denom = tmpDataset.ListRow.Count * Math.Exp(meanTerm[tmpDataset.InputVariables[i]]);
                double nominator = totalExp[tmpDataset.InputVariables[i]];
                termMark[tmpDataset.InputVariables[i]] = nominator / denom;
            }
            return termMark;
        }
        #endregion

        #region public_function
        public override string ToString()
        {
            return "AMGM";
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

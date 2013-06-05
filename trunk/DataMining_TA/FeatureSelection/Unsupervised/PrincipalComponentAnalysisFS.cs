﻿// <copyright file="PrincipalComponentAnalysisFS.cs">
// Copyright (c) 05-22-2013 All Right Reserved
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
// <date>05-22-2013</date>
// <summary>Class representing a PrincipalComponentAnalysisFS.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;

using Accord.Statistics.Analysis;
using Accord.Controls;
using Accord.Math;
using Accord.Statistics.Formats;

namespace FeatureSelection.Unsupervised
{
    /// <summary>
    /// Feature Ranking/Selection/Reduction using Principal Component Analysis
    /// This is a Unsupervised Method so it's don't need a class/label in the process
    /// Directly Implemented from journal 
    /// "A two-stage feature selection method for text categorization by using
    /// information gain, principal component analysis, and genetic algorithm"
    /// (Oguz,2011)
    /// </summary>
    public class PrincipalComponentAnalysisFS : IUnsupervisedFS
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
        /// Initializes a new instance of the <see cref="PrincipalComponentAnalysisFS"/> class.
        /// </summary>
        public PrincipalComponentAnalysisFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalComponentAnalysisFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public PrincipalComponentAnalysisFS(Dataset dataset)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalComponentAnalysisFS"/> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public PrincipalComponentAnalysisFS(int maxFeature)
        {
            this.dataset = null;
            this.maxFeature = maxFeature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalComponentAnalysisFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public PrincipalComponentAnalysisFS(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
        }
        #endregion

        #region public_function
        public Dataset Run()
        {
            if (this.dataset == null)
            {
                return this.dataset;
            }

            List<Variables> RemovedVariables = new List<Variables>();
            Dataset tmpDataset = this.dataset.Copy();
            int numRow = tmpDataset.ListRow.Count;
            Dictionary<Variables,double> meanTerm = new Dictionary<Variables, double>();
            Dictionary<Variables,double> termMark = new Dictionary<Variables,double>();

            /* Normalize Data */
            /*
            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach(Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    meanTerm[var] += Convert.ToDouble(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                }
            }

            for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                meanTerm[tmpDataset.InputVariables[i]] /= Convert.ToDouble(tmpDataset.InputVariables[i].RowFrequency);
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach (Variables var in tmpDataset.ListRow[i].InputValue.Keys.ToList())
                {
                    tmpDataset.ListRow[i].InputValue[var].ValueCell = Convert.ToDouble(tmpDataset.ListRow[i].InputValue[var].ValueCell) - meanTerm[var];
                }
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach (Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    meanTerm[var] += Convert.ToDouble(tmpDataset.ListRow[i].InputValue[var]);
                }
            }
            */
            /* Covariance Matrix */
            /*for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                termMark[tmpDataset.InputVariables[i]] = 0.0;
                for (int j = 0; j < tmpDataset.ListRow.Count; j++)
                {
                    if (!tmpDataset.ListRow[j].InputValue.ContainsKey(tmpDataset.InputVariables[i])) continue;
                    for (int k = j + 1; k < tmpDataset.ListRow.Count; k++)
                    {
                        if (!tmpDataset.ListRow[k].InputValue.ContainsKey(tmpDataset.InputVariables[i])) continue;
                        termMark[tmpDataset.InputVariables[i]] += (Convert.ToDouble(tmpDataset.ListRow[j].InputValue[tmpDataset.InputVariables[i]].ValueCell) * Convert.ToDouble(tmpDataset.ListRow[k].InputValue[tmpDataset.InputVariables[i]].ValueCell));
                    }
                }
                if (termMark[tmpDataset.InputVariables[i]] < 0.1)
                {
                    RemovedVariables.Add(tmpDataset.InputVariables[i]);
                }
            }
            */

            /* Using Accord .NET library PCA */
            double[][]sourceMatrix = new double[tmpDataset.ListRow.Count][];
            for (int i = 0; i < sourceMatrix.Length; i++)
            {
                sourceMatrix[i] = new double[tmpDataset.InputVariables.Count];
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                for (int j = 0; j < tmpDataset.InputVariables.Count; j++)
                {
                    sourceMatrix[i][j] = 0.0;
                    if (tmpDataset.ListRow[i].InputValue.ContainsKey(tmpDataset.InputVariables[j]))
                    {
                        sourceMatrix[i][j] = Convert.ToDouble(tmpDataset.ListRow[i].InputValue[tmpDataset.InputVariables[j]].ValueCell);
                    }
                }
            }

            // Creates the Principal Component Analysis of the given source
            //PrincipalComponentAnalysis pca = new PrincipalComponentAnalysis(sourceMatrix);


            // Compute the Principal Component Analysis
            //pca.Compute();

            // Creates a projection considering 80% of the information
            //double[,] components = pca.Transform(sourceMatrix, 8 / 10 * tmpDataset.InputVariables.Count);

            //double[,] test = components;
            for (int i = 0; i < RemovedVariables.Count; i++)
            {
                tmpDataset.InputVariables.Remove(RemovedVariables[i]);
            }

            // sort term by its IG value (Decreasing Order)
            tmpDataset.InputVariables.Sort((t1, t2) => termMark[t2].CompareTo(termMark[t1])); 

            // If number of Term > Max Feature then remove some lowest mark Term

            while (tmpDataset.InputVariables.Count > maxFeature)
            {
                Variables lastVar = tmpDataset.InputVariables.Last();
                RemovedVariables.Add(lastVar);
                tmpDataset.InputVariables.Remove(lastVar);
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                for (int j = 0; j < RemovedVariables.Count; j++)
                {
                    tmpDataset.ListRow[i].InputValue.Remove(RemovedVariables[j]);
                }
            }

            return tmpDataset;
        }
        /// <summary>
        /// Runs with running time calculation
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<Dataset, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            Dataset ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            return new KeyValuePair<Dataset, long>(ans, elapsedTime);
        }
        #endregion

        #region Implementation IDispersionMeasure
        public Dataset Run(Dataset dataset)
        {
            this.dataset = dataset;
            return this.Run();
        }

        public Dataset Run(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            return this.Run();
        }
        public KeyValuePair<Dataset, long> RunWithTime(Dataset dataset)
        {
            this.dataset = dataset;
            return this.RunWithTime();
        }

        public KeyValuePair<Dataset, long> RunWithTime(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            return this.RunWithTime();
        }
        #endregion
        
    }
}

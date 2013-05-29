// <copyright file="DocumentFrequencyFS.cs">
// Copyright (c) 05-29-2013 All Right Reserved
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
// <date>05-29-2013</date>
// <summary>Class representing a DocumentFrequencyFS.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;

namespace FeatureSelection.Unsupervised
{
    /// <summary>
    /// Feature Ranking/Selection/Reduction using Document Frequency
    /// This is a Unsupervised Method so it's don't need a class/label in the process
    /// </summary>
    public class DocumentFrequencyFS : IUnsupervisedFS
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
        /// Initializes a new instance of the <see cref="DocumentFrequencyFS"/> class.
        /// </summary>
        public DocumentFrequencyFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFrequencyFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public DocumentFrequencyFS(Dataset dataset)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFrequencyFS"/> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public DocumentFrequencyFS(int maxFeature)
        {
            this.dataset = null;
            this.maxFeature = maxFeature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFrequencyFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public DocumentFrequencyFS(Dataset dataset, int maxFeature)
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
            Dictionary<Variables,double> termMark = new Dictionary<Variables,double>();

            for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                termMark[tmpDataset.InputVariables[i]] = tmpDataset.InputVariables[i].RowFrequency;
            }

            for (int i = 0; i < RemovedVariables.Count; i++)
            {
                tmpDataset.InputVariables.Remove(RemovedVariables[i]);
            }

            // sort term by its IG value (Decreasing Order)
            tmpDataset.InputVariables.Sort((t1, t2) => termMark[t2].CompareTo(termMark[t1])); //still unsure with this delegate >_<
            /*
            for(int i = 0;i < tmpDataset.InputVariables.Count;i++)
            {
                Console.WriteLine(tmpDataset.InputVariables[i].NameVariables + " : " + termMark[tmpDataset.InputVariables[i]]);
            }
            */

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

            tmpDataset.TitleDataset = "DFFS - " + tmpDataset.TitleDataset;
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

        #region Implementation ISupervisedFS
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

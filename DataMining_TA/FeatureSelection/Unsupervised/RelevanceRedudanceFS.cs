// <copyright file="RelevanceRedudanceFS.cs">
// Copyright (c) 06-03-2013 All Right Reserved
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
// <date>06-03-2013</date>
// <summary>Class representing a RelevanceRedudanceFS.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;
using FeatureSelection.Unsupervised.DispersionMeasure;
using FeatureSelection.Unsupervised.SimilarityMeasure;

namespace FeatureSelection.Unsupervised
{
    /// <summary>
    /// Feature Ranking/Selection/Reduction using Relevance Redundance Feature Selection
    /// This is a Unsupervised Method so it's don't need a class/label in the process
    /// Directly Implemented from journal 
    /// "Efficient Feature Selection Filters for High-Dimensional Data" 
    /// (Ferreira et all, 2012)
    /// </summary>
    public class RelevanceRedudanceFS : IUnsupervisedFS
    {
        #region private_or_protected_properties
        /// <summary>
        /// The Maximum Feature to keep
        /// Set default to Integer Maximum
        /// (means that this method only rank dataset's features)
        /// </summary>
        private int maxFeature;
        private Dataset dataset;
        /// <summary>
        /// L Parameter
        /// Parameter for filter number of feature dynamically
        /// </summary>
        private double paramL;
        /// <summary>
        /// MS Parameter
        /// Maximum Similarity Value between 2 Features
        /// to be represented as a different feature(not redundant)
        /// </summary>
        private double paramMS;
        /// <summary>
        /// The dispersion measure method
        /// </summary>
        private IDispersionMeasure dispersionMeasureMethod;
        /// <summary>
        /// The similarity measure method
        /// </summary>
        private ISimilarityMeasure similarityMeasureMethod;
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
        public double ParamL
        {
          get { return paramL; }
          set { paramL = value; }
        }
        public IDispersionMeasure DispersionMeasureMethod
        {
            get { return dispersionMeasureMethod; }
            set { dispersionMeasureMethod = value; }
        }
        public ISimilarityMeasure SimilarityMeasureMethod
        {
          get { return similarityMeasureMethod; }
          set { similarityMeasureMethod = value; }
        }
        public double ParamMS
        {
            get { return paramMS; }
            set { paramMS = value; }
        }
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS"/> class.
        /// </summary>
        public RelevanceRedudanceFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
            this.paramL = 0.95;
            this.paramMS = 0.8;
            this.dispersionMeasureMethod = new MeanMedianFS();
            this.similarityMeasureMethod = new AbsoluteCosineSimilarity();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public RelevanceRedudanceFS(Dataset dataset)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
            this.paramL = 0.95;
            this.paramMS = 0.8;
            this.dispersionMeasureMethod = new MeanMedianFS();
            this.similarityMeasureMethod = new AbsoluteCosineSimilarity(dataset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS" /> class.
        /// </summary>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        /// <param name="similarityMeasureMethod">The similarity measure method.</param>
        public RelevanceRedudanceFS(IDispersionMeasure dispersionMeasureMethod, ISimilarityMeasure similarityMeasureMethod)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
            this.paramL = 0.95;
            this.paramMS = 0.8;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
            this.similarityMeasureMethod = similarityMeasureMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS" /> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        /// <param name="similarityMeasureMethod">The similarity measure method.</param>
        public RelevanceRedudanceFS(Dataset dataset, IDispersionMeasure dispersionMeasureMethod, ISimilarityMeasure similarityMeasureMethod)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
            this.paramL = 0.95;
            this.paramMS = 0.8;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
            this.similarityMeasureMethod = similarityMeasureMethod;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS" /> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        /// <param name="paramL">The parameter L.</param>
        /// <param name="paramMS">The parameter MS.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        /// <param name="similarityMeasureMethod">The similarity measure method.</param>
        public RelevanceRedudanceFS(int maxFeature, double paramL,double paramMS, IDispersionMeasure dispersionMeasureMethod,ISimilarityMeasure similarityMeasureMethod)
        {
            this.dataset = null;
            this.maxFeature = maxFeature;
            this.paramL = paramL;
            this.paramMS = paramMS;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
            this.similarityMeasureMethod = similarityMeasureMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS" /> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        /// <param name="paramL">The parameter L.</param>
        /// <param name="paramMS">The parameter MS.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceRedudanceFS(Dataset dataset, int maxFeature, double paramL,double paramMS)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            this.paramL = paramL;
            this.paramMS = paramMS;
            this.dispersionMeasureMethod = new MeanMedianFS();
            this.similarityMeasureMethod = new AbsoluteCosineSimilarity(dataset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceRedudanceFS" /> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        /// <param name="paramL">The parameter L.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceRedudanceFS(Dataset dataset, int maxFeature, double paramL, double paramMS, IDispersionMeasure dispersionMeasureMethod, ISimilarityMeasure similarityMeasureMethod)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            this.paramL = paramL;
            this.paramMS = paramMS;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
            this.similarityMeasureMethod = similarityMeasureMethod;
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
            Dictionary<Variables, double> termMark = dispersionMeasureMethod.Run(tmpDataset);
            double totalMark = 0.0;
            foreach (Variables var in termMark.Keys)
            {
                totalMark += termMark[var];
            }

            // sort term by its value (Decreasing Order)
            tmpDataset.InputVariables.Sort((t1, t2) => termMark[t2].CompareTo(termMark[t1]));

            // filter by value of L
            double markNow = totalMark;
            while(tmpDataset.InputVariables.Count > 0)
            {
                Variables lastVar = tmpDataset.InputVariables.Last();
                markNow -= termMark[lastVar];
                double percent = markNow / totalMark;
                if (percent < paramL)break;
                RemovedVariables.Add(lastVar);
                tmpDataset.InputVariables.Remove(lastVar);
            }
            
            // filter redudancy by similarity measure
            Variables pivotVar = tmpDataset.InputVariables[0];
            foreach (Variables var in tmpDataset.InputVariables.ToList())
            {
                if (var == pivotVar) continue;
                if (tmpDataset.InputVariables.Count == maxFeature || similarityMeasureMethod.Run(tmpDataset, pivotVar, var) >= paramMS)
                {
                    RemovedVariables.Add(var);
                    tmpDataset.InputVariables.Remove(var);
                    //Console.WriteLine("remove " + var.NameVariables);
                }
                else
                {
                    pivotVar = var;
                }
            }

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

            tmpDataset.TitleDataset += tmpDataset.InputVariables.Count +"Var-Relevance" + dispersionMeasureMethod.ToString() + "-" + tmpDataset.TitleDataset;
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

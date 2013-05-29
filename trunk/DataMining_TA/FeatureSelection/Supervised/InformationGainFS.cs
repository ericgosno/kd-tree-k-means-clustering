// <copyright file="InformationGainFS.cs">
// Copyright (c) 05-20-2013 All Right Reserved
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
// <date>05-20-2013</date>
// <summary>Class representing a InformationGainFS.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;

namespace FeatureSelection.Supervised
{
    /// <summary>
    /// Feature Ranking/Selection/Reduction using Information Gain
    /// This is a Supervised Method so it's need a class/label in the process
    /// Directly Implemented from journal 
    /// "A two-stage feature selection method for text categorization by using
    /// information gain, principal component analysis, and genetic algorithm"
    /// (Oguz,2011)
    /// </summary>
    public class InformationGainFS : ISupervisedFS
    {
        #region private_or_protected_properties
        /// <summary>
        /// The Maximum Feature to keep
        /// Set default to Integer Maximum
        /// (means that this method only rank dataset's features)
        /// </summary>
        private int maxFeature;
        private Dataset dataset;
        private Variables outputVariable;
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
        public Variables OutputVariable
        {
            get { return outputVariable; }
            set { outputVariable = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="InformationGainFS"/> class.
        /// </summary>
        public InformationGainFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
            this.outputVariable = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationGainFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="outputVariable">Class/Label Variable</param>
        public InformationGainFS(Dataset dataset, Variables outputVariable)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
            this.outputVariable = outputVariable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationGainFS"/> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public InformationGainFS(int maxFeature)
        {
            this.dataset = null;
            this.outputVariable = null;
            this.maxFeature = maxFeature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationGainFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="outputVariable">Class/Label Variable</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        public InformationGainFS(Dataset dataset, Variables outputVariable, int maxFeature)
        {
            this.dataset = dataset;
            this.outputVariable = outputVariable;
            this.maxFeature = maxFeature;
        }
        #endregion

        #region public_function
        public Dataset Run()
        {
            if (this.dataset == null || this.outputVariable == null || !this.dataset.OutputVariables.Contains(outputVariable))
            {
                return this.dataset;
            }

            List<Variables> RemovedVariables = new List<Variables>();
            Dataset tmpDataset = this.dataset.Copy();
            int numRow = tmpDataset.ListRow.Count;
            Dictionary<object, List<Row>> dictClass = new Dictionary<object, List<Row>>();
            Dictionary<Variables, double> dictTerm = new Dictionary<Variables,double>();
            Dictionary<object, Dictionary<Variables, int>> dictClassTerm = new Dictionary<object, Dictionary<Variables, int>>();
            
            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                if (!tmpDataset.ListRow[i].OutputValue.ContainsKey(outputVariable))
                {
                    numRow--;
                    continue;
                }
                object valNow = tmpDataset.ListRow[i].OutputValue[outputVariable].ValueCell;

                if(!dictClassTerm.ContainsKey(valNow))dictClassTerm[valNow] = new Dictionary<Variables,int>();

                if (!dictClass.ContainsKey(valNow))
                {
                    dictClass[valNow] = new List<Row>();
                }
                dictClass[valNow].Add(tmpDataset.ListRow[i]);

                foreach(Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    if(!dictTerm.ContainsKey(var))dictTerm[var] = 0.0;
                    dictTerm[var] += 1.0;
                    if(!dictClassTerm[valNow].ContainsKey(var))dictClassTerm[valNow][var] = 0;
                    dictClassTerm[valNow][var]++;
                }
            }

            foreach(Variables var in dictTerm.Keys.ToList())
            {
                dictTerm[var] /= Convert.ToDouble(numRow);
            }

            // Find total Entropy
            double totalEntropy = 0;
            foreach (object obj in dictClass.Keys)
            { 
                double probClass = Convert.ToDouble(dictClass[obj].Count) / Convert.ToDouble(numRow);
                double entClass = probClass * Math.Log(probClass);
                totalEntropy += entClass;
            }
            totalEntropy *= -1.0;

            // Find Information Gain per Term
            Dictionary<Variables,double> termMark = new Dictionary<Variables,double>();
            for(int j = 0;j < tmpDataset.InputVariables.Count;j++)
            {
                Variables var = tmpDataset.InputVariables[j];
                double posEnt = 0.0; 
                double negEnt = 0.0;

                foreach (object obj in dictClass.Keys)
                {
                    int haveTerm = 0;
                    // optimize with simple memoization
                    if (dictClassTerm.ContainsKey(obj)  && dictClassTerm[obj].ContainsKey(var))
                    {
                        haveTerm = dictClassTerm[obj][var]; 
                    }
                    /*
                    for (int i = 0; i < dictClass[obj].Count; i++)
                    {
                        if (dictClass[obj][i].InputValue.ContainsKey(var)) haveTerm++;
                    }
                     */
                    double probHave = Convert.ToDouble(haveTerm) / Convert.ToDouble(dictClass[obj].Count);
                    double probNot = 1.0 - probHave;
                    double EntHave = probHave * Math.Log(probHave);
                    double EntNot = probNot * Math.Log(probNot);
                    EntHave = (double.IsNaN(EntHave)) ? 0.0 : EntHave;
                    EntNot = (double.IsNaN(EntNot)) ? 0.0 : EntNot;

                    posEnt += EntHave;
                    negEnt += EntNot;
                }
                posEnt *= dictTerm[var];
                negEnt *= (1.0 - dictTerm[var]);
                double totalTermEnt = totalEntropy + posEnt + negEnt;
                if (double.IsNaN(totalTermEnt))
                {
                    RemovedVariables.Add(var);
                }
                termMark[var] = totalTermEnt;
            }

            for (int i = 0; i < RemovedVariables.Count; i++)
            {
                tmpDataset.InputVariables.Remove(RemovedVariables[i]);
            }

            // sort term by its IG value (Decreasing Order)
            tmpDataset.InputVariables.Sort((t1, t2) => termMark[t2].CompareTo(termMark[t1])); //still unsure with this delegate >_<
            Console.WriteLine("max : " + tmpDataset.InputVariables[0].NameVariables + " = " + termMark[tmpDataset.InputVariables[0]]);
            Console.WriteLine("min : " + tmpDataset.InputVariables.Last().NameVariables + " = " + termMark[tmpDataset.InputVariables.Last()]);

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
            tmpDataset.TitleDataset = "IGFS - " + tmpDataset.TitleDataset;
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
        public Dataset Run(Dataset dataset, Variables outputVariable)
        {
            this.dataset = dataset;
            this.outputVariable = outputVariable;
            return this.Run();
        }

        public Dataset Run(Dataset dataset, Variables outputVariable, int maxFeature)
        {
            this.dataset = dataset;
            this.outputVariable = outputVariable;
            this.maxFeature = maxFeature;
            return this.Run();
        }

        public KeyValuePair<Dataset, long> RunWithTime(Dataset dataset, Variables outputVariable)
        {
            this.dataset = dataset;
            this.outputVariable = outputVariable;
            return this.RunWithTime();
        }

        public KeyValuePair<Dataset, long> RunWithTime(Dataset dataset, Variables outputVariable, int maxFeature)
        {
            this.dataset = dataset;
            this.outputVariable = outputVariable;
            this.maxFeature = maxFeature;
            return this.RunWithTime();
        }
        #endregion
    }
}

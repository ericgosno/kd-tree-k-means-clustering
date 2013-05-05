// <copyright file="TFIDF.cs">
// Copyright (c) 05-05-2013 All Right Reserved
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
// <date>05-05-2013</date>
// <summary>Class representing a TFIDF.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFIDF.InverseDocumentFrequency;
using TFIDF.TermFrequency;
using Extension;

namespace TFIDF
{
    /// <summary>
    /// TFIDF Converter Class
    /// Convert Bag of Words Frequency dataset to TFIDF 
    /// </summary>
    public class TFIDF
    {
        #region private_or_protected_properties
        private IInverseDocumentFrequency methodIDF;
        private ITermFrequency methodTF;
        private Dataset dataset;
        #endregion

        #region public_properties
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }

        public IInverseDocumentFrequency MethodIDF
        {
            get { return methodIDF; }
            set { methodIDF = value; }
        }

        public ITermFrequency MethodTF
        {
            get { return methodTF; }
            set { methodTF = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TFIDF"/> class.
        /// </summary>
        public TFIDF()
        {
            this.dataset = new Dataset();
            this.methodTF = new LogarithmTermFrequency();
            this.methodIDF = new LogarithmInverseDocumentFrequency();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TFIDF"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public TFIDF(Dataset dataset)
        {
            this.dataset = dataset;
            this.methodTF = new LogarithmTermFrequency();
            this.methodIDF = new LogarithmInverseDocumentFrequency();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TFIDF"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="methodTF">The method TF.</param>
        /// <param name="methodIDF">The method IDF.</param>
        public TFIDF(Dataset dataset, ITermFrequency methodTF, IInverseDocumentFrequency methodIDF)
        {
            this.dataset = dataset;
            this.methodTF = methodTF;
            this.methodIDF = methodIDF;
        }
        #endregion

        #region public_function

        /// <summary>
        /// Calculates the frequency.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        public Dataset CalculateFrequency(Dataset dataset)
        {
            Dataset tmpDataset = this.dataset;
            for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                tmpDataset.InputVariables[i].RowFrequency = -1;
                tmpDataset.InputVariables[i].TermFrequency = -1;
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach (Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    if (var.RowFrequency < 0)var.RowFrequency = 1;
                    else var.RowFrequency++;
                    if(var.TermFrequency < 0)var.TermFrequency = Convert.ToInt32(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                    else var.TermFrequency += Convert.ToInt32(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                }
            }
            return tmpDataset;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public Dataset Run()
        {
            if (this.dataset == null || this.dataset.InputVariables.Count == 0 || this.dataset.ListRow.Count == 0)
            {
                return this.dataset;
            }
            Dataset tmpDataset = this.dataset;
            if(!tmpDataset.IsCalculatedFrequency)tmpDataset = CalculateFrequency(tmpDataset);
            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach(Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    int freq = Convert.ToInt32(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                    tmpDataset.ListRow[i].InputValue[var].ValueCell = (double)(methodTF.CalculateTermFrequency(freq) * methodIDF.CalculateInverseDocumentFrequency(tmpDataset.ListRow.Count, var.RowFrequency));
                }
            }
            return tmpDataset;
        }

        /// <summary>
        /// Runs the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        public Dataset Run(Dataset dataset)
        {
            this.dataset = dataset;
            return this.Run();
        }
        #endregion
    }
}

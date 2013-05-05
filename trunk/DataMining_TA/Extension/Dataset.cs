// <copyright file="Dataset.cs">
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
// <summary>Class representing a Dataset.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    /// <summary>
    /// Class Dataset
    /// Contains List Row, List Variables, and List Class
    /// Core Information for Data Processing
    /// </summary>
    public class Dataset
    {
        #region private_or_protected_properties
        private List<Row> listRow;
        private List<Variables> inputVariables;
        private List<Variables> outputVariables;
        private string titleDataset;
        private bool isCalculatedFrequency;
        #endregion

        #region public_properties
        public bool IsCalculatedFrequency
        {
            get { return isCalculatedFrequency; }
            set { isCalculatedFrequency = value; }
        }
        public string TitleDataset
        {
            get { return titleDataset; }
            set { titleDataset = value; }
        }

        public List<Row> ListRow
        {
            get { return listRow; }
            set { listRow = value; }
        }

        public List<Variables> InputVariables
        {
            get { return inputVariables; }
            set { inputVariables = value; }
        }

        public List<Variables> OutputVariables
        {
            get { return outputVariables; }
            set { outputVariables = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Dataset"/> class.
        /// </summary>
        public Dataset()
        {
            listRow = new List<Row>();
            inputVariables = new List<Variables>();
            outputVariables = new List<Variables>();
            titleDataset = this.GetHashCode().ToString();
            isCalculatedFrequency = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Dataset"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public Dataset(string title)
        {
            listRow = new List<Row>();
            inputVariables = new List<Variables>();
            outputVariables = new List<Variables>();
            isCalculatedFrequency = false;
            this.titleDataset = title;            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Dataset"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="listRow">The list row.</param>
        /// <param name="inputVariables">The input variables.</param>
        /// <param name="outputVariables">The output variables.</param>
        public Dataset(string title,List<Row> listRow, List<Variables> inputVariables, List<Variables> outputVariables)
        {
            this.listRow = listRow;
            this.inputVariables = inputVariables;
            this.outputVariables = outputVariables;
            this.titleDataset = title;
            this.isCalculatedFrequency = true;
            for (int i = 0; i < inputVariables.Count; i++)
            {
                if (inputVariables[i].RowFrequency < 0 || inputVariables[i].TermFrequency < 0)
                {
                    this.isCalculatedFrequency = false;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Dataset"/> class.
        /// </summary>
        /// <param name="another">Another.</param>
        public Dataset(Dataset another)
        {
            Dataset news = another.Copy();
            this.listRow = news.ListRow;
            this.inputVariables = news.InputVariables;
            this.outputVariables = news.OutputVariables;
            this.titleDataset = news.titleDataset;
            this.isCalculatedFrequency = news.isCalculatedFrequency;
        }
        #endregion

        #region public_function
        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns></returns>
        public Dataset Copy()
        {
            Dataset news = new Dataset();
            news.InputVariables = this.inputVariables;
            news.outputVariables = this.outputVariables;
            news.titleDataset = this.titleDataset;
            news.isCalculatedFrequency = this.isCalculatedFrequency;
            for (int i = 0; i < this.listRow.Count; i++)
            {
                news.listRow.Add(this.listRow[i].Copy());
            }
            return news;
        }

        /// <summary>
        /// Prints the dataset detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDatasetDetail()
        {
            List<string> report = new List<string>();
            report.Add("Dataset Title : " + this.titleDataset);
            report.Add("Input Variable : " + this.inputVariables.Count);
            report.Add("Output Variable : " + this.outputVariables.Count);
            report.Add("Number of Row : " + this.listRow.Count);
            return report;
        }
        #endregion
    }
}

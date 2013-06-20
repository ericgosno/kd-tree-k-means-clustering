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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Extension
{
    /// <summary>
    /// Class Dataset
    /// Contains List Row, List Variables, and List Class
    /// Core Information for Data Processing
    /// </summary>
    [Serializable()]
    public class Dataset : ISerializable
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
            news.inputVariables = new List<Variables>();
            news.outputVariables = new List<Variables>();
            for (int i = 0; i < this.inputVariables.Count; i++)
            {
                news.inputVariables.Add(this.inputVariables[i]);
            }
            for (int i = 0; i < this.outputVariables.Count; i++)
            {
                news.outputVariables.Add(this.outputVariables[i]);
            }

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

        /// <summary>
        /// Prints detail of dataset's input variable
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDatasetInputVariableDetail()
        {
            List<string> report = new List<string>();
            report.Add("Amount Input Variable : " + this.inputVariables.Count);
            for (int i = 0; i < inputVariables.Count; i++)
            {
                report.Add("Variable #" + (i+1).ToString() + " : ");
                report.AddRange(inputVariables[i].PrintVariableDetail());
            }
            return report;
        }

        /// <summary>
        /// Prints detail of dataset's output variable
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDatasetOutputVariableDetail()
        {
            List<string> report = new List<string>();
            report.Add("Amount Output Variable : " + this.outputVariables.Count);
            for (int i = 0; i < outputVariables.Count; i++)
            {
                report.Add("Variable #" + (i + 1).ToString() + " : ");
                report.AddRange(outputVariables[i].PrintVariableDetail());
            }
            return report;
        }

        /// <summary>
        /// Prints the dataset's row detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintDatasetRowDetail()
        {
            return PrintDatasetRowDetail(true, true);
        }

        /// <summary>
        /// Prints the dataset row detail.
        /// </summary>
        /// <param name="printInputRow">if set to <c>true</c> [print input row].</param>
        /// <param name="printOutputRow">if set to <c>true</c> [print output row].</param>
        /// <returns></returns>
        public List<string> PrintDatasetRowDetail(bool printInputRow,bool printOutputRow)
        {
            List<string> report = new List<string>();
            report.Add("Amount Row : " + this.listRow.Count);
            for (int i = 0; i < listRow.Count; i++)
            {
                report.AddRange(listRow[i].PrintRowDetail(printInputRow,printOutputRow));
            }
            return report;
        }

        /// <summary>
        /// Calculates Dataset's Total Entropy
        /// </summary>
        /// <param name="outputVariables">The output variables.</param>
        /// <returns></returns>
        public double CalculateENTotal(Variables outputVariables)
        {
            //Calculate ENTotal
            double ENTotal = 0.0;
            int numPoint = this.listRow.Count;
            Dictionary<object, int> numberPointPerClass = new Dictionary<object, int>();
            foreach (Row row in this.listRow)
            {
                if (!row.OutputValue.ContainsKey(outputVariables))
                {
                    numPoint--;
                    continue;
                }
                object value = row.OutputValue[outputVariables].ValueCell;
                if (outputVariables is ContinueVariable)
                {
                    //Continue Variable
                    // this must be discretized
                    ContinueVariable var = outputVariables as ContinueVariable;
                    double valDouble = Convert.ToDouble(value);
                    object realMark = null;
                    foreach (object obj in var.LimitParamVariables.Keys)
                    {
                        if (var.LimitParamVariables[obj].Key <= valDouble && var.LimitParamVariables[obj].Value >= valDouble)
                        {
                            realMark = obj;
                        }
                    }
                    if (realMark == null)
                    {
                        numPoint--;
                        continue;
                    }
                    value = realMark;
                }
                if (numberPointPerClass.ContainsKey(value)) numberPointPerClass[value]++;
                else numberPointPerClass[value] = 1;
            }
            foreach (int numObj in numberPointPerClass.Values)
            {
                double doubleNum = Convert.ToDouble(numObj) / Convert.ToDouble(numPoint);
                double logval = Math.Log(doubleNum,2.0);
                double all = doubleNum * logval;
                ENTotal += all;
            }
            ENTotal *= -1.0;
            return ENTotal;
        }
        #endregion

        #region implementation Iserializeable
        public Dataset(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.titleDataset = (string)info.GetValue("Title", typeof(string));
            }
            catch (Exception ex) { this.titleDataset = this.GetHashCode().ToString(); }
            try
            {
                this.listRow = (List<Row>)info.GetValue("Rows", typeof(List<Row>));
            }
            catch (Exception ex) { this.listRow = new List<Row>(); }
            try
            {
                this.inputVariables = (List<Variables>)info.GetValue("InputVariables", typeof(List<Variables>));
            }
            catch (Exception ex) { this.inputVariables = new List<Variables>(); }
            try
            {
                this.outputVariables = (List<Variables>)info.GetValue("OutputVariables", typeof(List<Variables>));
            }
            catch (Exception ex) { this.outputVariables = new List<Variables>(); }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Title", this.titleDataset);
            info.AddValue("Rows", this.listRow);
            info.AddValue("InputVariables", this.inputVariables);
            info.AddValue("OutputVariables", this.outputVariables);
        }
        #endregion
    }
}

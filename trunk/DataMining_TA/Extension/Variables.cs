// <copyright file="Variables.cs">
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
// <summary>Class representing a Variables entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extension
{
    /// <summary>
    /// Class Variable 
    /// Base Class of Variable Object
    /// </summary>
    [Serializable()]
    public class Variables : ISerializable
    {
        #region private_or_protected_properties
        protected string nameVariables;
        protected int rowFrequency;
        protected int termFrequency;
        protected KeyValuePair<double, double> limitVariables;
        #endregion

        #region public_properties
        public int RowFrequency
        {
            get { return rowFrequency; }
            set { rowFrequency = value; }
        }

        public int TermFrequency
        {
            get { return termFrequency; }
            set { termFrequency = value; }
        }
        public KeyValuePair<double, double> LimitVariables
        {
            get { return limitVariables; }
            set { limitVariables = value; }
        }
        public string NameVariables
        {
            get { return nameVariables; }
            set { nameVariables = value; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Variables"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        public Variables(string nameVariables)
        {
            this.nameVariables = nameVariables;
            limitVariables = new KeyValuePair<double, double>((double)int.MaxValue,(double)int.MinValue);
            this.rowFrequency = -1;
            this.termFrequency = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variables"/> class.
        /// </summary>
        public Variables()
        {
            this.nameVariables = "Variable#" + this.GetHashCode().ToString();
            limitVariables = new KeyValuePair<double, double>((double)int.MaxValue, (double)int.MinValue);
            this.rowFrequency = -1;
            this.termFrequency = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variables"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="rowFrequency">The row frequency.</param>
        /// <param name="termFrequency">The term frequency.</param>
        public Variables(string nameVariables, int rowFrequency, int termFrequency)
        {
            limitVariables = new KeyValuePair<double, double>((double)int.MaxValue, (double)int.MinValue);
            this.nameVariables = nameVariables;
            this.rowFrequency = rowFrequency;
            this.termFrequency = termFrequency;
        }
        #endregion

        #region public_function
        /// <summary>
        /// Resets the limit variables.
        /// </summary>
        public void ResetLimitVariables()
        {
            this.limitVariables = new KeyValuePair<double, double>((double)int.MaxValue, (double)int.MinValue);
        }


        /// <summary>
        /// Rescales the limit variables.
        /// </summary>
        /// <param name="newVariableValue">The new variable value.</param>
        public void RescaleLimitVariables(double newVariableValue)
        {
            double newMin = Math.Min(this.LimitVariables.Key, newVariableValue);
            double newMax = Math.Max(this.LimitVariables.Value, newVariableValue);
            this.LimitVariables = new KeyValuePair<double, double>(newMin, newMax);
        }

        /// <summary>
        /// Prints Variable detail.
        /// </summary>
        /// <returns></returns>
        public virtual List<string> PrintVariableDetail()
        {
            List<string> report = new List<string>();
            report.Add("Variable Name : " + nameVariables);
            if(rowFrequency > 0)report.Add("Row Frequency : " + rowFrequency);
            if(termFrequency > 0)report.Add("Term Frequency : " + termFrequency);
            if(limitVariables.Key < (double)int.MaxValue)report.Add("Minimum Value : " + limitVariables.Key);
            if (limitVariables.Value > (double)int.MinValue) report.Add("Maximum Value : " + limitVariables.Value);
            return report;
        }
        #endregion

        #region implementation Iserializeable
        public Variables(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.nameVariables = (string)info.GetValue("Title", typeof(string));
            }
            catch (Exception ex) { this.nameVariables = "Variable#" + this.GetHashCode().ToString(); }
            try
            {
                this.limitVariables = (KeyValuePair<double,double>)info.GetValue("limitVariables",typeof(KeyValuePair<double,double>));
            }
            catch(Exception ex1) {this.limitVariables = new KeyValuePair<double,double>(double.MinValue,double.MaxValue); }
            try
            {
                this.rowFrequency = (int)info.GetValue("rowFrequency",typeof(int));
            }
            catch(Exception ex2) {this.rowFrequency = -1;}
            try
            {  
                this.termFrequency = (int)info.GetValue("termFrequency",typeof(int));
            }
            catch(Exception ex3){this.termFrequency = -1;}
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Title", this.nameVariables);
            info.AddValue("rowFrequency", this.rowFrequency);
            info.AddValue("termFrequency", this.termFrequency);
            info.AddValue("limitVariables", this.limitVariables);
        }
        #endregion
    }

}

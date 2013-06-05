// <copyright file="Row.cs">
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
// <summary>Class representing a Row.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extension
{
    /// <summary>
    /// Row Object
    /// Dataset's singular entity/data point
    /// </summary>
    [Serializable()]
    public class Row : ISerializable
    {
        #region private_or_protected_properties
        private Dictionary<Variables, Cell> inputValue;
        private Dictionary<Variables, Cell> outputValue;
        private string rowIdentificator;
        #endregion

        #region public_properties
        public string RowIdentificator
        {
            get { return rowIdentificator; }
            set { rowIdentificator = value; }
        }

        public Dictionary<Variables, Cell> InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        public Dictionary<Variables, Cell> OutputValue
        {
            get { return outputValue; }
            set { outputValue = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        public Row()
        {
            inputValue = new Dictionary<Variables, Cell>();
            outputValue = new Dictionary<Variables, Cell>();
            this.rowIdentificator = this.GetHashCode().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        /// <param name="identificator">The identificator.</param>
        public Row(String identificator)
        {
            inputValue = new Dictionary<Variables, Cell>();
            outputValue = new Dictionary<Variables, Cell>();
            this.rowIdentificator = identificator;
        }
        #endregion

        #region public_function
        /// <summary>
        /// Euclideans the distance.
        /// </summary>
        /// <param name="another">Another.</param>
        /// <returns></returns>
        public double EuclideanDistance(Row another)
        {
            double ans = 0.0;
            foreach (Variables var in this.inputValue.Keys)
            {
                if (another.inputValue.ContainsKey(var))
                {
                    double val = Convert.ToDouble(this.inputValue[var].ValueCell) - Convert.ToDouble(another.inputValue[var].ValueCell);
                    ans += (val * val);
                }
                else
                {
                    double val = Convert.ToDouble(this.inputValue[var].ValueCell);
                    ans += (val * val);
                }
            }
            foreach (Variables var in another.inputValue.Keys)
            {
                if(!this.inputValue.ContainsKey(var))
                {
                    double val = Convert.ToDouble(another.inputValue[var].ValueCell);
                    ans += (val * val);
                }
            }
            ans = Math.Sqrt(ans);
            return ans;
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns></returns>
        public Row Copy()
        {
            Row newRow = new Row(this.rowIdentificator);
            foreach (Cell c in inputValue.Values)
            {
                newRow.inputValue[c.VarCell] = c.Copy();
            }
            foreach (Cell c in outputValue.Values)
            {
                newRow.outputValue[c.VarCell] = c.Copy();
            }
            return newRow;
        }

        /// <summary>
        /// Prints the row detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintRowDetail()
        {
            return PrintRowDetail(true, true);
        }

        /// <summary>
        /// Prints the row detail.
        /// </summary>
        /// <param name="printInputRow">if set to <c>true</c> [print input row].</param>
        /// <param name="printOutputRow">if set to <c>true</c> [print output row].</param>
        /// <returns></returns>
        public List<string> PrintRowDetail(bool printInputRow, bool printOutputRow)
        {
            List<string> report = new List<string>();
            report.Add("Row Name : " + rowIdentificator);
            if (inputValue.Count > 0 && printInputRow)
            {
                report.Add("Input : ");
                foreach (Cell cell in inputValue.Values)
                {
                    report.AddRange(cell.PrintCellDetail());
                }
            }
            if (outputValue.Count > 0 && printOutputRow)
            {
                report.Add("Output : ");
                foreach (Cell cell in outputValue.Values)
                {
                    report.AddRange(cell.PrintCellDetail());
                }
            }
            return report;
        }
        #endregion

        #region implementation Iserializeable
        public Row(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.rowIdentificator = (string)info.GetValue("Title", typeof(string));
            }
            catch (Exception ex) { this.rowIdentificator = this.GetHashCode().ToString(); }
            try
            {
                this.inputValue = (Dictionary<Variables, Cell>)info.GetValue("InputVariables", typeof(Dictionary<Variables, Cell>));
            }
            catch (Exception ex) { this.inputValue = new Dictionary<Variables, Cell>(); }
            try
            {
                this.outputValue = (Dictionary<Variables, Cell>)info.GetValue("OutputVariables", typeof(Dictionary<Variables, Cell>));
            }
            catch (Exception ex) { this.outputValue = new Dictionary<Variables, Cell>(); }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Title", this.rowIdentificator);
            info.AddValue("InputVariables", this.inputValue);
            info.AddValue("OutputVariables", this.outputValue);
        }
        #endregion
    }

}

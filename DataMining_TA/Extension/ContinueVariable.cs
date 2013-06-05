// <copyright file="ContinueVariable.cs">
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
// <summary>Class representing a ContinueVariable.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extension
{
    /// <summary>
    /// ContinueVariable Class
    /// Store information about Continuous Type Variable
    /// </summary>
    [Serializable()]
    public class ContinueVariable : Variables
    {
        #region private_or_protected_properties
        private Dictionary<object, KeyValuePair<double, double>> limitParamVariables;
        #endregion

        #region public_properties
        public Dictionary<object, KeyValuePair<double, double>> LimitParamVariables
        {
            get { return limitParamVariables; }
            set { limitParamVariables = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        public ContinueVariable(string nameVariables)
            : base(nameVariables)
        {
            this.limitParamVariables = new Dictionary<object,KeyValuePair<double,double>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVariable"/> class.
        /// </summary>
        public ContinueVariable()
            : base()
        {
            this.limitParamVariables = new Dictionary<object,KeyValuePair<double,double>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="rowFrequency">The row frequency.</param>
        /// <param name="termFrequency">The term frequency.</param>
        public ContinueVariable(string nameVariables, int rowFrequency, int termFrequency)
            : base(nameVariables,rowFrequency,termFrequency)
        {
            this.limitParamVariables = new Dictionary<object, KeyValuePair<double, double>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVariable"/> class.
        /// </summary>
        /// <param name="limitParamVariables">The limit param variables.</param>
        public ContinueVariable(Dictionary<object,KeyValuePair<double,double>> limitParamVariables)
            : base()
        {
            this.limitParamVariables = limitParamVariables;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="limitParamVariables">The limit param variables.</param>
        public ContinueVariable(string nameVariables, Dictionary<object, KeyValuePair<double, double>> limitParamVariables)
            : base(nameVariables)
        {
            this.limitParamVariables = limitParamVariables;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="rowFrequency">The row frequency.</param>
        /// <param name="termFrequency">The term frequency.</param>
        /// <param name="limitParamVariables">The limit param variables.</param>
        public ContinueVariable(string nameVariables, int rowFrequency, int termFrequency, Dictionary<object, KeyValuePair<double, double>> limitParamVariables)
            : base(nameVariables, rowFrequency, termFrequency)
        {
            this.limitParamVariables = limitParamVariables;
        }
        #endregion

        #region Override Function
        public override List<string> PrintVariableDetail()
        {
            List<string> report = new List<string>();
            report.Add("Variable Type : Continue Variable");
            report.AddRange(base.PrintVariableDetail());
            if (limitParamVariables.Count > 0)
            {
                report.Add("Limit Parameter Variable : ");
                foreach (object paramVar in limitParamVariables.Keys)
                {
                    report.Add("Parameter #" + paramVar.ToString() + " Min : " + limitParamVariables[paramVar].Key + " Max : " + limitParamVariables[paramVar].Value);
                }
            }
            return report;
        }
        #endregion

        #region implementation Iserializeable
        public ContinueVariable(SerializationInfo info, StreamingContext ctxt)
            : base(info,ctxt)
        {
            try
            {
                this.limitParamVariables = (Dictionary<object, KeyValuePair<double, double>>)info.GetValue("limitParamVariables", typeof(Dictionary<object, KeyValuePair<double, double>>));
            }
            catch(Exception ex)
            {
                this.limitParamVariables = new Dictionary<object, KeyValuePair<double, double>>();
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("limitParamVariables", this.limitParamVariables);
        }
        #endregion
    }
}

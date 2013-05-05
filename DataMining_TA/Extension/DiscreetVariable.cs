// <copyright file="DiscreetVariable.cs">
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
// <summary>Class representing a DiscreetVariable.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    /// <summary>
    /// Class DiscreetVariable
    /// Store information about Discreet Type Variable
    /// </summary>
    public class DiscreetVariable :  Variables
    {
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreetVariable"/> class.
        /// </summary>
        public DiscreetVariable()
            : base()
        {
 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreetVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        public DiscreetVariable(string nameVariables)
            : base(nameVariables)
        {
 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreetVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="rowFrequency">The row frequency.</param>
        /// <param name="termFrequency">The term frequency.</param>
        public DiscreetVariable(string nameVariables, int rowFrequency, int termFrequency)
            : base(nameVariables, rowFrequency, termFrequency)
        { 

        }
        #endregion

        #region Override Function
        public override List<string> PrintVariableDetail()
        {
            List<string> report = new List<string>();
            report.Add("Variable Type : Discreet Variable");
            report.AddRange(base.PrintVariableDetail());
            return report;
        }
        #endregion
    }
}

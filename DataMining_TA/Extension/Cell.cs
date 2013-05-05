// <copyright file="Cell.cs">
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
// <summary>Class representing a Cell.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    /// <summary>
    /// Row's Cell Object
    /// Contains Value and Pointer to Variable
    /// </summary>
    public class Cell
    {
        #region private_or_protected_properties
        private Variables varCell;
        private object valueCell;
        #endregion

        #region public_properties
        public Variables VarCell
        {
            get { return varCell; }
            set { varCell = value; }
        }

        public object ValueCell
        {
            get { return valueCell; }
            set { valueCell = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        public Cell()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="varCell">The var cell.</param>
        /// <param name="valueCell">The value cell.</param>
        public Cell(Variables varCell, object valueCell)
        {
            this.varCell = varCell;
            this.valueCell = valueCell;
        }
        #endregion

        #region public_function
        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns></returns>
        public Cell Copy()
        {
            return new Cell(this.varCell, this.valueCell);
        }

        /// <summary>
        /// Prints the cell detail.
        /// </summary>
        /// <returns></returns>
        public List<string> PrintCellDetail()
        {
            List<string> report = new List<string>();
            report.Add("Cell ==> Var : " + varCell.NameVariables + " Value : " + valueCell.ToString());
            return report;
        }
        #endregion
    }

}

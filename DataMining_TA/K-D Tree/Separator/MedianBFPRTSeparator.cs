// <copyright file="MedianBFPRTSeparator.cs">
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
// <summary>Class representing a MedianBFPRTSeparator.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K_D_Tree.Separator
{
    /// <summary>
    /// Separate Bucket by list points's median using BFPRT Method
    /// </summary>
    class MedianBFPRTSeparator :  ISeparator
    {
        #region private_or_protected_properties
        private int constValue;
        #endregion

        #region public_properties
        public int ConstValue
        {
            get { return constValue; }
            set { constValue = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MedianBFPRTSeparator"/> class.
        /// </summary>
        public MedianBFPRTSeparator()
        {
            constValue = 5;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MedianBFPRTSeparator"/> class.
        /// </summary>
        /// <param name="constValue">Constant limit number of point to determine what method use to find median</param>
        public MedianBFPRTSeparator(int constValue)
        {
            this.constValue = constValue;
        }
        #endregion

        #region implementation of ISeparator
        /// <summary>
        /// Runs the specified list point.
        /// </summary>
        /// <param name="listPoint">The list point.</param>
        /// <returns></returns>
        public double Run(List<double> listPoint)
        {
            if (listPoint.Count <= constValue)
            {
                listPoint.Sort();
                return listPoint[listPoint.Count / 2];
            }

            int numGroup = listPoint.Count / 5;
            List<double> next_median = new List<double>();

            for (int i = 0; i < numGroup; i++)
            {
                List<double> group = new List<double>();
                int subLeft = i * 5;
                for (int j = subLeft; j < (subLeft + 5); j++)
                {
                    group.Add(listPoint[j]);
                }
                group.Sort();
                next_median.Add(group[2]);
            }
            return Run(next_median);
        }
        #endregion
    }
}

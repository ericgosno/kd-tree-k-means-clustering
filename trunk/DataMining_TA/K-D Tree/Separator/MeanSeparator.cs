// <copyright file="MeanSeparator.cs">
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
// <summary>Class representing a MeanSeparator.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K_D_Tree.Separator
{
    /// <summary>
    /// Separate Bucket by list points's mean
    /// </summary>
    public class MeanSeparator : ISeparator
    {
        #region implementation of ISeparator
        /// <summary>
        /// Runs the specified list point.
        /// </summary>
        /// <param name="listPoint">The list point.</param>
        /// <returns></returns>
        public double Run(List<double> listPoint)
        {
            double ans = 0.0;
            for (int i = 0; i < listPoint.Count; i++)
            {
                ans += listPoint[i];
            }
            ans /= listPoint.Count;
            return ans;
        }
        #endregion
    }
}

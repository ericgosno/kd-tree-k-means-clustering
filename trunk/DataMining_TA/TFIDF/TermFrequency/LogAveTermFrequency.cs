// <copyright file="LogAveTermFrequency.cs">
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
// <summary>Class representing a LogAveTermFrequency.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    /// <summary>
    /// Term Frequency Calculation using Boolean
    /// </summary>
    public class LogAveTermFrequency : ITermFrequency
    {
        #region private_or_protected_properties
        double averageRawTermFrequency;
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LogAveTermFrequency"/> class.
        /// </summary>
        /// <param name="averageRawTermFrequency">The average raw term frequency.</param>
        public LogAveTermFrequency(double averageRawTermFrequency)
        {
            this.averageRawTermFrequency = averageRawTermFrequency;
        }
        #endregion

        #region implementation of ITermFrequency
        /// <summary>
        /// Calculates the term frequency.
        /// </summary>
        /// <param name="RawTermFrequency">The raw term frequency.</param>
        /// <returns></returns>
        public double CalculateTermFrequency(int RawTermFrequency)
        {
            return (1.0 + Math.Log(Convert.ToDouble(RawTermFrequency))) / (1.0 + Math.Log(averageRawTermFrequency));
        }
        #endregion
    }
}

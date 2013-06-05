// <copyright file="ISimilarityMeasure.cs">
// Copyright (c) 06-03-2013 All Right Reserved
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
// <date>06-03-2013</date>
// <summary>Class representing a ISimilarityMeasure.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace FeatureSelection.Unsupervised.SimilarityMeasure
{
    /// <summary>
    /// Interface for Similarity Measure Method
    /// </summary>
    public interface ISimilarityMeasure
    {
        double Run(Variables var1, Variables var2);
        double Run(Dataset dataset,Variables var1, Variables var2);
    }
}

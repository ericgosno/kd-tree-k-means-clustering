// <copyright file="IClusteringInitialization.cs">
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
// <summary>Class representing a IClusteringInitialization.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Clustering.Initialization
{
    /// <summary>
    /// Interface of Clustering Initialization Method
    /// </summary>
    public interface IClusteringInitialization
    {
        KeyValuePair<List<Row>, long> RunWithTime();
        KeyValuePair<List<Row>, long> RunWithTime(Dataset dataset,int K);
        List<Row> Run();
        List<Row> Run(Dataset dataset, int K);
        List<string> PrintDetail(); 

    }
}

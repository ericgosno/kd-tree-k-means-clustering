// <copyright file="Cluster.cs">
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
// <summary>Class representing a Cluster.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Extension
{
    /// <summary>
    /// Cluster Object
    /// Contains List of Objects as a result of clustering
    /// </summary>
    public class Cluster
    {
        #region private_or_protected_properties
        Row centroid;
        List<Row> memberCluster;
        #endregion

        #region public_properties
        public Row Centroid
        {
            get { return centroid; }
            set { centroid = value; }
        }

        public List<Row> MemberCluster
        {
            get { return memberCluster; }
            set { memberCluster = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Cluster"/> class.
        /// </summary>
        /// <param name="centroid">The centroid.</param>
        public Cluster(Row centroid)
        {
            this.centroid = centroid;
            this.memberCluster = new List<Row>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Cluster"/> class.
        /// </summary>
        public Cluster()
        {
            this.centroid = null;
            this.memberCluster = new List<Row>();
        }
        #endregion

        #region public_function
        public double calculateSSE()
        {
            double temp;
            double ans = 0.0;
            for (int i = 0; i < memberCluster.Count; i++)
            {
                foreach (Variables c in centroid.InputValue.Keys)
                {
                    if (memberCluster[i].InputValue.ContainsKey(c))
                    {
                        temp = Convert.ToDouble(memberCluster[i].InputValue[c].ValueCell) - Convert.ToDouble(centroid.InputValue[c].ValueCell);
                    }
                    else
                    {
                        temp = Convert.ToDouble(centroid.InputValue[c].ValueCell);
                    }
                    ans += (temp * temp);
                }
                foreach (Variables c in memberCluster[i].InputValue.Keys)
                {
                    if (!centroid.InputValue.ContainsKey(c))
                    {
                        temp = Convert.ToDouble(memberCluster[i].InputValue[c].ValueCell);
                        ans += (temp * temp);
                    }
                }                
            }
            return ans;
        }
        #endregion
    }

}

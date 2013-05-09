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
        /// <summary>
        /// Calculates the SSE.
        /// </summary>
        /// <returns></returns>
        public double calculateSSE()
        {
            double totalans = 0.0;
            for (int i = 0; i < memberCluster.Count; i++)
            {
                double dist = memberCluster[i].EuclideanDistance(centroid);
                totalans += (dist * dist);
            }
            return totalans;
        }

        /// <summary>
        /// Calculates Cluster's Total Entropy
        /// </summary>
        /// <param name="outputVariables">The output variables.</param>
        /// <returns></returns>
        public double CalculateENCluster(Variables outputVariables)
        {
            //Calculate ENTotal
            double ENTotal = 0.0;
            int numPoint = this.memberCluster.Count;
            Dictionary<object, int> numberPointPerClass = new Dictionary<object, int>();
            foreach (Row row in this.memberCluster)
            {
                if (!row.OutputValue.ContainsKey(outputVariables))
                {
                    numPoint--;
                    continue;
                }
                object value = row.OutputValue[outputVariables].ValueCell;
                if (outputVariables is ContinueVariable)
                {
                    //Continue Variable
                    // this must be discretized
                    ContinueVariable var = outputVariables as ContinueVariable;
                    double valDouble = Convert.ToDouble(value);
                    object realMark = null;
                    foreach (object obj in var.LimitParamVariables.Keys)
                    {
                        if (var.LimitParamVariables[obj].Key <= valDouble && var.LimitParamVariables[obj].Value >= valDouble)
                        {
                            realMark = obj;
                        }
                    }
                    if (realMark == null)
                    {
                        numPoint--;
                        continue;
                    }
                    value = realMark;
                }
                if (numberPointPerClass.ContainsKey(value)) numberPointPerClass[value]++;
                else numberPointPerClass[value] = 1;
            }
            double tes = 0.0;
            foreach (int numObj in numberPointPerClass.Values)
            {
                double doubleNum = Convert.ToDouble(numObj) / Convert.ToDouble(numPoint);
                tes += doubleNum;
                double lognum = Math.Log(doubleNum, 2.0);
                double ENk = doubleNum * lognum;
                ENTotal += ENk;
            }
            ENTotal *= -1.0;
            return ENTotal;
        }
        #endregion
    }

}

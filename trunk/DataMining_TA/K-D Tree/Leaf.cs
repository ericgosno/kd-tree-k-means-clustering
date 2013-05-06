// <copyright file="Leaf.cs">
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
// <summary>Class representing a Leaf.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace K_D_Tree
{
    /// <summary>
    /// KD-Tree's Leaf Object
    /// </summary>
    public class Leaf : Node
    {
        #region private_or_protected_properties
        protected Row upperBound;
        protected Row lowerBound;
        private List<Row> pointInside;
        private double volume = double.MinValue;
        private double density = double.MinValue;
        private Row midPoint = null;
        #endregion

        #region public_properties
        public Row UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }

        public Row LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }
        public List<Row> PointInside
        {
            get { return pointInside; }
            set { pointInside = value; }
        }
        public double Volume
        {
            get {
                if (volume > double.MinValue) return volume;
                else
                {
                    volume = CalculateVolume();
                    return volume;
                }
            }
        }
        public double Density
        {
            get {
                if (density > double.MinValue) return density;
                else
                {
                    density = CalculateDensity();
                    return density;
                }
            }
        }
        public Row MidPoint
        {
            get 
            {
                if (midPoint == null) midPoint = CalculateMidPoint();
                return midPoint; 
            }
        }
        #endregion

        #region Cosntructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf"/> class.
        /// </summary>
        public Leaf()
            : base()
        {
            this.pointInside = new List<Row>();
            this.lowerBound = new Row();
            this.upperBound = new Row();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf"/> class.
        /// </summary>
        /// <param name="depth">Leaf depth.</param>
        public Leaf(int depth) 
            : base(depth)
        {
            this.pointInside = new List<Row>();
            this.lowerBound = new Row();
            this.upperBound = new Row();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf"/> class.
        /// </summary>
        /// <param name="depth">Leaf depth.</param>
        /// <param name="pointInside">The point inside leaf bucket.</param>
        public Leaf(int depth, List<Row> pointInside)
            : base(depth)
        {
            this.pointInside = pointInside;
            this.Recalculate();
        }
        #endregion

        #region public_function
        /// <summary>
        /// Recalculates volume, mid Point, and density of leaf bucket
        /// </summary>
        public void Recalculate()
        {
            this.lowerBound = CalculateLowerBound();
            this.upperBound = CalculateUpperBound();
            this.volume = CalculateVolume();
            this.midPoint = CalculateMidPoint();
            this.density = CalculateDensity();
            return;
        }
        #endregion

        #region private_function
        /// <summary>
        /// Calculates the lower bound.
        /// </summary>
        /// <returns></returns>
        private Row CalculateLowerBound()
        {
            Row lb = new Row();
            for (int i = 0; i < pointInside.Count; i++)
            {
                foreach (Variables var in pointInside[i].InputValue.Keys)
                {
                    if (lb.InputValue.ContainsKey(var))
                    {
                        lb.InputValue[var].ValueCell = Math.Min(Convert.ToDouble(lb.InputValue[var].ValueCell), Convert.ToDouble(pointInside[i].InputValue[var].ValueCell));
                    }
                    else
                    {
                        Cell newCell = new Cell(var, Convert.ToDouble(pointInside[i].InputValue[var].ValueCell));
                        lb.InputValue.Add(var, newCell);
                    }
                }
            }
            return lb;
        }

        /// <summary>
        /// Calculates the upper bound.
        /// </summary>
        /// <returns></returns>
        private Row CalculateUpperBound()
        {
            Row ub = new Row();
            for (int i = 0; i < pointInside.Count; i++)
            {
                foreach (Variables var in pointInside[i].InputValue.Keys)
                {
                    if (ub.InputValue.ContainsKey(var))
                    {
                        ub.InputValue[var].ValueCell = Math.Max(Convert.ToDouble(ub.InputValue[var].ValueCell), Convert.ToDouble(pointInside[i].InputValue[var].ValueCell));
                    }
                    else
                    {
                        Cell newCell = new Cell(var, Convert.ToDouble(pointInside[i].InputValue[var].ValueCell));
                        ub.InputValue.Add(var, newCell);
                    }
                }
            }
            return ub;
        }

        /// <summary>
        /// Calculates the volume of Leaf Bucket.
        /// </summary>
        /// <returns></returns>
        private double CalculateVolume()
        {
            if (upperBound.InputValue.Count <= 0 && lowerBound.InputValue.Count <= 0)
            {
                return double.MinValue;
            }
            double ans = 1.0;
            int numParameter = 0;

            foreach (Variables var in upperBound.InputValue.Keys)
            {
                double val = 0.0;
                if (lowerBound.InputValue.ContainsKey(var))
                {
                    double val1 = (double)upperBound.InputValue[var].ValueCell;
                    double val2 = (double)lowerBound.InputValue[var].ValueCell;
                    if (val1 > (double)int.MinValue && val1 < (double)int.MaxValue && val2 > (double)int.MinValue && val2 < (double)int.MaxValue)
                        val = Math.Abs(val1 - val2);
                }
                else
                {
                    double val1 = (double)upperBound.InputValue[var].ValueCell;
                    if (val1 > (double)int.MinValue && val1 < (double)int.MaxValue)
                        val = Math.Abs(val1);
                }
                if (val >= 0.1 && Math.Log(val) >= 0.01)
                {
                    ans *= Math.Log(val);
                    numParameter++;
                }
            }
            foreach (Variables var2 in lowerBound.InputValue.Keys)
            {
                double val = 0.0;
                if(!upperBound.InputValue.ContainsKey(var2))
                {
                    
                    double val1 = (double)lowerBound.InputValue[var2].ValueCell;
                    if (val1 > (double)int.MinValue && val1 < (double)int.MaxValue)
                        val = Math.Abs(val1);
                    if (val >= 0.1 && Math.Log(val) >= 1e2)
                    {
                        ans *=  Math.Log(val);
                        numParameter++;
                    }
                }
            }
            ans = Math.Pow(ans, 1.0 / (double)numParameter);
            ans = Math.Exp(ans);
            return ans;
        }

        /// <summary>
        /// Calculates the density of leaf bucket
        /// </summary>
        /// <returns></returns>
        private double CalculateDensity()
        {
            if (this.Volume < 0.0 && this.pointInside.Count <= 0)
            {
                return double.MinValue;
            }
            try
            {
                double ans = (double)this.pointInside.Count / this.Volume;
                return ans;
            }
            catch (Exception ex)
            {
                return double.MinValue;
            }
        }

        /// <summary>
        /// Calculates the mid point of leaf bucket
        /// </summary>
        /// <returns></returns>
        private Row CalculateMidPoint()
        {
            if (this.pointInside.Count <= 0) return null;
            Row midPoint = new Row();
            for(int i = 0;i < this.pointInside.Count;i++)
            {
                foreach(Variables var in this.pointInside[i].InputValue.Keys)
                {
                    if(midPoint.InputValue.ContainsKey(var))
                    {
                        midPoint.InputValue[var].ValueCell = Convert.ToDouble(midPoint.InputValue[var].ValueCell) + Convert.ToDouble(this.pointInside[i].InputValue[var].ValueCell);
                    }
                    else
                    {
                        Cell news = new Cell(var,Convert.ToDouble(this.pointInside[i].InputValue[var].ValueCell));
                        midPoint.InputValue.Add(var, news);
                    }
                }
            }
            foreach (Variables var in midPoint.InputValue.Keys)
            {
                midPoint.InputValue[var].ValueCell = Convert.ToDouble(midPoint.InputValue[var].ValueCell) / Convert.ToDouble(this.pointInside.Count);
            }
            return midPoint;
        }
        #endregion
    }
}

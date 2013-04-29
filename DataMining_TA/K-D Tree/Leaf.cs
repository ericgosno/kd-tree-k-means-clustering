using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace K_D_Tree
{
    public class Leaf : Node
    {
        private List<Row> pointInside;
        private double volume = double.MinValue;
        private double density = double.MinValue;
        private Row midPoint = null;

        #region public_properties
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

        public Leaf()
            : base()
        {
            this.pointInside = new List<Row>();
        }

        public Leaf(int depth) 
            : base(depth)
        {
            this.pointInside = new List<Row>();
        }

        public Leaf(int depth, List<Row> pointInside)
            : base(depth)
        {
            this.pointInside = pointInside;
        }

        public void Recalculate()
        {
            this.volume = CalculateVolume();
            this.midPoint = CalculateMidPoint();
            this.density = CalculateDensity();
            return;
        }

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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    public class Row
    {
        private Dictionary<Variables, Cell> inputValue;
        private Dictionary<Variables, Cell> outputValue;
        private string rowIdentificator;

        #region public_properties
        public string RowIdentificator
        {
            get { return rowIdentificator; }
            set { rowIdentificator = value; }
        }

        public Dictionary<Variables, Cell> InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        public Dictionary<Variables, Cell> OutputValue
        {
            get { return outputValue; }
            set { outputValue = value; }
        }
        #endregion

        public Row()
        {
            inputValue = new Dictionary<Variables, Cell>();
            outputValue = new Dictionary<Variables, Cell>();
            this.rowIdentificator = this.GetHashCode().ToString();
        }

        public Row(String identificator)
        {
            inputValue = new Dictionary<Variables, Cell>();
            outputValue = new Dictionary<Variables, Cell>();
            this.rowIdentificator = identificator;
        }
        public double EuclideanDistance(Row another)
        {
            double ans = 0.0;
            foreach (Variables var in this.inputValue.Keys)
            {
                if (another.inputValue.ContainsKey(var))
                {
                    double val = Convert.ToDouble(this.inputValue[var].ValueCell) - Convert.ToDouble(another.inputValue[var].ValueCell);
                    ans += (val * val);
                }
                else
                {
                    double val = Convert.ToDouble(this.inputValue[var].ValueCell);
                    ans += (val * val);
                }
            }
            foreach (Variables var in another.inputValue.Keys)
            {
                if(!this.inputValue.ContainsKey(var))
                {
                    double val = Convert.ToDouble(another.inputValue[var].ValueCell);
                    ans += (val * val);
                }
            }
            ans = Math.Sqrt(ans);
            return ans;
        }

        public Row Copy()
        {
            Row newRow = new Row(this.rowIdentificator);
            foreach (Cell c in inputValue.Values)
            {
                newRow.inputValue[c.VarCell] = c.Copy();
            }
            foreach (Cell c in outputValue.Values)
            {
                newRow.outputValue[c.VarCell] = c.Copy();
            }
            return newRow;
        }
    }

}

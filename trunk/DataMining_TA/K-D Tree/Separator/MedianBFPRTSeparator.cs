using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K_D_Tree.Separator
{
    class MedianBFPRTSeparator :  ISeparator
    {
        private int constValue;

        public int ConstValue
        {
            get { return constValue; }
            set { constValue = value; }
        }

        public MedianBFPRTSeparator()
        {
            constValue = 5;
        }
        public MedianBFPRTSeparator(int constValue)
        {
            this.constValue = constValue;
        }

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
    }
}

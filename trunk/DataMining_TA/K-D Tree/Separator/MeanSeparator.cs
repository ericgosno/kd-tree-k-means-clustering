using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K_D_Tree.Separator
{
    public class MeanSeparator : ISeparator
    {
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
    }
}

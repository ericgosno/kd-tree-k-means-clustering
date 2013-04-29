using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K_D_Tree.Separator
{
    class MedianSortSelectSeparator : ISeparator
    {
        public double Run(List<double> listPoint)
        {
            listPoint.Sort();
            return listPoint[listPoint.Count / 2];
        }
    }
}

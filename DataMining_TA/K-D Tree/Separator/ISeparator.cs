using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace K_D_Tree.Separator
{
    public interface ISeparator
    {
        double Run(List<double> listPoint);
    }
}

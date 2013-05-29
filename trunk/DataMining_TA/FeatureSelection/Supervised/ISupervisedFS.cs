using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace FeatureSelection.Supervised
{
    public interface ISupervisedFS
    {
        Dataset Run(Dataset dataset, Variables outputVariable);
        Dataset Run(Dataset dataset, Variables outputVariable,int maxFeature);
        KeyValuePair<Dataset, long> RunWithTime(Dataset dataset, Variables outputVariable);
        KeyValuePair<Dataset, long> RunWithTime(Dataset dataset,Variables outputVariable, int maxFeature);
    }
}

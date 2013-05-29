using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace FeatureSelection.Unsupervised
{
    public interface IUnsupervisedFS
    {
        Dataset Run(Dataset dataset);
        Dataset Run(Dataset dataset,int maxFeature);
        KeyValuePair<Dataset, long> RunWithTime(Dataset dataset);
        KeyValuePair<Dataset, long> RunWithTime(Dataset dataset,int maxFeature);
    }
}

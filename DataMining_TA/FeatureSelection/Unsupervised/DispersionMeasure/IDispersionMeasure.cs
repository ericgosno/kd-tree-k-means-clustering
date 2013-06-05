using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace FeatureSelection.Unsupervised.DispersionMeasure
{
    public interface IDispersionMeasure
    {
        Dictionary<Variables, double> Run(Dataset dataset);
        Dictionary<Variables, double> Run(Dataset dataset, int maxFeature);
        KeyValuePair<Dictionary<Variables, double>, long> RunWithTime(Dataset dataset);
        KeyValuePair<Dictionary<Variables, double>, long> RunWithTime(Dataset dataset, int maxFeature);
    }
}

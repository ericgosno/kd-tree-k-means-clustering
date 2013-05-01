using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Clustering
{
    public interface IClustering
    {
        ClusteringResult Run(Dataset dataset, int numCluster, bool isNormalize);
        ClusteringResult Run();
        List<string> PrintClusterResult(object ClusterResult);
    }
}

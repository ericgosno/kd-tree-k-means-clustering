using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Clustering.Initialization
{
    public interface IClusteringInitialization
    {
        KeyValuePair<List<Row>, long> RunWithTime();
        KeyValuePair<List<Row>, long> RunWithTime(Dataset dataset,int K);
        List<Row> Run();
        List<Row> Run(Dataset dataset, int K);
        List<string> PrintDetail(); 

    }
}

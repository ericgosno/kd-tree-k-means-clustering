using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;
using FeatureSelection.Unsupervised;

namespace DebuggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            double percentageToKeep = 1.0;
            int FeatureToKeep = 0;
            Dataset newDataset = null;
            Dataset dataset = null;

            IUnsupervisedFS DFFS = new TermVarianceFS();

            //dataset = PenDigits_Debug.run();
            dataset = Template_BagOfWord_Debug.run();
            dataset = DFFS.Run(dataset);
            //dataset = ImageSegmentation_Debug.run();
            StandardClusteringTesting.run(dataset,20);

            percentageToKeep = 0.80;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "75% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.70;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "70% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.60;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "60% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.40;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "40% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.20;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "20% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.08;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "8% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.04;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "4% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            DFFS = new DocumentFrequencyFS();

            //dataset = PenDigits_Debug.run();
            dataset = Template_BagOfWord_Debug.run();
            dataset = DFFS.Run(dataset);
            //dataset = ImageSegmentation_Debug.run();
            StandardClusteringTesting.run(dataset, 20);

            percentageToKeep = 0.80;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "75% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.70;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "70% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.60;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "60% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.40;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "40% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.20;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "20% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.08;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "8% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);

            percentageToKeep = 0.04;
            FeatureToKeep = Convert.ToInt32(percentageToKeep * Convert.ToDouble(dataset.InputVariables.Count));
            newDataset = DFFS.Run(dataset, FeatureToKeep);
            newDataset.TitleDataset = "4% - " + newDataset.TitleDataset;
            StandardClusteringTesting.run(newDataset, 20);
            
            string hold = Console.ReadLine();
        }
    }
}

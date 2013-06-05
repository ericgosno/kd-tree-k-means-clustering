﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;
using FeatureSelection.Unsupervised;
using FeatureSelection.Unsupervised.DispersionMeasure;
using FeatureSelection.Unsupervised.SimilarityMeasure;

using System.Xml.Serialization;
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
            string base_url = @"E:\5109100153 - Eric\tc\";

            //dataset = PenDigits_Debug.run();
            dataset = Template_BagOfWord_Debug.run();
            

            //Try Serializer
            //Serializer.SerializeObject(base_url + "Dataset - " + dataset.TitleDataset + ".txt", dataset);
            //dataset = (Dataset)Serializer.DeSerializeObject(base_url + "Dataset - Newsgroup Dataset.txt");
            //dataset.Save(base_url + "Dataset - " + dataset.TitleDataset + ".xml");

            IUnsupervisedFS DFFS = new RelevanceRedudanceFS();
            newDataset = DFFS.Run(dataset);
            Serializer.SerializeObject(base_url + "Dataset - " + newDataset.TitleDataset + ".txt", newDataset);
            //dataset = ImageSegmentation_Debug.run();
            StandardClusteringTesting.run(newDataset, 20);

            DFFS = new RelevanceRedudanceFS(new MeanAbsoluteDifferenceFS(), new AbsoluteCosineSimilarity());
            newDataset = DFFS.Run(dataset);
            Serializer.SerializeObject(base_url + "Dataset - " + newDataset.TitleDataset + ".txt", newDataset);
            StandardClusteringTesting.run(newDataset, 20);

            DFFS = new RelevanceRedudanceFS(new AMGMFS(), new AbsoluteCosineSimilarity());
            newDataset = DFFS.Run(dataset);
            Serializer.SerializeObject(base_url + "Dataset - " + newDataset.TitleDataset + ".txt", newDataset);
            StandardClusteringTesting.run(newDataset, 20);

            DFFS = new RelevanceRedudanceFS(new TermVarianceFS(), new AbsoluteCosineSimilarity());
            newDataset = DFFS.Run(dataset);
            Serializer.SerializeObject(base_url + "Dataset - " + newDataset.TitleDataset + ".txt", newDataset);
            StandardClusteringTesting.run(newDataset, 20);

            StandardClusteringTesting.run(dataset, 20);
            Console.WriteLine("finish!");
            string hold = Console.ReadLine();
        }
    }
}

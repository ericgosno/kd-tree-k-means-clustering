using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Extension;
using FeatureSelection.Unsupervised.DispersionMeasure;

namespace FeatureSelection.Unsupervised
{
    /// <summary>
    /// Feature Ranking/Selection/Reduction using Relevance Feature Selection
    /// This is a Unsupervised Method so it's don't need a class/label in the process
    /// Directly Implemented from journal 
    /// "Efficient Feature Selection Filters for High-Dimensional Data" 
    /// (Ferreira et all, 2012)
    /// </summary>
    public class RelevanceFS : IUnsupervisedFS
    {
        #region private_or_protected_properties
        /// <summary>
        /// The Maximum Feature to keep
        /// Set default to Integer Maximum
        /// (means that this method only rank dataset's features)
        /// </summary>
        private int maxFeature;
        private Dataset dataset;
        /// <summary>
        /// L Parameter
        /// Parameter for filter number of feature dynamically
        /// </summary>
        private double paramL;
        /// <summary>
        /// The dispersion measure method
        /// </summary>
        private IDispersionMeasure dispersionMeasureMethod;
        #endregion

        #region public_properties
        public int MaxFeature
        {
            get { return maxFeature; }
            set { maxFeature = value; }
        }
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        public double ParamL
        {
          get { return paramL; }
          set { paramL = value; }
        }
        public IDispersionMeasure DispersionMeasureMethod
        {
            get { return dispersionMeasureMethod; }
            set { dispersionMeasureMethod = value; }
        }
        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS"/> class.
        /// </summary>
        public RelevanceFS()
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
            this.paramL = 0.95;
            this.dispersionMeasureMethod = new MeanMedianFS();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public RelevanceFS(Dataset dataset)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
            this.paramL = 0.95;
            this.dispersionMeasureMethod = new MeanMedianFS();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS" /> class.
        /// </summary>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceFS(IDispersionMeasure dispersionMeasureMethod)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = null;
            this.paramL = 0.95;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS" /> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceFS(Dataset dataset, IDispersionMeasure dispersionMeasureMethod)
        {
            this.maxFeature = int.MaxValue;
            this.dataset = dataset;
            this.paramL = 0.95;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS" /> class.
        /// </summary>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        /// <param name="paramL">The parameter L.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceFS(int maxFeature, double paramL, IDispersionMeasure dispersionMeasureMethod)
        {
            this.dataset = null;
            this.maxFeature = maxFeature;
            this.paramL = paramL;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS" /> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        /// <param name="paramL">The parameter L.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceFS(Dataset dataset, int maxFeature, double paramL)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            this.paramL = paramL;
            this.dispersionMeasureMethod = new MeanMedianFS();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceFS" /> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxFeature">Maximum Number of Features to Keep</param>
        /// <param name="paramL">The parameter L.</param>
        /// <param name="dispersionMeasureMethod">The dispersion measure method.</param>
        public RelevanceFS(Dataset dataset, int maxFeature, double paramL, IDispersionMeasure dispersionMeasureMethod)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            this.paramL = paramL;
            this.dispersionMeasureMethod = dispersionMeasureMethod;
        }

        #endregion

        #region public_function
        public Dataset Run()
        {
            if (this.dataset == null)
            {
                return this.dataset;
            }

            List<Variables> RemovedVariables = new List<Variables>();
            Dataset tmpDataset = this.dataset.Copy();
            int numRow = tmpDataset.ListRow.Count;
            Dictionary<Variables,double> meanTerm = new Dictionary<Variables, double>();
            Dictionary<Variables, double> termMark = dispersionMeasureMethod.Run(tmpDataset);
            double totalMark = 0.0;
            foreach (Variables var in termMark.Keys)
            {
                totalMark += termMark[var];
            }

            // sort term by its value (Decreasing Order)
            tmpDataset.InputVariables.Sort((t1, t2) => termMark[t2].CompareTo(termMark[t1]));

            // filter by value of L
            double markNow = totalMark;
            while(tmpDataset.InputVariables.Count > 0)
            {
                Variables lastVar = tmpDataset.InputVariables.Last();
                markNow -= termMark[lastVar];
                if (markNow / totalMark < paramL) break;
                RemovedVariables.Add(lastVar);
                tmpDataset.InputVariables.Remove(lastVar);
            }

            // If number of Term > Max Feature then remove some lowest mark Term
            while (tmpDataset.InputVariables.Count > maxFeature)
            {
                Variables lastVar = tmpDataset.InputVariables.Last();
                RemovedVariables.Add(lastVar);
                tmpDataset.InputVariables.Remove(lastVar);
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                for (int j = 0; j < RemovedVariables.Count; j++)
                {
                    tmpDataset.ListRow[i].InputValue.Remove(RemovedVariables[j]);
                }
            }

            tmpDataset.TitleDataset += tmpDataset.InputVariables.Count +"Var-Relevance" + dispersionMeasureMethod.ToString() + "-" + tmpDataset.TitleDataset;
            return tmpDataset;
        }
        /// <summary>
        /// Runs with running time calculation
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<Dataset, long> RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            Dataset ans = this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            return new KeyValuePair<Dataset, long>(ans, elapsedTime);
        }
        #endregion

        #region Implementation IDispersionMeasure
        public Dataset Run(Dataset dataset)
        {
            this.dataset = dataset;
            return this.Run();
        }

        public Dataset Run(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            return this.Run();
        }
        public KeyValuePair<Dataset, long> RunWithTime(Dataset dataset)
        {
            this.dataset = dataset;
            return this.RunWithTime();
        }

        public KeyValuePair<Dataset, long> RunWithTime(Dataset dataset, int maxFeature)
        {
            this.dataset = dataset;
            this.maxFeature = maxFeature;
            return this.RunWithTime();
        }
        #endregion
    }
}

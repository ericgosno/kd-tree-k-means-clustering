using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace FeatureSelection.Unsupervised.SimilarityMeasure
{
    /// <summary>
    /// Similarity Measure using Absolute Cosine Similarity
    /// Directly Implemented from
    /// "Efficient Feature Selection Filters for High-Dimensional Data" 
    /// (Ferreira et all, 2012)
    /// </summary>
    public class AbsoluteCosineSimilarity : ISimilarityMeasure
    {
        #region private_or_protected_properties
        private Dataset dataset;
        private Dictionary<Variables, double> totalSquareValue;
        #endregion

        #region public_properties
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }
        #endregion

        #region Constructor
        public AbsoluteCosineSimilarity()
        {
            this.dataset = null;
            this.totalSquareValue = new Dictionary<Variables, double>();
        }

        public AbsoluteCosineSimilarity(Dataset dataset)
        {
            this.dataset = dataset;
            CalculateTotalSquare();
        }
        #endregion

        #region private_function
        private void CalculateTotalSquare()
        {
            this.totalSquareValue = new Dictionary<Variables, double>();
            //calculate totalSquareValue
            for (int i = 0; i < dataset.ListRow.Count; i++)
            {
                foreach (Variables var in dataset.ListRow[i].InputValue.Keys)
                {
                    if (!totalSquareValue.ContainsKey(var)) totalSquareValue[var] = 0.0;
                    totalSquareValue[var] += Math.Pow(Convert.ToDouble(dataset.ListRow[i].InputValue[var].ValueCell), 2.0);
                }
            }
        }

        private double CalculateTotalSquareVariable(Variables var)
        {
            double val = 0.0;
            for (int i = 0; i < dataset.ListRow.Count; i++)
            {
                if(dataset.ListRow[i].InputValue.ContainsKey(var))
                {
                    val += Math.Pow(Convert.ToDouble(dataset.ListRow[i].InputValue[var].ValueCell), 2.0);
                }
            }
            totalSquareValue[var] = val;
            return val;
        }
        #endregion

        #region public_function

        #endregion

        #region implementation ISimilarityMeasure
        public double Run(Variables var1, Variables var2)
        {
            if (dataset == null) return double.NaN;
            double nominator = 0.0;
            double denomvar1 = (totalSquareValue.ContainsKey(var1)) ? totalSquareValue[var1] : CalculateTotalSquareVariable(var1);
            double denomvar2 = (totalSquareValue.ContainsKey(var2)) ? totalSquareValue[var2] : CalculateTotalSquareVariable(var2);
            double denom = Math.Sqrt(denomvar1 * denomvar2);
            for (int i = 0; i < dataset.ListRow.Count; i++)
            {
                double ang1 = 0.0;
                double ang2 = 0.0;
                if(dataset.ListRow[i].InputValue.ContainsKey(var1))ang1 = Convert.ToDouble(dataset.ListRow[i].InputValue[var1].ValueCell);
                if(dataset.ListRow[i].InputValue.ContainsKey(var2)) ang2 = Convert.ToDouble(dataset.ListRow[i].InputValue[var2].ValueCell);
                nominator += (ang1 * ang2);
            }
            return nominator / denom;
        }

        public double Run(Dataset dataset, Variables var1, Variables var2)
        {
            this.dataset = dataset;
            return this.Run(var1, var2);
        }
        #endregion
    }
}

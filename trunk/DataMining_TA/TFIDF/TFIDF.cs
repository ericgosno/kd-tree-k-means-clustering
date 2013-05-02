using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFIDF.InverseDocumentFrequency;
using TFIDF.TermFrequency;
using Extension;

namespace TFIDF
{
    public class TFIDF
    {
        private iInverseDocumentFrequency methodIDF;
        private iTermFrequency methodTF;
        private Dataset dataset;

        #region public_properties
        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }

        public iInverseDocumentFrequency MethodIDF
        {
            get { return methodIDF; }
            set { methodIDF = value; }
        }

        public iTermFrequency MethodTF
        {
            get { return methodTF; }
            set { methodTF = value; }
        }
        #endregion

        #region Constructor
        public TFIDF()
        {
            this.dataset = new Dataset();
            this.methodTF = new LogarithmTermFrequency();
            this.methodIDF = new LogarithmInverseDocumentFrequency();
        }
        public TFIDF(Dataset dataset)
        {
            this.dataset = dataset;
            this.methodTF = new LogarithmTermFrequency();
            this.methodIDF = new LogarithmInverseDocumentFrequency();
        }
        public TFIDF(Dataset dataset, iTermFrequency methodTF, iInverseDocumentFrequency methodIDF)
        {
            this.dataset = dataset;
            this.methodTF = methodTF;
            this.methodIDF = methodIDF;
        }
        #endregion

        public Dataset CalculateFrequency(Dataset dataset)
        {
            Dataset tmpDataset = this.dataset;
            for (int i = 0; i < tmpDataset.InputVariables.Count; i++)
            {
                tmpDataset.InputVariables[i].RowFrequency = -1;
                tmpDataset.InputVariables[i].TermFrequency = -1;
            }

            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach (Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    if (var.RowFrequency < 0)var.RowFrequency = 1;
                    else var.RowFrequency++;
                    if(var.TermFrequency < 0)var.TermFrequency = Convert.ToInt32(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                    else var.TermFrequency += Convert.ToInt32(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                }
            }
            return tmpDataset;
        }

        public Dataset Run()
        {
            if (this.dataset == null || this.dataset.InputVariables.Count == 0 || this.dataset.ListRow.Count == 0)
            {
                return this.dataset;
            }
            Dataset tmpDataset = this.dataset;
            if(!tmpDataset.IsCalculatedFrequency)tmpDataset = CalculateFrequency(tmpDataset);
            for (int i = 0; i < tmpDataset.ListRow.Count; i++)
            {
                foreach(Variables var in tmpDataset.ListRow[i].InputValue.Keys)
                {
                    int freq = Convert.ToInt32(tmpDataset.ListRow[i].InputValue[var].ValueCell);
                    tmpDataset.ListRow[i].InputValue[var].ValueCell = (double)(methodTF.CalculateTermFrequency(freq) * methodIDF.CalculateInverseDocumentFrequency(tmpDataset.ListRow.Count, var.RowFrequency));
                }
            }
            return tmpDataset;
        }

        public Dataset Run(Dataset dataset)
        {
            this.dataset = dataset;
            return this.Run();
        }
    }
}

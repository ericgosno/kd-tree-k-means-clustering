using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    public class Dataset
    {
        private List<Row> listRow;
        private List<Variables> inputVariables;
        private List<Variables> outputVariables;
        private string titleDataset;
        private bool isCalculatedFrequency;


        #region public_properties
        public bool IsCalculatedFrequency
        {
            get { return isCalculatedFrequency; }
            set { isCalculatedFrequency = value; }
        }
        public string TitleDataset
        {
            get { return titleDataset; }
            set { titleDataset = value; }
        }

        public List<Row> ListRow
        {
            get { return listRow; }
            set { listRow = value; }
        }

        public List<Variables> InputVariables
        {
            get { return inputVariables; }
            set { inputVariables = value; }
        }

        public List<Variables> OutputVariables
        {
            get { return outputVariables; }
            set { outputVariables = value; }
        }
        #endregion

        #region constructor
        public Dataset()
        {
            listRow = new List<Row>();
            inputVariables = new List<Variables>();
            outputVariables = new List<Variables>();
            titleDataset = this.GetHashCode().ToString();
            isCalculatedFrequency = false;
        }
        public Dataset(string title)
        {
            listRow = new List<Row>();
            inputVariables = new List<Variables>();
            outputVariables = new List<Variables>();
            isCalculatedFrequency = false;
            this.titleDataset = title;            
        }
        public Dataset(string title,List<Row> listRow, List<Variables> inputVariables, List<Variables> outputVariables)
        {
            this.listRow = listRow;
            this.inputVariables = inputVariables;
            this.outputVariables = outputVariables;
            this.titleDataset = title;
            this.isCalculatedFrequency = true;
            for (int i = 0; i < inputVariables.Count; i++)
            {
                if (inputVariables[i].RowFrequency < 0 || inputVariables[i].TermFrequency < 0)
                {
                    this.isCalculatedFrequency = false;
                }
            }
        }
        public Dataset(Dataset another)
        {
            Dataset news = another.Copy();
            this.listRow = news.ListRow;
            this.inputVariables = news.InputVariables;
            this.outputVariables = news.OutputVariables;
            this.titleDataset = news.titleDataset;
            this.isCalculatedFrequency = news.isCalculatedFrequency;
        }
        #endregion

        public Dataset Copy()
        {
            Dataset news = new Dataset();
            news.InputVariables = this.inputVariables;
            news.outputVariables = this.outputVariables;
            news.titleDataset = this.titleDataset;
            news.isCalculatedFrequency = this.isCalculatedFrequency;
            for (int i = 0; i < this.listRow.Count; i++)
            {
                news.listRow.Add(this.listRow[i].Copy());
            }
            return news;
        }

        public List<string> PrintDetail()
        {
            List<string> report = new List<string>();
            report.Add("Dataset Title : " + this.titleDataset);
            report.Add("Input Variable : " + this.inputVariables.Count);
            report.Add("Output Variable : " + this.outputVariables.Count);
            report.Add("Number of Row : " + this.listRow.Count);
            return report;
        }
    }
}

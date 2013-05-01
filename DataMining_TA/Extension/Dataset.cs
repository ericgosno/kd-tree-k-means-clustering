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

        #region public_properties
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
        }
        public Dataset(List<Row> listRow, List<Variables> inputVariables, List<Variables> outputVariables)
        {
            this.listRow = listRow;
            this.inputVariables = inputVariables;
            this.outputVariables = outputVariables;
        }
        public Dataset(Dataset another)
        {
            Dataset news = another.Copy();
            this.listRow = news.ListRow;
            this.inputVariables = news.InputVariables;
            this.outputVariables = news.OutputVariables;
        }
        #endregion

        public Dataset Copy()
        {
            Dataset news = new Dataset();
            news.InputVariables = this.inputVariables;
            news.outputVariables = this.outputVariables;
            for (int i = 0; i < this.listRow.Count; i++)
            {
                news.listRow.Add(this.listRow[i].Copy());
            }
            return news;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    public class Cell
    {
        private Variables varCell;
        private object valueCell;

        public Variables VarCell
        {
            get { return varCell; }
            set { varCell = value; }
        }

        public object ValueCell
        {
            get { return valueCell; }
            set { valueCell = value; }
        }

        public Cell()
        {
        }
        public Cell(Variables varCell, object valueCell)
        {
            this.varCell = varCell;
            this.valueCell = valueCell;
        }

        public Cell Copy()
        {
            return new Cell(this.varCell, this.valueCell);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;
using System.Diagnostics;
using K_D_Tree.Separator;

namespace K_D_Tree
{
    public class KDTree
    {
        List<Row> listRow;
        Node root;
        List<Variables> listVariables;
        int maxPointInLeaf;
        ISeparator separateMethod;
        List<Leaf> listBucketLeaf;

        #region public_properties
        public List<Row> ListRow
        {
            get { return listRow; }
            set { listRow = value; }
        }
        public Node Root
        {
            get { return root; }
            set { root = value; }
        }
        public List<Variables> ListVariables
        {
            get { return listVariables; }
            set { listVariables = value; }
        }
        public List<Leaf> ListBucketLeaf
        {
            get { return listBucketLeaf; }
            set { listBucketLeaf = value; }
        }
        #endregion

        public KDTree()
        {
            listVariables = new List<Variables>();
            listRow = new List<Row>();
            listBucketLeaf = new List<Leaf>();
            root = null;
            maxPointInLeaf = 1;
            // Default Separator Method
            separateMethod = new MedianBFPRTSeparator();
        }

        public KDTree(List<Row> listRow, List<Variables> listVariables,int maxPointInLeaf)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.root = null;
            this.maxPointInLeaf = maxPointInLeaf;
            // Default Separator Method
            this.separateMethod = new MedianBFPRTSeparator();
        }

        public KDTree(List<Row> listRow, List<Variables> listVariables, int maxPointInLeaf,ISeparator separatorMethod)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.root = null;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
        }

        private void CreateRoot()
        {
            root = new Node(0);
            Row lowerBound = new Row();
            Row upperBound = new Row();
            for (int i = 0; i < listVariables.Count; i++)
            {
                double lower = (listVariables[i].LimitVariables.Key < (double)int.MaxValue) ? listVariables[i].LimitVariables.Key : 0.0;
                double upper = (listVariables[i].LimitVariables.Value > (double)int.MinValue) ? listVariables[i].LimitVariables.Value : 0.0;
                Cell low = new Cell(listVariables[i],listVariables[i].LimitVariables.Key);
                lowerBound.InputValue.Add(low.VarCell, low);
                Cell up = new Cell(listVariables[i], listVariables[i].LimitVariables.Value);
                upperBound.InputValue.Add(up.VarCell, up);
            }
            root.LowerBound = lowerBound;
            root.UpperBound = upperBound;
        }

        private void CreateRootLeaf(List<Row> rows)
        {
            root = new Leaf(0, rows);
            Row lowerBound = new Row();
            Row upperBound = new Row();
            for (int i = 0; i < listVariables.Count; i++)
            {
                Cell low = new Cell(listVariables[i], listVariables[i].LimitVariables.Key);
                lowerBound.InputValue.Add(low.VarCell, low);
                Cell up = new Cell(listVariables[i], listVariables[i].LimitVariables.Value);
                upperBound.InputValue.Add(up.VarCell, up);
            }
            root.LowerBound = lowerBound;
            root.UpperBound = upperBound;

        }

        private void RecursiveRun(List<Row> pointList, int depth,Node nodeNow)
        {
            nodeNow.PivotVariable = listVariables[depth % listVariables.Count];
            nodeNow.Depth = depth;
            // Find Median 
            List<double> listNum = new List<double>();
            for(int i = 0;i < pointList.Count;i++)
            {
                if (pointList[i].InputValue.ContainsKey(nodeNow.PivotVariable))
                {
                    listNum.Add(Convert.ToDouble(pointList[i].InputValue[nodeNow.PivotVariable].ValueCell));
                }
                else listNum.Add(0.0); 
            }
            nodeNow.PivotValue = separateMethod.Run(listNum);

            // Separate to 2 List
            List<Row> leftRow = new List<Row>();
            List<Row> rightRow = new List<Row>();
            int numMedian = 0;
            for (int i = 0; i < pointList.Count; i++)
            {
                double value = 0.0;
                if (pointList[i].InputValue.ContainsKey(nodeNow.PivotVariable))
                {
                    value = Convert.ToDouble(pointList[i].InputValue[nodeNow.PivotVariable].ValueCell);
                }
                if (Math.Abs(value - nodeNow.PivotValue) < 1e-3)
                {
                    numMedian++;
                    if (numMedian % 2 == 0) leftRow.Add(pointList[i]);
                    else rightRow.Add(pointList[i]);
                }
                else if (value > nodeNow.PivotValue)
                {
                    rightRow.Add(pointList[i]);
                }
                else
                {
                    leftRow.Add(pointList[i]);
                }
            }
            // construct child
            Node leftChild = null;
            if (leftRow.Count <= maxPointInLeaf && leftRow.Count > 0) leftChild = new Leaf(depth + 1, leftRow);
            else if(leftRow.Count > 0) leftChild = new Node(depth + 1);
            leftChild.LowerBound = nodeNow.LowerBound.Copy();
            leftChild.UpperBound = nodeNow.UpperBound.Copy();
            leftChild.UpperBound.InputValue[nodeNow.PivotVariable].ValueCell = nodeNow.PivotValue;

            Node rightChild = null;
            if (rightRow.Count <= maxPointInLeaf && rightRow.Count > 0) rightChild = new Leaf(depth + 1, leftRow);
            else if(rightRow.Count > 0) rightChild = new Node(depth + 1);
            rightChild.LowerBound = nodeNow.LowerBound.Copy();
            rightChild.UpperBound = nodeNow.UpperBound.Copy();
            rightChild.LowerBound.InputValue[nodeNow.PivotVariable].ValueCell = nodeNow.PivotValue;

            nodeNow.LeftChild = leftChild;
            nodeNow.RightChild = rightChild;
            if (leftRow.Count > maxPointInLeaf) 
                RecursiveRun(leftRow, depth + 1, leftChild);
            if (rightRow.Count > maxPointInLeaf) 
                RecursiveRun(rightRow, depth + 1, rightChild);
            return;
        }

        private void RecursiveTraceLeafBucket(Node posnow)
        {
            if (posnow is Leaf)
            {
                Leaf leafNow = posnow as Leaf;
                if(leafNow.PointInside.Count > 0)listBucketLeaf.Add(posnow as Leaf);
                return;
            }
            try
            {
                if (posnow.LeftChild != null) RecursiveTraceLeafBucket(posnow.LeftChild);
            }
            catch(Exception ex){ }
            try
            {
                if (posnow.RightChild != null) RecursiveTraceLeafBucket(posnow.RightChild);
            }
            catch (Exception ex) { }
        }

        public List<Leaf> TraceLeafBucket()
        {
            listBucketLeaf = new List<Leaf>();
            RecursiveTraceLeafBucket(root);
            return listBucketLeaf;
        }

        public void Run()
        {
            if (listRow == null || listVariables == null ||  listRow.Count <= 0 || listVariables.Count <= 0)
                return;
            if (listRow.Count <= maxPointInLeaf)
            {
                CreateRootLeaf(listRow);
                return;
            }
            CreateRoot();
            RecursiveRun(this.listRow, 0, this.root);
        }

        public void Run(List<Row> listRow, List<Variables> listVariables, int maxPointInLeaf)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.maxPointInLeaf = maxPointInLeaf;
            this.Run();
        }

        public void Run(List<Row> listRow, List<Variables> listVariables, int maxPointInLeaf, ISeparator separatorMethod)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
            this.Run();
        }

        public long RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
            return elapsedTime;
        }
        public long RunWithTime(List<Row> listRow, List<Variables> listVariables, int maxPointInLeaf)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.maxPointInLeaf = maxPointInLeaf;
            return this.RunWithTime();
        }
        public long RunWithTime(List<Row> listRow, List<Variables> listVariables, int maxPointInLeaf, ISeparator separatorMethod)
        {
            this.listRow = listRow;
            this.listVariables = listVariables;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
            return this.RunWithTime();
        }

    }
}

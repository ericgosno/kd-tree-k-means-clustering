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
        Node root;
        Dataset dataset;
        int maxPointInLeaf;

        ISeparator separateMethod;
        List<Leaf> listBucketLeaf;

        #region public_properties
        public int MaxPointInLeaf
        {
          get { return maxPointInLeaf; }
          set { maxPointInLeaf = value; }
        }

        public Dataset Dataset
        {
            get { return dataset; }
            set { dataset = value; }
        }

        public Node Root
        {
            get { return root; }
            set { root = value; }
        }
        public List<Leaf> ListBucketLeaf
        {
            get { return listBucketLeaf; }
            set { listBucketLeaf = value; }
        }
        #endregion

        public KDTree()
        {
            this.dataset = new Dataset();
            listBucketLeaf = new List<Leaf>();
            root = null;
            this.maxPointInLeaf = 1;
            // Default Separator Method
            separateMethod = new MedianBFPRTSeparator();
        }

        public KDTree(Dataset dataset,int maxPointInLeaf)
        {
            this.dataset = dataset;
            listBucketLeaf = new List<Leaf>();
            this.root = null;
            this.maxPointInLeaf = maxPointInLeaf;
            // Default Separator Method
            this.separateMethod = new MedianBFPRTSeparator();
        }

        public KDTree(Dataset dataset, int maxPointInLeaf,ISeparator separatorMethod)
        {
            this.dataset = dataset;
            listBucketLeaf = new List<Leaf>();
            this.root = null;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
        }

        private void CreateRoot()
        {
            root = new Node(0);
            Row lowerBound = new Row();
            Row upperBound = new Row();
            for (int i = 0; i < dataset.InputVariables.Count; i++)
            {
                double lower = (dataset.InputVariables[i].LimitVariables.Key < (double)int.MaxValue) ? dataset.InputVariables[i].LimitVariables.Key : 0.0;
                double upper = (dataset.InputVariables[i].LimitVariables.Value > (double)int.MinValue) ? dataset.InputVariables[i].LimitVariables.Value : 0.0;
                Cell low = new Cell(dataset.InputVariables[i],dataset.InputVariables[i].LimitVariables.Key);
                lowerBound.InputValue.Add(low.VarCell, low);
                Cell up = new Cell(dataset.InputVariables[i], dataset.InputVariables[i].LimitVariables.Value);
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
            for (int i = 0; i < dataset.InputVariables.Count; i++)
            {
                Cell low = new Cell(dataset.InputVariables[i], dataset.InputVariables[i].LimitVariables.Key);
                lowerBound.InputValue.Add(low.VarCell, low);
                Cell up = new Cell(dataset.InputVariables[i], dataset.InputVariables[i].LimitVariables.Value);
                upperBound.InputValue.Add(up.VarCell, up);
            }
            root.LowerBound = lowerBound;
            root.UpperBound = upperBound;

        }

        private void RecursiveRun(List<Row> pointList, int depth,Node nodeNow)
        {
            nodeNow.PivotVariable = dataset.InputVariables[depth % dataset.InputVariables.Count];
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
            if (leftRow.Count <= maxPointInLeaf && leftRow.Count > 0)
            {
                leftChild = new Leaf(depth + 1, leftRow);
                listBucketLeaf.Add(leftChild as Leaf);
            }
            else if (leftRow.Count > 0) leftChild = new Node(depth + 1);
            leftChild.LowerBound = nodeNow.LowerBound.Copy();
            leftChild.UpperBound = nodeNow.UpperBound.Copy();
            leftChild.UpperBound.InputValue[nodeNow.PivotVariable].ValueCell = nodeNow.PivotValue;

            Node rightChild = null;
            if (rightRow.Count <= maxPointInLeaf && rightRow.Count > 0)
            {
                rightChild = new Leaf(depth + 1, leftRow);
                listBucketLeaf.Add(rightChild as Leaf);
            }
            else if (rightRow.Count > 0) rightChild = new Node(depth + 1);
            rightChild.LowerBound = nodeNow.LowerBound.Copy();
            rightChild.UpperBound = nodeNow.UpperBound.Copy();
            rightChild.LowerBound.InputValue[nodeNow.PivotVariable].ValueCell = nodeNow.PivotValue;

            //nodeNow.LeftChild = leftChild;
            //nodeNow.RightChild = rightChild;
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
            if (dataset.ListRow == null || dataset.InputVariables == null ||  dataset.ListRow.Count <= 0 || dataset.InputVariables.Count <= 0)
                return;

            this.listBucketLeaf = new List<Leaf>();
            if (dataset.ListRow.Count <= maxPointInLeaf)
            {
                CreateRootLeaf(dataset.ListRow);
                return;
            }
            CreateRoot();
            RecursiveRun(this.dataset.ListRow, 0, this.root);
        }

        public void Run(Dataset dataset, int maxPointInLeaf)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            this.Run();
        }

        public void Run(Dataset dataset, int maxPointInLeaf, ISeparator separatorMethod)
        {
            this.dataset = dataset;
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
        public long RunWithTime(Dataset dataset, int maxPointInLeaf)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            return this.RunWithTime();
        }
        public long RunWithTime(Dataset dataset, int maxPointInLeaf, ISeparator separatorMethod)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
            return this.RunWithTime();
        }

    }
}

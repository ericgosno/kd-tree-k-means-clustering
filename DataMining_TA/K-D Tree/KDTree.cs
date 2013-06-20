// <copyright file="KDTree.cs">
// Copyright (c) 05-04-2013 All Right Reserved
// </copyright>

// This script is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.   

// The GNU General Public License can be found at 
// http://www.gnu.org/copyleft/gpl.html

// This script is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU General Public License for more details.

// <author>Eric Budiman Gosno <eric.gosno@gmail.com></author>
// <date>05-04-2013</date>
// <summary>Class representing a KDTree.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;
using System.Diagnostics;
using K_D_Tree.Separator;

namespace K_D_Tree
{
    /// <summary>
    /// K-Dimensional Tree Implementation
    /// </summary>
    public class KDTree
    {
        #region private_or_protected_properties
        private Node root;
        private Dataset dataset;
        private int maxPointInLeaf;
        private ISeparator separateMethod;
        private List<Leaf> listBucketLeaf;
        #endregion

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

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="KDTree"/> class.
        /// </summary>
        public KDTree()
        {
            this.dataset = new Dataset();
            listBucketLeaf = new List<Leaf>();
            root = null;
            this.maxPointInLeaf = 1;
            // Default Separator Method
            separateMethod = new MedianBFPRTSeparator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KDTree"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxPointInLeaf">The max point in leaf.</param>
        public KDTree(Dataset dataset,int maxPointInLeaf)
        {
            this.dataset = dataset;
            listBucketLeaf = new List<Leaf>();
            this.root = null;
            this.maxPointInLeaf = maxPointInLeaf;
            // Default Separator Method
            this.separateMethod = new MedianBFPRTSeparator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KDTree"/> class.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxPointInLeaf">The max point in leaf.</param>
        /// <param name="separatorMethod">The separator method.</param>
        public KDTree(Dataset dataset, int maxPointInLeaf,ISeparator separatorMethod)
        {
            this.dataset = dataset;
            listBucketLeaf = new List<Leaf>();
            this.root = null;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
        }
        #endregion

        #region public_function
        /// <summary>
        /// Traces the leaf bucket.
        /// </summary>
        /// <returns></returns>
        public List<Leaf> TraceLeafBucket()
        {
            listBucketLeaf = new List<Leaf>();
            RecursiveTraceLeafBucket(root);
            return listBucketLeaf;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            if (dataset.ListRow == null || dataset.InputVariables == null || dataset.ListRow.Count <= 0 || dataset.InputVariables.Count <= 0)
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

        /// <summary>
        /// Runs the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxPointInLeaf">The max point in leaf.</param>
        public void Run(Dataset dataset, int maxPointInLeaf)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            this.Run();
        }

        /// <summary>
        /// Runs the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxPointInLeaf">The max point in leaf.</param>
        /// <param name="separatorMethod">The separator method.</param>
        public void Run(Dataset dataset, int maxPointInLeaf, ISeparator separatorMethod)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
            this.Run();
        }

        /// <summary>
        /// Runs the with time.
        /// </summary>
        /// <returns></returns>
        public long RunWithTime()
        {
            var sw = Stopwatch.StartNew();
            this.Run();
            long elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
            return elapsedTime;
        }

        /// <summary>
        /// Runs the with time.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxPointInLeaf">The max point in leaf.</param>
        /// <returns></returns>
        public long RunWithTime(Dataset dataset, int maxPointInLeaf)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            return this.RunWithTime();
        }


        /// <summary>
        /// Runs the with time.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="maxPointInLeaf">The max point in leaf.</param>
        /// <param name="separatorMethod">The separator method.</param>
        /// <returns></returns>
        public long RunWithTime(Dataset dataset, int maxPointInLeaf, ISeparator separatorMethod)
        {
            this.dataset = dataset;
            this.maxPointInLeaf = maxPointInLeaf;
            this.separateMethod = separatorMethod;
            return this.RunWithTime();
        }
        #endregion

        #region private_function
        /// <summary>
        /// Creates the root.
        /// </summary>
        private void CreateRoot()
        {
            root = new Node(0);
        }

        /// <summary>
        /// Creates the root as leaf.
        /// </summary>
        /// <param name="rows">List rows.</param>
        private void CreateRootLeaf(List<Row> rows)
        {
            root = new Leaf(0, rows);
        }

        /// <summary>
        /// Recursives the run.
        /// </summary>
        /// <param name="pointList">The point list.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="nodeNow">The node now.</param>
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
                    //rightRow.Add(pointList[i]);
                    leftRow.Add(pointList[i]);
                    //if (numMedian % 2 == 0) leftRow.Add(pointList[i]);
                    //else rightRow.Add(pointList[i]);
                }
                else if (value < nodeNow.PivotValue)
                {
                    leftRow.Add(pointList[i]);
                }
                else
                {
                    rightRow.Add(pointList[i]);
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

            Node rightChild = null;
            if (rightRow.Count <= maxPointInLeaf && rightRow.Count > 0)
            {
                rightChild = new Leaf(depth + 1, leftRow);
                listBucketLeaf.Add(rightChild as Leaf);
            }
            else if (rightRow.Count > 0) rightChild = new Node(depth + 1);

            nodeNow.LeftChild = leftChild;
            nodeNow.RightChild = rightChild;
            if (leftRow.Count > maxPointInLeaf) 
                RecursiveRun(leftRow, depth + 1, leftChild);
            if (rightRow.Count > maxPointInLeaf) 
                RecursiveRun(rightRow, depth + 1, rightChild);
            return;
        }

        /// <summary>
        /// Recursives the trace leaf bucket.
        /// </summary>
        /// <param name="posnow">Node now</param>
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
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace K_D_Tree
{
    public class Node
    {
        protected Variables pivotVariable;
        protected double pivotValue;
        protected Row upperBound;
        protected Row lowerBound;
        protected Node leftChild;
        protected Node rightChild;
        protected int depth;

        #region public_properties
        public Variables PivotVariable
        {
            get { return pivotVariable; }
            set { pivotVariable = value; }
        }

        public double PivotValue
        {
            get { return pivotValue; }
            set { pivotValue = value; }
        }

        public Row UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }

        public Row LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        public Node LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }

        public Node RightChild
        {
            get { return rightChild; }
            set { rightChild = value; }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        #endregion

        public Node()
        {
            this.leftChild = null;
            this.rightChild = null;
        }

        public Node(int depth)
        {
            this.leftChild = null;
            this.rightChild = null;
            this.depth = depth;
        }
    }
}

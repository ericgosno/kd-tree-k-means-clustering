// <copyright file="Node.cs">
// Copyright (c) 05-05-2013 All Right Reserved
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
// <date>05-05-2013</date>
// <summary>Class representing a Node.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace K_D_Tree
{
    /// <summary>
    /// KD-Tree's Node Object
    /// </summary>
    public class Node
    {
        #region protected_or_private_properties
        protected Variables pivotVariable;
        protected double pivotValue;
        protected Node leftChild;
        protected Node rightChild;
        protected int depth;
        #endregion

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

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
            this.leftChild = null;
            this.rightChild = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="depth">Node depth</param>
        public Node(int depth)
        {
            this.leftChild = null;
            this.rightChild = null;
            this.depth = depth;
        }
        #endregion
    }
}

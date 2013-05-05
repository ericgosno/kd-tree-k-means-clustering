// <copyright file="CategoricalVariable.cs">
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
// <summary>Class representing a CategoricalVariable.cs entity.</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    /// <summary>
    /// CategoricalVariable Class
    /// Store information about Categorical Type Variable
    /// </summary>
    class CategoricalVariable : DiscreetVariable
    {
        #region private_or_protected_properties
        private Dictionary<object, int> paramVariables;
        #endregion

        #region public_properties
        public Dictionary<object, int> ParamVariables
        {
            get { return paramVariables; }
            set { paramVariables = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoricalVariable" /> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        public CategoricalVariable(string nameVariables)
            : base(nameVariables)
        {
            this.paramVariables = new Dictionary<object, int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoricalVariable" /> class.
        /// </summary>
        public CategoricalVariable()
            : base()
        {
            this.paramVariables = new Dictionary<object, int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoricalVariable" /> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="rowFrequency">The row frequency.</param>
        /// <param name="termFrequency">The term frequency.</param>
        public CategoricalVariable(string nameVariables, int rowFrequency, int termFrequency)
            : base(nameVariables,rowFrequency,termFrequency)
        {
            this.paramVariables = new Dictionary<object, int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoricalVariable"/> class.
        /// </summary>
        /// <param name="paramVariables">The param variables.</param>
        public CategoricalVariable(Dictionary<object, int> paramVariables)
            : base()
        {
            this.paramVariables = paramVariables;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CategoricalVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="paramVariables">The param variables.</param>
        public CategoricalVariable(string nameVariables, Dictionary<object, int> paramVariables)
            : base(nameVariables)
        {
            this.paramVariables = paramVariables;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CategoricalVariable"/> class.
        /// </summary>
        /// <param name="nameVariables">The name variables.</param>
        /// <param name="rowFrequency">The row frequency.</param>
        /// <param name="termFrequency">The term frequency.</param>
        /// <param name="paramVariables">The param variables.</param>
        public CategoricalVariable(string nameVariables, int rowFrequency, int termFrequency, Dictionary<object, int> paramVariables)
            : base(nameVariables, rowFrequency, termFrequency)
        {
            this.paramVariables = paramVariables;
        }
        #endregion
    }
}

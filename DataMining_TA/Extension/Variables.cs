﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension
{
    public class Variables
    {
        private string nameVariables;
        private bool isDiscreet;
        private int frequency;
        private KeyValuePair<double, double> limitVariables;
        private Dictionary<object, int> paramVariables;
        private Dictionary<object,KeyValuePair<double, double>> limitParamVariables;

        #region public_properties
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }
        public bool IsDiscreet
        {
            get { return isDiscreet; }
            set { isDiscreet = value; }
        }
        public KeyValuePair<double, double> LimitVariables
        {
            get { return limitVariables; }
            set { limitVariables = value; }
        }

        public Dictionary<object, KeyValuePair<double, double>> LimitParamVariables
        {
            get { return limitParamVariables; }
            set { limitParamVariables = value; }
        }

        public string NameVariables
        {
            get { return nameVariables; }
            set { nameVariables = value; }
        }

        public Dictionary<object, int> ParamVariables
        {
            get { return paramVariables; }
            set { paramVariables = value; }
        }
        #endregion

        public Variables(string nameVariables)
        {
            this.nameVariables = nameVariables;
            paramVariables = new Dictionary<object, int>();
            limitParamVariables = new Dictionary<object, KeyValuePair<double, double>>();
            limitVariables = new KeyValuePair<double, double>((double)int.MaxValue,(double)int.MinValue);
            this.frequency = 0;
        }

        public Variables()
        {
            paramVariables = new Dictionary<object, int>();
            limitParamVariables = new Dictionary<object, KeyValuePair<double, double>>();
            limitVariables = new KeyValuePair<double, double>((double)int.MaxValue, (double)int.MinValue);
            this.frequency = 0;
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    public class LogarithmTermFrequency : iTermFrequency
    {
        public double CalculateTermFrequency(int RawTermFrequency)
        {
            return 1.0 + Math.Log(Convert.ToDouble(RawTermFrequency));
        }
    }
}

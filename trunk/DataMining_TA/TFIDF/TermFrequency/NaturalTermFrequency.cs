using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    public class NaturalTermFrequency : iTermFrequency
    {
        public double CalculateTermFrequency(int RawTermFrequency)
        {
            return Convert.ToDouble(RawTermFrequency);
        }
    }
}

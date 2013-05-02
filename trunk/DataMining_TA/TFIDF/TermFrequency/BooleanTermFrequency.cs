using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    public class BooleanTermFrequency : iTermFrequency
    {
        public double CalculateTermFrequency(int RawTermFrequency)
        {
            return (RawTermFrequency > 0) ? 1.0 : 0.0;
        }
    }
}

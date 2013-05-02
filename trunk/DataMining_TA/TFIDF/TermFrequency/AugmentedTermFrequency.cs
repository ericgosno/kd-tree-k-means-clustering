using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    public class AugmentedTermFrequency : iTermFrequency
    {
        double maxRawTermFrequency;
        public AugmentedTermFrequency(int maxRawTermFrequency)
        {
            this.maxRawTermFrequency = Convert.ToDouble(maxRawTermFrequency);
        }
        public double CalculateTermFrequency(int RawTermFrequency)
        {
            return 0.5 + ((0.5 * Convert.ToDouble(RawTermFrequency)) / maxRawTermFrequency);
        }
    }
}

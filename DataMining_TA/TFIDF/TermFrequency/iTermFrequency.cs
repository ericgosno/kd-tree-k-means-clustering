using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.TermFrequency
{
    public interface iTermFrequency
    {
        double CalculateTermFrequency(int RawTermFrequency);
    }
}

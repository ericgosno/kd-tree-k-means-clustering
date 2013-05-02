using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.InverseDocumentFrequency
{
    public class NaturalInverseDocumentFrequency : iInverseDocumentFrequency
    {

        public double CalculateInverseDocumentFrequency(int numDocument, int rawDocumentFrequency)
        {
            return 1.0;
        }
    }
}

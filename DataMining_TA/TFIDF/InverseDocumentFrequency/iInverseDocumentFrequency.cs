using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFIDF.InverseDocumentFrequency
{
    public interface iInverseDocumentFrequency
    {
        double CalculateInverseDocumentFrequency(int numDocument, int rawDocumentFrequency);
    }
}

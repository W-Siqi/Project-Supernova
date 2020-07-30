using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

public abstract class ProbabilityFilter
{
    public abstract Distribution Filt();

    protected static Distribution GetAvartageDistribution(Card[] cards) { 
        var distribution = new Distribution();
        float pdfStep = 1f / (float)cards.Length;
        float pdf = pdfStep;
        foreach (var c in cards)
        {
            distribution.cardPDFs.Add(new KeyValuePair<Card, float>(c, pdf));
            pdf += pdfStep;
        }
        return distribution;
    }
}

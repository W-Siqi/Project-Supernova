using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector{
    public static Card[] Select(ProbabilityFilter probFilter, CountFilter countFilter) {
        var distribution = probFilter.Filt();
        var count = countFilter.Filt();

        var res = new List<Card>();
        for (int i = 0; i < count; i++) {
            res.Add(distribution.cards[i]);
        }
        return res.ToArray();
    }
}

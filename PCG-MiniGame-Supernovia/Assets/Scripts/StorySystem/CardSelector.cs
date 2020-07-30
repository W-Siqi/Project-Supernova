using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class CardSelector {
        public static Card[] Select(ProbabilityFilter probFilter, CountFilter countFilter) {
            var distribution = probFilter.Filt();
            var count = Mathf.Min(countFilter.Filt(), distribution.cardPDFs.Count);

            var res = new List<Card>();
            for (int i = 0; i < count; i++) {
                float rand = Random.Range(0f, distribution.maxPDF);
                var selected = distribution.Sample(rand);
                res.Add(selected);
                distribution.EraseFromDistribution(selected);
            }
            return res.ToArray();
        }
    }
}
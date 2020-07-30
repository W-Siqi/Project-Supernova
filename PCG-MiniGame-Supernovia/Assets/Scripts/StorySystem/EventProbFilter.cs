using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

public class EventProbFilter : ProbabilityFilter
{
    public override Distribution Filt() {
        var candidates = new List<EventCard>();
        foreach (var eventCard in StoryContext.instance.eventDeck) {
            if (eventCard.preconditonSet.SatisfiedByCurrentContext()) {
                candidates.Add(eventCard);
            }
        }
        return GetAvartageDistribution(candidates.ToArray());
    }
}

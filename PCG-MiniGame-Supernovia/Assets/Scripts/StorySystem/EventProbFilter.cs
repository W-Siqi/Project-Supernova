using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventProbFilter : ProbabilityFilter
{
    public override Distribution Filt() {
        return GetAvartageDistribution(StoryContext.instance.eventDeck.ToArray());
    }
}

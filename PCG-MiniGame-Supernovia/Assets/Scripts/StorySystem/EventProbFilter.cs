using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventProbFilter : ProbabilityFilter
{
    public override Distrubution Filt() {
        var distribution = new Distrubution();
        distribution.cards = StoryContext.instance.eventDeck.ToArray();
        return distribution;
    }
}

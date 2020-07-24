using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemProbFilter : ProbabilityFilter
{
    private CharacterCard hookedCharacter;
    public StratagemProbFilter(CharacterCard hookedCharacter) {
        this.hookedCharacter = hookedCharacter;
    }

    public override Distrubution Filt() {
        var distribution = new Distrubution();
        distribution.cards = StoryContext.instance.stratagemDeck.ToArray();
        return distribution;
    }
}

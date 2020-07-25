using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemProbFilter : ProbabilityFilter
{
    private CharacterCard hookedCharacter;
    public StratagemProbFilter(CharacterCard hookedCharacter) {
        this.hookedCharacter = hookedCharacter;
    }

    public override Distribution Filt() {
        return GetAvartageDistribution(StoryContext.instance.stratagemDeck.ToArray());
    }
}

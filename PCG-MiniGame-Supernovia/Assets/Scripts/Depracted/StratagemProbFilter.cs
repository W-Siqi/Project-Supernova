using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

public class StratagemProbFilter : ProbabilityFilter
{
    private CharacterCard hookedCharacter;
    public StratagemProbFilter(CharacterCard hookedCharacter) {
        this.hookedCharacter = hookedCharacter;
    }

    public override Distribution Filt() {
        var candidates = new List<StratagemCard>();
        //foreach (var stratagemCard in StoryContext.instance.stratagemDeck) {
        //    //if (stratagemCard.preconditonSet.SatisfiedByCurrentContext()) {
        //    //    candidates.Add(stratagemCard);
        //    //}
        //}
        return GetAvartageDistribution(candidates.ToArray());
    }
}

using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StratagemConsequenceSet
{
    public TraitAlteration traitAlterationWhenAccept = new TraitAlteration();
    public TraitAlteration traitAlterationWhenDecline = new TraitAlteration();
    public StatusConsequence statusConsequenceWhenAccept = new StatusConsequence();
    public void Apply(CharacterCard stratagemProvider, bool isStratagemAccepted) {
        if (isStratagemAccepted) {
            var finalDelta = new StatusVector(statusConsequenceWhenAccept.delta);
            if (stratagemProvider.HasTrait(Trait.wise)) {
                finalDelta.AmplifyValueIfPositive(PCGVariableTable.instance.wiseTraitAmplifyRate);
            }
            StoryContext.instance.statusVector += finalDelta;

            traitAlterationWhenAccept.ApplyTo(stratagemProvider);
        }
        else {
            //忠诚度必减少
            stratagemProvider.loyalty -= 1;

            traitAlterationWhenDecline.ApplyTo(stratagemProvider);
        }
    }
}

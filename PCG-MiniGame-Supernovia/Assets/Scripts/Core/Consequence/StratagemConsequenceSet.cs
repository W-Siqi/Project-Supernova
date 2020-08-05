using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StratagemConsequenceSet
{
    public List<TraitAlteration> traitAlterationsWhenAccept = new List<TraitAlteration>();
    public List<TraitAlteration> traitAlterationsWhenDecline = new List<TraitAlteration>();
    public StatusVector statusDeltaWhenAccept = new StatusVector();

    public void Apply(CharacterCard stratagemProvider, bool isStratagemAccepted) {
        if (isStratagemAccepted) {
            var finalDelta = new StatusVector(statusDeltaWhenAccept);
            if (stratagemProvider.HasTrait(Trait.wise)) {
                finalDelta.AmplifyValueIfPositive(PCGVariableTable.instance.wiseTraitAmplifyRate);
            }

            foreach (var traitAlter in traitAlterationsWhenAccept) {
                traitAlter.ApplyTo(stratagemProvider);
            }
        }
        else {
            //忠诚度必减少
            stratagemProvider.loyalty -= 1;

            foreach (var traitAlter in traitAlterationsWhenDecline) {
                traitAlter.ApplyTo(stratagemProvider);
            }
        }
    }
}

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
    //public void Apply(GameState gameStateToApply, GameConfig gameConfig, CharacterCard stratagemProvider, bool isStratagemAccepted) {
    //    if (isStratagemAccepted) {
    //        // 记次数
    //        gameStateToApply.acceptCountInCurrentRound++;

    //        var finalDelta = new StatusVector(statusConsequenceWhenAccept.delta);
    //        // Trait-Pos:奸诈
    //        if (stratagemProvider.HasTrait(Trait.tricky)) {
    //            if (Random.value < 0.5) {
    //                finalDelta = -1f * finalDelta;
    //            }
    //        }
    //        // Trait-Pos:明智
    //        if (stratagemProvider.HasTrait(Trait.wise)) {
    //            finalDelta.AmplifyValueIfPositive(gameConfig.wiseTraitAmplifyRate);
    //        }

    //        // Trait-Pos: 好战
    //        if (stratagemProvider.HasTrait(Trait.warlike)) {
    //            finalDelta.army += gameConfig.warlikeTraitArmyValueWhenAccept;
    //        }
    //        gameStateToApply.statusVector += finalDelta;

    //        traitAlterationWhenAccept.ApplyTo(stratagemProvider);

    //        // Trait-Pos:残暴
    //        if (stratagemProvider.HasTrait(Trait.cruel)) {
    //            gameStateToApply.statusVector.people -= gameConfig.cruelTraitPeopleValuePerDecision;
    //        }

    //        // Trait-Pos:傲慢
    //        if (stratagemProvider.HasTrait(Trait.arrogent)) {
    //            foreach (var character in gameStateToApply.characterDeck) {
    //                if (character != stratagemProvider && character.HasTrait(Trait.jealous)) {
    //                    character.loyalty -= 1;
    //                    break;
    //                }
    //            }
    //        }

    //        // Trait-Pos:嫉妒
    //        if (gameStateToApply.acceptCountInCurrentRound >= gameConfig.jealousTraitThreshold) {
    //            foreach (var character in gameStateToApply.characterDeck) {
    //                if (character != stratagemProvider && character.HasTrait(Trait.jealous)) {
    //                    character.loyalty -= 1;
    //                }
    //            }
    //        }
    //    }
    //    else {
    //        // Trait-Pos:宽容
    //        if (stratagemProvider.HasTrait(Trait.tolerant)) {
    //            if (Random.value > gameConfig.tolerantTraitKeepLoyaltyProbability) {
    //                stratagemProvider.loyalty -= 1;
    //            }
    //        }
    //        else {
    //            stratagemProvider.loyalty -= 1;
    //        }

    //        traitAlterationWhenDecline.ApplyTo(stratagemProvider);
    //    }
    //}
}

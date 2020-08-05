//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PCG;

//public class ConsequenceApplier
//{
//    // TBD：人物和fight先是空实现
//    public static void  ApplyAsEvent(ConsequenceSet consequenceSet,BindingInfo[] bindedInfos) {
//        if (consequenceSet.statusConsequenceEnabled) {
//            ApplyStatusConsequence(consequenceSet.statusConsequence);
//        }
//    }

//    public static void ApplyAsStratagem(ConsequenceSet consequenceSet,CharacterCard stratagemProvider) {
//        if (consequenceSet.statusConsequenceEnabled) {
//            var staConseq = consequenceSet.statusConsequence;
//            if (stratagemProvider.HasTrait(Trait.wise)) {
//                var ampliy = (1 + PCGVariableTable.instance.wiseTraitAmplifyRate);
//                if (staConseq.delta.army > 0) {
//                    staConseq.delta.army = (int) (staConseq.delta.army*ampliy);
//                }

//                if (staConseq.delta.money > 0) {
//                    staConseq.delta.money = (int)(staConseq.delta.money * ampliy);
//                }

//                if (staConseq.delta.people > 0) {
//                    staConseq.delta.people = (int)(staConseq.delta.people * ampliy);
//                }
//            }
//            ApplyStatusConsequence(staConseq);
//        }
//    }


//    private static void ApplyStatusConsequence(StatusConsequence consequence) {
//        StoryContext.instance.statusVector += consequence.delta;
//    }

//    //// 暂时废弃
//    //private static void ApplyQualifiersAlateration(List<Qualifier> target, QualiferAlteration alteration) {
//    //    var exist = target.Exists((i) => i.name == alteration.targetQualifier.name);
//    //    var enhanceProbability = 0.5f;
//    //    var weakenProbability = 0.5f;
//    //    switch (alteration.type) {
//    //        case QualiferAlteration.Type.add:
//    //            if (!exist) {
//    //                target.Add(new Qualifier(alteration.targetQualifier.name));
//    //            }
//    //            break;
//    //        case QualiferAlteration.Type.remove:
//    //            if (exist) {
//    //                target.Remove(alteration.targetQualifier);
//    //            }
//    //            break;
//    //        default:
//    //            Debug.LogError("undefined alteration type");
//    //            break;
//    //    }
//    //}

//    //private static void ApplyFightConsequence(FightConsequence fightConsequence, BindingInfo[] bindedInfos) {
//    //    if (bindedCharacters.Length < 2) {
//    //        Debug.LogError("fight consequence， 角色绑定人数错误");
//    //        return;
//    //    }
//    //    var attacker = fightConsequence.GetAttacker(bindedCharacters);
//    //    var defender = fightConsequence.GetDefender(bindedCharacters);
//    //    defender.attributes.HP -= attacker.attributes.atkVal;
//    //}

//    //private static void ApplyCharatcterConsequence(CharacterCard target, CharacterConsequence consequence) {
//    //    foreach (var qualiterAlteraion in consequence.qualiferAlterations) {
//    //        ApplyQualifiersAlateration(target.qualifiers, qualiterAlteraion);
//    //    }
//    //}
//}

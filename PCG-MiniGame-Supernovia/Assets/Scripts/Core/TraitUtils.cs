using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCG {
    public class TraitUtils {
        private static List<Trait> allValues = null;
        public static bool IsEvil(Trait trait) {
            if (trait == Trait.wise 
                || trait == Trait.silence 
                || trait == Trait.honest
                || trait == Trait.tolerant
                || trait ==  Trait.warlike) {
                return false;
            }
            return true;
        }
        public static string TranslateToName(Trait trait) {
            switch (trait) {
                case Trait.none:
                    return "?";
                case Trait.wise:
                    return "智";
                case Trait.silence:
                    return "默";
                case Trait.honest:
                    return "廉";
                case Trait.tolerant:
                    return "容";
                case Trait.warlike:
                    return "军";
                case Trait.arrogent:
                    return "傲";
                case Trait.corrupt:
                    return "贪";
                case Trait.cruel:
                    return "凶";
                case Trait.jealous:
                    return "妒";
                case Trait.tricky:
                    return "骗";
            }
            return trait.ToString();
        }

        public static string GetFullName(Trait trait) {
            switch (trait) {
                case Trait.none:
                    return "未知";
                case Trait.wise:
                    return "智慧";
                case Trait.silence:
                    return "沉默";
                case Trait.honest:
                    return "廉洁";
                case Trait.tolerant:
                    return "宽容";
                case Trait.warlike:
                    return "骁勇";
                case Trait.arrogent:
                    return "傲慢";
                case Trait.corrupt:
                    return "贪婪";
                case Trait.cruel:
                    return "残暴";
                case Trait.jealous:
                    return "嫉妒";
                case Trait.tricky:
                    return "欺骗";
            }
            return trait.ToString();
        }

        public static string GetTooltip(Trait trait) {
            switch (trait) {
                case Trait.none:
                    return "未确定的性格，没有任何效果";
                case Trait.wise:
                    return "所提的政策会放大正向buff";
                case Trait.silence:
                    return "每回合一定几率不会提意见";
                case Trait.honest:
                    return "采纳他的建议，一定几率会消除其他人物的“腐败";
                case Trait.tolerant:
                    return "不采纳他的建议，有一定几率不会降低忠诚度";
                case Trait.warlike:
                    return "采纳他的建议，会额外增加";
                case Trait.arrogent:
                    return "如果采纳他的意见，激发一个角色的嫉妒";
                case Trait.corrupt:
                    return "财政每回合减少5，定几率会消除其他人物的“廉洁”";
                case Trait.cruel:
                    return "任何采纳的建议，都会降低民心";
                case Trait.jealous:
                    return "如果一个回合采纳3次其他角色，会降低忠诚度";
                case Trait.tricky:
                    return "他的提议中，50%的后果和他的描述是相反的";
            }
            return trait.ToString();
        }

        public static Trait GetRandomTrait(bool canBeNone = false) {
            if (allValues == null) { 
                allValues = new List<Trait>();
                foreach (Trait trait in Enum.GetValues(typeof(Trait))) {
                    allValues.Add(trait);
                }
            }

            var v =  allValues[UnityEngine.Random.Range(0, allValues.Count)];

            if (!canBeNone && v == Trait.none) {
                return GetRandomTrait();
            }
            else {
                return v;
            }
        }
    }
}
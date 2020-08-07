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
                    return "诚";
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
                    return "奸";
            }
            return trait.ToString();
        }

        public static Trait GetRandomTrait() {
            if (allValues == null) { 
                allValues = new List<Trait>();
                foreach (Trait trait in Enum.GetValues(typeof(Trait))) {
                    allValues.Add(trait);
                }
            }
            return allValues[UnityEngine.Random.Range(0, allValues.Count)];
        }
    }
}
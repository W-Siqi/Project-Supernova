using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCG {
    public class TraitUtils {

        public static bool IsEvil(Trait trait) {
            if (trait == Trait.wise) {
                return false;
            }
            return true;
        }
        public static string TranslateToName(Trait trait) {
            switch (trait) {
                case Trait.arrogent:
                    return "傲";
                case Trait.corrupt:
                    return "贪";
                case Trait.cruel:
                    return "凶";
                case Trait.indulgent:
                    return "欲";
                case Trait.jealous:
                    return "妒";
                case Trait.silence:
                    return "默";
                case Trait.tricky:
                    return "奸";
                case Trait.warlike:
                    return "莽";
                case Trait.wise:
                    return "智";
            }
            return trait.ToString();
        }

        public static Trait GetRandomTrait() {
            var allValues = new List<Trait>();
            foreach (Trait trait in Enum.GetValues(typeof(Trait))) {
                allValues.Add(trait);
            }
            return allValues[UnityEngine.Random.Range(0, allValues.Count)];
        }
    }
}
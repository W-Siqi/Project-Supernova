using System.Collections;
using System.Collections.Generic;
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
                    return "傲慢";
                case Trait.corrupt:
                    return "腐败";
                case Trait.cruel:
                    return "残忍";
                case Trait.indulgent:
                    return "放纵";
                case Trait.jealous:
                    return "嫉妒";
                case Trait.silence:
                    return "沉默";
                case Trait.tricky:
                    return "奸诈";
                case Trait.warlike:
                    return "好战";
                case Trait.wise:
                    return "明智";
            }
            return trait.ToString();
        }
    }
}
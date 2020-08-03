using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class TraitUtils {

        public static bool IsEvil(Trait trait) {
            if (trait == Trait.kind || trait == Trait.tolerant || trait == Trait.hopeful) {
                return false;
            }
            return true;
        }
        public static string TranslateToName(Trait trait) {
            switch (trait) {
                case Trait.arrogent:
                    return "傲慢";
                case Trait.fury:
                    return "怒";
                case Trait.greedy:
                    return "贪";
                case Trait.hopeful:
                    return "希冀";
                case Trait.hopeless:
                    return "绝望";
                case Trait.kind:
                    return "善";
                case Trait.lazy:
                    return "懒";
                case Trait.tolerant:
                    return "宽容";
            }
            return trait.ToString();
        }
    }
}
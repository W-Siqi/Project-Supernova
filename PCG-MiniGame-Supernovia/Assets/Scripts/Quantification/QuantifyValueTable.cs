using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

namespace PCG {
    [System.Serializable]
    public class QuantifyValueTable {
        //private const string SAVE_DIR = "Assets/Resources";
        //private const string SAVE_NAME = "quantifyTable.asset";

        //public static QuantifyValueTable _instance = null;


        public TraitQuantifyValue wiseTrait;
        public TraitQuantifyValue silenceTrait;
        public TraitQuantifyValue honestTrait;
        public TraitQuantifyValue tolerantTrait;
        public TraitQuantifyValue warlikeTrait;
        public TraitQuantifyValue corruptTrait;
        public TraitQuantifyValue cruelTrait;
        public TraitQuantifyValue trickyTrait;
        public TraitQuantifyValue jealousTrait;
        public TraitQuantifyValue arrogentTrait;


        public DiscreteQuantifyValue winRound;
        public DiscreteQuantifyValue loyalty;
        public DiscreteQuantifyValue characterCount;
        public DiscreteQuantifyValue eventCountPerRound;

        public Dictionary<Trait, TraitQuantifyValue> triatQuantifyDict {
            get { 
                if(_triatQuantifyDict==null){
                    _triatQuantifyDict = new Dictionary<Trait, TraitQuantifyValue>();
                    _triatQuantifyDict[Trait.wise] = wiseTrait;
                    _triatQuantifyDict[Trait.silence] = silenceTrait;
                    _triatQuantifyDict[Trait.honest] = honestTrait;
                    _triatQuantifyDict[Trait.tolerant] = tolerantTrait;
                    _triatQuantifyDict[Trait.warlike] = warlikeTrait;
                    _triatQuantifyDict[Trait.corrupt] = corruptTrait;
                    _triatQuantifyDict[Trait.cruel] = cruelTrait;
                    _triatQuantifyDict[Trait.tricky] = trickyTrait;
                    _triatQuantifyDict[Trait.jealous] = jealousTrait;
                    _triatQuantifyDict[Trait.arrogent] = arrogentTrait;
                }
                return _triatQuantifyDict;
            }
        }

        private Dictionary<Trait, TraitQuantifyValue> _triatQuantifyDict = null;

        public Trait GetRandomTraitUsingQuanficiton(Trait trait, bool moreDifficult) {
            var curDifficulty = triatQuantifyDict[trait].difficultyFacor;

            var allValues = new List<Trait>();
            foreach (Trait t in Enum.GetValues(typeof(Trait))) {
                if (t != Trait.none) {
                    allValues.Add(t);
                }
            }
            int iter = UnityEngine.Random.Range(0,allValues.Count);
            for (int i = 0; i < allValues.Count; i++) {
                if (moreDifficult && triatQuantifyDict[allValues[iter]].difficultyFacor < curDifficulty) {
                    return allValues[iter];
                }
                else if (!moreDifficult && triatQuantifyDict[allValues[iter]].difficultyFacor > curDifficulty) {
                    return allValues[iter];
                }
                else {
                    iter = (iter + 1) % allValues.Count;
                }
            }
            return trait;
        }

        //public static QuantifyValueTable instance {
        //    get {
        //        if (_instance == null) {
        //            _instance = LoadOrCreate();
        //        }
        //        return _instance;
        //    }
        //}

        //private static QuantifyValueTable LoadOrCreate() {
        //    var savePath = string.Format("{0}/{1}",SAVE_DIR,SAVE_NAME);
        //    var archive = AssetDatabase.LoadAssetAtPath<QuantifyValueTable>(savePath);
        //    if (archive == null) {
        //        if (!Directory.Exists(SAVE_DIR)) {
        //            Directory.CreateDirectory(SAVE_DIR);
        //        }

        //        archive = ScriptableObject.CreateInstance<QuantifyValueTable>();
        //        AssetDatabase.CreateAsset(archive, savePath);
        //    }
        //    return archive;
        //}
    }
}

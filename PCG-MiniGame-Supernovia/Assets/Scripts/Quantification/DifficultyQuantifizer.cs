using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace PCG {
    [ExecuteInEditMode]
    public class DifficultyQuantifizer : MonoBehaviour {
        [System.Serializable]
        public class Config {
            public int recipeTestCount = 1000;
            public int playcountPerRecepe = 100;
            public string savePath = "Assets/quantifyTable.txt";
        }

        public Config config;
        public QuantifyValueTable quantifyValueTable;


        [ContextMenu("计算量化表")]
        private void CalculateQuantifyValueTable() {
            StartCoroutine(CalcutatingQuantifyValueTable());
        }

        [ContextMenu("保存量化表")]
        private void SaveQuantifyValueTable() {
            var serialized = JsonUtility.ToJson(quantifyValueTable);
            StreamWriter writer = new StreamWriter(config.savePath, true);
            writer.WriteLine(serialized);
            writer.Close();
        }

        [ContextMenu("加载量化表")]
        private void LoadQuantifyValueTable() {
            StreamReader reader = new StreamReader(config.savePath);
            var serialized  = reader.ReadToEnd();
            reader.Close();
            quantifyValueTable = JsonUtility.FromJson<QuantifyValueTable>(serialized);
        }

        IEnumerator CalcutatingQuantifyValueTable() {
            foreach (Trait trait in Enum.GetValues(typeof(Trait))) {
                if (trait != Trait.none) {
                    yield return StartCoroutine(QuanfifyTraitFactor(quantifyValueTable, trait));
                }
            }
        }

        IEnumerator QuanfifyTraitFactor(QuantifyValueTable quantifyValueTable, Trait trait) {
            var autoPlayMgr = FindObjectOfType<AutoPlayManager>();
            var recipe = new Recipe();
            double avgWinRateDelta = 0;
            for (int i = 0; i < config.recipeTestCount; i++) {
                // 完成一次对照试验

                // 第一次 - 给定trait
                recipe.ToDirectionalRandom(quantifyValueTable, trait);
                autoPlayMgr.Play(recipe, config.playcountPerRecepe);
                while (autoPlayMgr.isPlaying) {
                    yield return null;
                }
                float winRateWithTrait = autoPlayMgr.lastPlayStatistic.winRate;

                // 第二次 - 同样的recipe remove掉
                WipeOutTrait(recipe.gameState, trait);
                autoPlayMgr.Play(recipe, config.playcountPerRecepe);
                while (autoPlayMgr.isPlaying) {
                    yield return null;
                }
                float winRateWithoutTrait = autoPlayMgr.lastPlayStatistic.winRate;

                // 更新平均值
                double winRateDelta = winRateWithTrait - winRateWithoutTrait;
                avgWinRateDelta = (avgWinRateDelta * i + winRateDelta) / (double)(i + 1);
                quantifyValueTable.triatQuantifyDict[trait].difficultyFacor = avgWinRateDelta;
            }
        }

        private void WipeOutTrait(GameState gameState, Trait trait) {
            foreach (var character in gameState.characterDeck) {
                foreach (var p in character.personalities) {
                    if (p.trait == trait) {
                        p.trait = Trait.none;
                    }
                }
            }
        }
    }
}
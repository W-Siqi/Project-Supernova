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

        private delegate void ApplyDiscrteteValueToRecipe(Recipe recipe, int value);

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
            Debug.Log(serialized);
            quantifyValueTable = JsonUtility.FromJson<QuantifyValueTable>(serialized);
        }

        IEnumerator CalcutatingQuantifyValueTable() {
            // 量化traits
            foreach (Trait trait in Enum.GetValues(typeof(Trait))) {
                if (trait != Trait.none) {
                    yield return StartCoroutine(QuantifyTraitFactor(quantifyValueTable, trait));
                }
            }

            // 量化 win round count
            yield return StartCoroutine(QuantifyDiscreteValue(
                quantifyValueTable,
                quantifyValueTable.winRound,
                (recepe,val)=> { 
                    recepe.gameConfig.roundCount = val; 
                }));

            //  量化 loyalty 
            yield return StartCoroutine(QuantifyDiscreteValue(
            quantifyValueTable,
            quantifyValueTable.loyalty,
            (recepe, val) => {
                foreach (var character in recepe.gameState.characterDeck) {
                    character.loyalty = val;
                }
            }));

            //  量化 eventcount 
            yield return StartCoroutine(QuantifyDiscreteValue(
            quantifyValueTable,
            quantifyValueTable.eventCountPerRound,
            (recepe, val) => {
                recepe.gameConfig.eventCountPerRound = val;
            }));
        }

        IEnumerator QuantifyTraitFactor(QuantifyValueTable quantifyValueTable, Trait trait) {
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

        IEnumerator QuantifyDiscreteValue(
            QuantifyValueTable quantifyValueTable,
            DiscreteQuantifyValue valueToQuantify, 
            ApplyDiscrteteValueToRecipe applyDiscrteteValueToRecipe) {
            valueToQuantify.difficultyFactors = new List<double>();
            // from value是基准点，算为0
            valueToQuantify.difficultyFactors.Add(0);
            var recipe = new Recipe();
            var autoPlayMgr = FindObjectOfType<AutoPlayManager>();
            for (int value = valueToQuantify.from + valueToQuantify.step; value <= valueToQuantify.to; value += valueToQuantify.step) {
                valueToQuantify.difficultyFactors.Add(0);
                // 完成和value - step 的多组对照
                double avgWinRateDelta = 0;
                for (int i = 0; i < config.recipeTestCount; i++) {
                    // 完成一次对照试验
                    recipe.ToRandom(quantifyValueTable);

                    // 第一次 - 用value - step
                    applyDiscrteteValueToRecipe(recipe, value - valueToQuantify.step);
                    autoPlayMgr.Play(recipe, config.playcountPerRecepe);
                    while (autoPlayMgr.isPlaying) {
                        yield return null;
                    }
                    float winRatePreValue = autoPlayMgr.lastPlayStatistic.winRate;

                    // 第二次 - 用value
                    applyDiscrteteValueToRecipe(recipe, value);
                    autoPlayMgr.Play(recipe, config.playcountPerRecepe);
                    while (autoPlayMgr.isPlaying) {
                        yield return null;
                    }
                    float winRateCurValue = autoPlayMgr.lastPlayStatistic.winRate;


                    // 更新平均值
                    double winRateDelta = winRateCurValue - winRatePreValue;
                    avgWinRateDelta = (avgWinRateDelta * i + winRateDelta) / (double)(i + 1);
                    valueToQuantify.difficultyFactors[valueToQuantify.difficultyFactors.Count - 1] = avgWinRateDelta; 
                }
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
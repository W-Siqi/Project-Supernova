using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace PCG {
    public class RecipePCG : MonoBehaviour {
        [System.Serializable]
        public class Config {
            public float errorAllowed = 0.1f;
            public int playRoundsPerEstimate = 300;
            public int maxStep = 10;
            public Text dashboardText;
        }

        [SerializeField]
        private Config config;
        [SerializeField]
        private AutoPlayManager autoPlay;
        [SerializeField]
        private DifficultyQuantifizer quantifizer;
        [SerializeField]
        private Recipe recipe = new Recipe();
        [SerializeField]
        private List<Recipe> recipeHistory = new List<Recipe>();

        [SerializeField]
        [Range(0.3f, 0.9f)]
        private float debugDifficulty;
        

        [ContextMenu("Debug PCG")]
        private void DebugPCG (){
            StartCoroutine(GenerateRecipe(debugDifficulty));
        }

        public IEnumerator GenerateRecipe(float targetDifficulty) {
            recipeHistory.Clear();
            recipe.ToRandom(quantifizer.quantifyValueTable,targetDifficulty);
            for (int i = 0; i < config.maxStep; i++) {
                autoPlay.Play(recipe, config.playRoundsPerEstimate);
                while (autoPlay.isPlaying) {
                    yield return null;
                }

                recipe.difficulty = 1 - autoPlay.lastPlayStatistic.winRate;
                LogToDashBoard(string.Format("step {0} - [估算结果]：{1} - 误差{2}",i, recipe.difficulty * 100, (recipe.difficulty - targetDifficulty) * 100));

                recipeHistory.Add(new Recipe(recipe));

                if (Math.Abs(targetDifficulty - recipe.difficulty) < config.errorAllowed) {
                    break;
                }
                else {
                    recipe.AdjustDifficulty(quantifizer.quantifyValueTable, targetDifficulty);
                }
            }

            Recipe bestRecipe = recipe;
            float minDiff = Mathf.Abs(bestRecipe.difficulty - targetDifficulty);
            int bestStep = recipeHistory.Count;
            for(int i = 0; i < recipeHistory.Count; i++) {
                var r = recipeHistory[i];
                if (Mathf.Abs(r.difficulty - targetDifficulty) < minDiff) {
                    bestRecipe = r;
                    minDiff = Mathf.Abs(bestRecipe.difficulty - targetDifficulty);
                    bestStep = i;
                }
            }

            LogToDashBoard("================================== ");
            LogToDashBoard("[最优step]： " + bestStep);
            LogToDashBoard("[最终结果]： " + bestRecipe.difficulty*100);
            recipe = new Recipe(bestRecipe);
        }

        public Recipe GetCurrentRecipeCopy() {
            return new Recipe(recipe);
        }

        private List<string> dashBoardInfo = new List<string>();

        private void LogToDashBoard(string log) {
            dashBoardInfo.Add(log);
            if (dashBoardInfo.Count > 12) {
                dashBoardInfo.RemoveAt(0);
            }
            var wholeLog = "";
            foreach (var info in dashBoardInfo) {
                wholeLog += info + "\n";
            }
            if (config.dashboardText != null) {
                config.dashboardText.text = wholeLog;
            }
        }
    }
}
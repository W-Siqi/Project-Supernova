using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class RecipePCG : MonoBehaviour {
        [System.Serializable]
        public class Config {
            public float errorAllowed = 0.1f;
            public int playRoundsPerEstimate = 300;
            public int maxStep = 10;
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
            recipe.ToRandom(quantifizer.quantifyValueTable);
            for (int i = 0; i < config.maxStep; i++) {
                autoPlay.Play(recipe, config.playRoundsPerEstimate);
                while (autoPlay.isPlaying) {
                    yield return null;
                }

                recipe.difficulty = autoPlay.lastPlayStatistic.winRate;
                Debug.Log("[PCG] - 估算结果： "+recipe.difficulty);

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
            foreach (var r in recipeHistory) {
                if (Mathf.Abs(r.difficulty - targetDifficulty) < minDiff) {
                    bestRecipe = r;
                    minDiff = Mathf.Abs(bestRecipe.difficulty - targetDifficulty);
                }
            }

            Debug.Log("[PCG] - 最终结果： " + bestRecipe.difficulty);
            recipe = new Recipe(bestRecipe);
        }

        public Recipe GetCurrentRecipeCopy() {
            return new Recipe(recipe);
        }
    }
}
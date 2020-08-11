using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class RecipePCG : MonoBehaviour {
        [SerializeField]
        public class Config {
            public float errorAllowed = 0.1f;
            public int maxStep = 10;
        }

        [SerializeField]
        Config config;
        [SerializeField]
        private AutoPlayManager autoPlay;
        [SerializeField]
        private DifficultyQuantifizer quantifizer;
        [SerializeField]
        private Recipe recipe = new Recipe();
        [SerializeField]
        private List<Recipe> recipeHistory = new List<Recipe>();

        public IEnumerator GenerateRecipe(float targetDifficulty) {
            recipe.ToRandom(quantifizer.quantifyValueTable);
            for (int i = 0; i < config.maxStep; i++) {
                autoPlay.Play(recipe, 1000);
                while (autoPlay.isPlaying) {
                    yield return null;
                }
                recipe.difficulty = autoPlay.lastPlayStatistic.winRate;

                if (Math.Abs(targetDifficulty - recipe.difficulty) < config.maxStep) {
                    break;
                }
                else {
                    recipeHistory.Add(new Recipe(recipe));
                    recipe.AdjustDifficulty(quantifizer.quantifyValueTable, targetDifficulty);
                }
            }
        }

        public Recipe GetCurrentRecipeCopy() {
            return new Recipe(recipe);
        }
    }
}
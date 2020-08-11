using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    // 保存真正用来游玩的gamestate 和gameconfig
    public class PlayData : MonoBehaviour {
        static PlayData _instance = null;
        public static PlayData instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<PlayData>();
                }
                return _instance;
            }
        }


        public GameState gameState = new GameState();
        public GameConfig gameConfig = new GameConfig();

        /// <summary>
        /// 生成残局
        /// </summary>
        public IEnumerator InitData(float difficulty) {
            var recipePCG = FindObjectOfType<RecipePCG>();
            yield return StartCoroutine(recipePCG.GenerateRecipe(difficulty));
            var recipe = recipePCG.GetCurrentRecipeCopy();
            gameState = recipe.gameState;
            gameConfig = recipe.gameConfig;
        }

        public void InitDataWithoutPCG(float difficulty) {
            var recipe = new Recipe();
            recipe.ToRandom(FindObjectOfType<DifficultyQuantifizer>().quantifyValueTable,difficulty);
            gameState = recipe.gameState;
            gameConfig = recipe.gameConfig;
        }
    }
}


using PCG;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace PCG {
    [ExecuteInEditMode]
    public class AutoPlayManager : MonoBehaviour {
        private string LOG_SAVE_PATH = "Assets/GamelogPackage.asset";

        public int batchPerFrame = 0;
        public MultiAutoPlayStatistic lastPlayStatistic;

        [Range(0, 1)]
        public float debugDifficulty = 0.5f;
        public int debugPlayTimes = 10000;
        public Recipe debugRecipe = new Recipe();

        public bool recordLog;
        public bool useIntelligentAutoPlayer = true;

        public bool isPlaying { get; private set; }

        private GameLogPackage gameLogPackage = null;

        [ContextMenu("刷新Recipe")]
        public void RefreshDebugRecipe() {
            var quantifyValueTable = FindObjectOfType<DifficultyQuantifizer>().quantifyValueTable;
            debugRecipe.ToRandom(quantifyValueTable);
        }


        [ContextMenu("Debug Play")]
        public void DebugPlay() {
            Play(debugRecipe, debugPlayTimes, recordLog);
        }

        public void Play(Recipe recipe, int playTimes, bool recordLog = false) {
            if (isPlaying) {
                UnityEngine.Debug.LogError("不能重复play");
                return;
            }
            if (recordLog && playTimes > 200) {
                UnityEngine.Debug.LogError("记录日志不能超过数量");
                return;
            }
            lastPlayStatistic = new MultiAutoPlayStatistic();
            StartCoroutine(PlayingCoroutine(recipe.gameState, recipe.gameConfig, playTimes, recordLog));
        }

        // 模拟的总协程
        IEnumerator PlayingCoroutine(GameState gameState, GameConfig gameConfig, int playTimes, bool recordLog) {
            if (recordLog) {
                gameLogPackage = ScriptableObject.CreateInstance<GameLogPackage>();
                gameLogPackage.gameState = gameState;
            }
            isPlaying = true;

            Profiler.BeginSample("autoplay");
            var startTime = Time.time;

            AutoPlayer autoPlayer;
            if (useIntelligentAutoPlayer) {
                autoPlayer = new IntelligentAutoPlayer();
            }
            else {
                autoPlayer = new MonteCarloAutoPlayer();
            }

            var initStateForRunning = gameState.MakeDeepCopy();
            int playCount = 0;
            while (playCount < playTimes) {
                int batchCount = Mathf.Min(batchPerFrame, playTimes - playCount);
                for (int i = 0; i < batchCount; i++) {
                    // 让auto player去play
                    var playRes = autoPlayer.Play(initStateForRunning, gameConfig.MakeDeepCopy(), recordLog);
                    // 还原gameState状态
                    initStateForRunning.RecoverTo(gameState);
                    // 合并结果
                    lastPlayStatistic.Merge(playRes);
                    // 记录日志
                    if (recordLog) {
                        gameLogPackage.playStatistics.Add(playRes);
                    }
                }
                playCount += batchCount;
                yield return null;
            }

            Profiler.EndSample();
            // UnityEngine.Debug.Log("时间： " + (Time.time - startTime).ToString());
            if (recordLog) {
                gameLogPackage.summerayStatistic = lastPlayStatistic;
                AssetDatabase.CreateAsset(gameLogPackage,LOG_SAVE_PATH);
            }
            isPlaying = false;
        }
    }
}
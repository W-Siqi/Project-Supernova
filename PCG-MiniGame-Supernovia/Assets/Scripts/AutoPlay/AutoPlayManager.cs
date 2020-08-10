using PCG;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

public class AutoPlayManager : MonoBehaviour
{
    public int batchPerFrame = 0;
    public GameState lastPlayedGameInitState;
    public GameConfig lastPlayedGameConfig;
    public MultiAutoPlayStatistic lastPlayStatistic;
    public List<GameLog> lastGameLogs = new List<GameLog>();

    [Range(0,1)]
    public float debugDifficulty = 0.5f;
    public int debugPlayTimes = 10000;
    public bool recordLog;
    public bool useIntelligentAutoPlayer = true;


    [ContextMenu("刷新残局debug")]
    public void RefreshAndDebugPlay() {
        PlayData.instance.InitData(debugDifficulty);
        Play(PlayData.instance.gameState.MakeDeepCopy(), PlayData.instance.gameConfig.MakeDeepCopy(), debugPlayTimes,recordLog);
    }
         
    public void Play(GameState gameState, GameConfig gameConfig, int playTimes, bool recordLog = false) {
        if (recordLog && playTimes > 200) {
            UnityEngine.Debug.LogError("记录日志不能超过数量");
            return;
        }
        lastGameLogs.Clear();
        lastPlayStatistic = new MultiAutoPlayStatistic();
        lastPlayedGameInitState = gameState.MakeDeepCopy();
        lastPlayedGameConfig = gameConfig.MakeDeepCopy();
        StartCoroutine(PlayingCoroutine(gameState, gameConfig, playTimes, recordLog));
    }

    // 模拟的总协程
    IEnumerator PlayingCoroutine(GameState gameState, GameConfig gameConfig, int playTimes, bool recordLog) {
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
                var playRes = autoPlayer.Play(initStateForRunning, gameConfig.MakeDeepCopy(),recordLog);
                // 还原gameState状态
                initStateForRunning.RecoverTo(gameState);
                // 合并结果
                lastPlayStatistic.Merge(playRes);
                // 记录日志
                if (recordLog) {
                    lastGameLogs.Add(playRes.gameLog);
                }
            }
            playCount += batchCount;
            yield return null;
        }

        Profiler.EndSample();
        UnityEngine.Debug.Log("时间： " + (Time.time - startTime).ToString());
    }
}

using PCG;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

public class AutoPlayManager : MonoBehaviour
{
    public int batchPerFrame = 0;
    public MultiAutoPlayStatistic lastPlayStatistic;
    public int debugPlayTimes = 10000;
    public bool useIntelligentAutoPlayer = true;


    [ContextMenu("刷新残局debug")]
    public void RefreshAndDebugPlay() {
        PlayData.instance.InitData();
        ViewManager.instance.InitForGameStart();
        Play(PlayData.instance.gameState.MakeDeepCopy(), PlayData.instance.gameConfig.MakeDeepCopy(), debugPlayTimes);
    }

    [ContextMenu("按当前残局debug")]
    public void DebugPlay() {
        Play(PlayData.instance.gameState.MakeDeepCopy(), PlayData.instance.gameConfig.MakeDeepCopy(), debugPlayTimes);
    }
         
    public void Play(GameState gameState, GameConfig gameConfig, int playTimes) {
        lastPlayStatistic = new MultiAutoPlayStatistic();
        StartCoroutine(PlayingCoroutine(gameState, gameConfig, playTimes));
    }

    IEnumerator PlayingCoroutine(GameState gameState, GameConfig gameConfig, int playTimes) {
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
                var playRes = autoPlayer.Play(initStateForRunning, gameConfig.MakeDeepCopy());
                initStateForRunning.RecoverTo(gameState);
                lastPlayStatistic.Merge(playRes);
            }
            playCount += batchCount;
            yield return null;
        }

        Profiler.EndSample();
        UnityEngine.Debug.Log("时间： " + (Time.time - startTime).ToString());
    }
}

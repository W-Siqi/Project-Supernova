using PCG;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AutoPlayManager : MonoBehaviour
{
    public int batchPerFrame = 0;
    public MultiAutoPlayStatistic lastPlayStatistic;
    public int debugPlayTimes = 10000;
    
    [ContextMenu("按当前残局debug")]
    public void DebugPlay() {
        Play(PlayData.instance.gameState.MakeDeepCopy(), PlayData.instance.gameConfig.MakeDeepCopy(), debugPlayTimes);
    }
         
    public void Play(GameState gameState, GameConfig gameConfig, int playTimes) {
        lastPlayStatistic = new MultiAutoPlayStatistic();
        StartCoroutine(PlayingCoroutine(gameState, gameConfig, playTimes));
    }

    IEnumerator PlayingCoroutine(GameState gameState, GameConfig gameConfig, int playTimes) {
        var startTime = Time.time;

        int playCount = 0;
        var autoPlayer = new MonteCarloAutoPlayer();
        while (playCount < playTimes) {
            int batchCount = Mathf.Min(batchPerFrame, playTimes - playCount);
            for (int i = 0; i < batchCount; i++) {
                var playRes = autoPlayer.Play(gameState.MakeDeepCopy(),gameConfig.MakeDeepCopy());
                lastPlayStatistic.Merge(playRes);
            }
            playCount += batchCount;
            yield return null;
        }

        UnityEngine.Debug.Log("时间： " + (Time.time - startTime).ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameLogPlayer : MonoBehaviour {
        public AutoPlayManager autoPlayManager;
        public int debugPlayIndex = 0;
        [ContextMenu("读取AutoPlayer当前数据播放")]
        private void DebugPlay() {
            Play(autoPlayManager.lastPlayedGameInitState, autoPlayManager.lastGameLogs[debugPlayIndex]);
        }

        bool isPlaying = false;
        public void Play(GameState initState, GameLog gameLog) {
            if (!isPlaying) {
                StartCoroutine(PlayGamelog(initState.MakeDeepCopy(), gameLog));
            }
            else {
                Debug.LogError("不能反复播放！");
            }
        }

        private IEnumerator PlayGamelog(GameState gameState, GameLog gameLog) {
            isPlaying = true;
            ViewManager.instance.InitForGameStart(gameState);
            var modifyEventPlayer = ViewManager.instance.gameStateModifyEventPlayer;
            foreach (var logInfo in gameLog.logInfos) {
                if (logInfo.isStageSwitchEvent) {
                    // 切 stage
                    if (logInfo.switchToCountil) {
                        // init from event stage
                        ViewManager.instance.characterStausPannel.ForceSync();
                        ViewManager.instance.statusVectorPannel.ForceSync(PlayData.instance.gameState.statusVector);

                        // new page
                        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneTex);
                        StoryBook.instance.ViewContent(newPageContent);
                        yield return new WaitForSeconds(1.5f);
                        ViewManager.instance.InitViewForCouncialStage();
                    }
                    else {
                        // init from council
                        ViewManager.instance.characterStausPannel.Hide();

                        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
                        StoryBook.instance.ViewContent(newPageContent);
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                else if (logInfo.isModifyEvent) {
                    // 播放事件
                    yield return StartCoroutine(modifyEventPlayer.PlayEvent(gameState, logInfo.modifyEvent));
                }
                else { 
                    // 播放选择对话

                }
            }
            isPlaying = false;
        }
    }
}

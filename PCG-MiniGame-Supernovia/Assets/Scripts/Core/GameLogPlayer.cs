using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameLogPlayer : MonoBehaviour {
        public AutoPlayManager autoPlayManager;
        public int debugPlayIndex = 0;

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
                if (logInfo.type == GameLog.LogInfo.Type.SwitchStage) {
                    // 切 stage
                    if (logInfo.switchToCountil) {
                        // init from event stage
                        ViewManager.instance.characterStausPannel.ForceSync();
                        ViewManager.instance.statusVectorPannel.ForceSync(PlayData.instance.gameState.statusVector);

                        // new page
                        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneTex);
                        StoryBook.instance.ViewContent(newPageContent);
                        yield return new WaitForSeconds(1.5f);
                        ViewManager.instance.InitViewForCouncialStage(999,999);
                    }
                    else {
                        // init from council
                        ViewManager.instance.characterStausPannel.Hide();
                        ViewManager.instance.gameDashboard.UpdateState(999, 999, false);
                        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
                        StoryBook.instance.ViewContent(newPageContent);
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                else if (logInfo.type == GameLog.LogInfo.Type.ModifyEvnet) {
                    // 播放事件
                    yield return StartCoroutine(modifyEventPlayer.PlayEvent(gameState, logInfo.modifyEvent));
                }
                else if (logInfo.type == GameLog.LogInfo.Type.StratagemDecide) {
                    // 播放选择对话
                    var straCard = gameState.stratagemDeck[logInfo.stratagemCardIndex];
                    var character = gameState.characterDeck[logInfo.providerCharacterIndex];
                    ViewManager.instance.ViewCharacterOfDialog(character);
                    ViewManager.instance.characterStausPannel.OnSelect(character);
                    ViewManager.instance.ViewDialog(straCard, character);
                    yield return new WaitForSeconds(2f);
                    ViewManager.instance.EndViewDialog();
                    ViewManager.instance.EndViewCharacterOfDialog();
                }
                else if (logInfo.type == GameLog.LogInfo.Type.Ending) {
                    PlayEnding(logInfo);
                }
            }
            isPlaying = false;
        }


        private void PlayEnding(GameLog.LogInfo logInfo) {
            var status = logInfo.statusVectorOfEnding;
            var endingMgr = FindObjectOfType<StoryEndingManager>();
            StoryBook.PageContent pageContentOfEnd = null;
            bool win = false;


            if (logInfo.minLoyaltyOfEnding <= 0) {
                pageContentOfEnd = endingMgr.badEndLoyalty;
            }
            else if (status.army <= 0) {
                pageContentOfEnd = endingMgr.badEndArmy;
            }
            else if (status.money <= 0) {
                pageContentOfEnd = endingMgr.badEndMoney;
            }
            else if (status.people <= 0) {
                pageContentOfEnd = endingMgr.badEndPeople;
            }
            else {
                win = true;
                pageContentOfEnd = endingMgr.goodEnd;
            }

            // fill content
            if (win) {
                endingMgr.endingTitle.text = "成功结局";
            }
            else {
                endingMgr.endingTitle.text = "失败结局";
            }

            endingMgr.endingContent.text = pageContentOfEnd.text;
            endingMgr.endingImage.texture = pageContentOfEnd.image;

            ViewManager.instance.OnStortEnd();
        }    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameManager : MonoBehaviour {
        public bool usePCG;
        public StartMenuManager startMenuManager;
        public PlayLoopManager playLoopManager;
        public StoryEndingManager storyEndingManager;
        
        private void Awake() {
            StartCoroutine(PlayGame());
        }

        // [ContextMenu("start game")]
        public IEnumerator PlayGame() {
            yield return StartCoroutine(startMenuManager.WaitStartGame());

            yield return StartCoroutine(InitGameProcedule());

            // 开始游戏主循环
            yield return StartCoroutine(playLoopManager.PlayLoop());

            // ending
            storyEndingManager.OnStoryEnd(PlayData.instance.gameState);
        }

        IEnumerator InitGameProcedule() {
            // 初始化故事状态
            int seed = Random.Range(-10000, 100000);
            if (usePCG) {
                startMenuManager.PCGDashboard.SetActive(true);
                startMenuManager.UserInterface.SetActive(false);
                yield return StartCoroutine(PlayData.instance.InitData(startMenuManager.selectedDifficulty));
                yield return new WaitForSeconds(2f);
            }
            else {
                PlayData.instance.InitDataWithoutPCG(startMenuManager.selectedDifficulty);
            }

            ViewManager.instance.InitForGameStart(PlayData.instance.gameState);
        }
    }
}

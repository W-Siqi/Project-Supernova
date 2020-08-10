using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace PCG {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private StartMenuManager startMenuManager;
        [SerializeField]
        private PlayLoopManager playLoopManager;
        [SerializeField]
        private StoryEndingManager storyEndingManager;
        
        private void Awake() {
            StartCoroutine(PlayGame());
        }

        // [ContextMenu("start game")]
        public IEnumerator PlayGame() {
            yield return StartCoroutine(startMenuManager.WaitStartGame());

            InitGameProcedule();

            // 开始游戏主循环
            yield return StartCoroutine(playLoopManager.PlayLoop());

            // ending
            storyEndingManager.OnStoryEnd();
        }

        void InitGameProcedule() {
            // 初始化故事状态
            int seed = Random.Range(-10000, 100000);
            PlayData.instance.InitData(startMenuManager.selectedDifficulty);
            ViewManager.instance.InitForGameStart(PlayData.instance.gameState);
        }
    }
}

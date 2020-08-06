using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace PCG {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private PlayLoopManager playLoopManager;
        [SerializeField]
        private StoryEndingManager storyEndingManager;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(PlayGame());
        }

        // [ContextMenu("start game")]
        public IEnumerator PlayGame() {
            InitGameProcedule();

            // 开始游戏主循环
            yield return StartCoroutine(playLoopManager.PlayLoop());

            // ending
            yield return StartCoroutine(storyEndingManager.PlayStoryEnding());
        }

        void InitGameProcedule() {
            // 初始化故事状态
            int seed = Random.Range(-10000, 100000);
            PlayData.instance.InitData();
            ViewManager.instance.Init();
        }
    }
}

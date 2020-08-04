using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace PCG {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private PlayLoopManager playLoopManager;
        [SerializeField]
        private GuideManager guideManager;
        [SerializeField]
        private bool skipGuide;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            StartGame();
        }

        // [ContextMenu("start game")]
        public void StartGame() {
            StartCoroutine(StartGameProcedule());
        }

        IEnumerator StartGameProcedule() {
            // 初始化故事状态
            int seed = Random.Range(-10000, 100000);
            StoryContext.instance.InitForNewStory(seed);

            ViewManager.instance.Init();

            // 跑教程
            if (!skipGuide) {
                yield return StartCoroutine(guideManager.RunGuidence());
            }
            // 开始游戏主循环
            playLoopManager.StartPlayLoop();
        }
    }
}

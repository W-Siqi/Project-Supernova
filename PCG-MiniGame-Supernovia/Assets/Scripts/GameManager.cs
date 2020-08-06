using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace PCG {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private GuideManager guideManager;
        [SerializeField]
        private PlayLoopManager playLoopManager;
        [SerializeField]
        private StoryEndingManager storyEndingManager;

        [SerializeField]
        private bool skipGuide;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(PlayGame());
        }

        // [ContextMenu("start game")]
        public IEnumerator PlayGame() {
            yield return StartCoroutine(StartGameProcedule());

            // 开始游戏主循环
            yield return StartCoroutine(playLoopManager.PlayLoop());

            // ending
            yield return StartCoroutine(storyEndingManager.PlayStoryEnding());
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
        }
    }
}

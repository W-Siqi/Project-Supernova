using PCGP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameLog {
        [System.Serializable]
        public class LogInfo{
            public int stratagemCardIndex = -1;
            public bool acceptStratagem;
            public GameStateModifyEvent modifyEvent = null;
            public bool switchToCountil = false;
            public bool switchToEventStream = false;

            /// <summary>
            /// 第一判断优先级
            /// </summary>
            public bool isStageSwitchEvent {
                get { return switchToCountil || switchToEventStream; }
            }

            /// <summary>
            /// 第二判断优先级
            /// </summary>
            public bool isModifyEvent {
                get { return modifyEvent != null; }
            }
        }

        public List<LogInfo> logInfos = new List<LogInfo>();

        public void AddLog(GameStateModifyEvent gameStateModifyEvent) {
            var info = new LogInfo();
            info.modifyEvent = gameStateModifyEvent;
            logInfos.Add(info);
        }

        public void AddLogs(GameStateModifyEvent[] gameStateModifyEvents) {
            foreach (var e in gameStateModifyEvents) {
                AddLog(e);
            }
        }

        public void AddLog(int stratagemCardIndex,bool accpect) {
            var info = new LogInfo();
            info.stratagemCardIndex = stratagemCardIndex;
            info.acceptStratagem = accpect;
            logInfos.Add(info);
        }

        public void StageSwitchLog(bool isToCountil) {
            var info = new LogInfo();
            if (isToCountil) {
                info.switchToCountil = true;
            }
            else {
                info.switchToEventStream = true;
            }
            logInfos.Add(info);
        }
    }
}
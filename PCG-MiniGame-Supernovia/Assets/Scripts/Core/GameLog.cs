using PCGP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameLog {
        [System.Serializable]
        public class LogInfo{
            public enum Type { 
                none,SwitchStage,ModifyEvnet,StratagemDecide,Ending
            }

            public Type type = Type.none;

            public int stratagemCardIndex = -1;
            public bool acceptStratagem;
            public int providerCharacterIndex = -1;

            public GameStateModifyEvent modifyEvent = null;

            public bool switchToCountil = false;
            public bool switchToEventStream = false;

            public StatusVector statusVectorOfEnding;
            public int minLoyaltyOfEnding;
        }

        public List<LogInfo> logInfos = new List<LogInfo>();

        public void AddLog(GameStateModifyEvent gameStateModifyEvent) {
            var info = new LogInfo();
            info.type = LogInfo.Type.ModifyEvnet;
            info.modifyEvent = gameStateModifyEvent;
            logInfos.Add(info);
        }

        public void AddLogs(GameStateModifyEvent[] gameStateModifyEvents) {
            foreach (var e in gameStateModifyEvents) {
                AddLog(e);
            }
        }

        public void AddLog(int stratagemCardIndex, int providerIndex,bool accpect) {
            var info = new LogInfo();
            info.type = LogInfo.Type.StratagemDecide;
            info.stratagemCardIndex = stratagemCardIndex;
            info.acceptStratagem = accpect;
            info.providerCharacterIndex = providerIndex;
            logInfos.Add(info);
        }

        public void AddStageSwitchLog(bool isToCountil) {
            var info = new LogInfo();
            info.type = LogInfo.Type.SwitchStage;
            if (isToCountil) {
                info.switchToCountil = true;
            }
            else {
                info.switchToEventStream = true;
            }
            logInfos.Add(info);
        }

        public void LogEnding(GameState gameStateOfEnding) {
            var info = new LogInfo();
            info.type = LogInfo.Type.Ending;
            info.statusVectorOfEnding = new StatusVector(gameStateOfEnding.statusVector);
            info.minLoyaltyOfEnding = 1;
            foreach (var chara in gameStateOfEnding.characterDeck) {
                info.minLoyaltyOfEnding = Mathf.Min(info.minLoyaltyOfEnding,chara.loyalty);
            }
            logInfos.Add(info);
        }
    }
}
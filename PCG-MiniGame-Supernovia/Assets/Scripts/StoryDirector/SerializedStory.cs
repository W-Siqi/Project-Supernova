using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using PCG;

// 完整记录一个story的发生经过
[System.Serializable]
public class SerializedStory : ScriptableObject
{
    [System.Serializable]
    public class Section {
        public List<CouncilStageInfo> councilStageInfos = new List<CouncilStageInfo>();
        public List<EventCard> eventCards = new List<EventCard>();
        // 这个是和eventcard一 一对应的
        public List<ShowInfo> showInfos = new List<ShowInfo>();
    }

    [System.Serializable]
    public class CouncilStageInfo {
        public CharacterCard characterCard = new CharacterCard();
        public List<StratagemCard> stratagemCards = new List<StratagemCard>();
    }

    private const string DEFAULT_SAVE_DIR = "Assets/Resources";
    private const string DEFAULT_SAVE_NAME = "SerializedStory.asset";

    public List<Section> sections = new List<Section>();


    public static SerializedStory CreateAndSave() {
        var savePath = string.Format("{0}/{1}", DEFAULT_SAVE_DIR, DEFAULT_SAVE_NAME);

        if (!Directory.Exists(DEFAULT_SAVE_DIR)) {
            Directory.CreateDirectory(DEFAULT_SAVE_DIR);
        }

        var serializedStory = ScriptableObject.CreateInstance<SerializedStory>();
        AssetDatabase.CreateAsset(serializedStory, savePath);
        return serializedStory;
    }
}

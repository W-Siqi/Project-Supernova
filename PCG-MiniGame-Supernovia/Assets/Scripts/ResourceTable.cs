using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTable : MonoBehaviour
{
    private static ResourceTable _instance = null;
    public static ResourceTable instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ResourceTable>();
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class SceneReferencePage {
        public Canvas swipeCanvas;
    }


    [System.Serializable]
    public class Texturepage {
        public Texture aynominousCharacter;
        public RenderTexture fightSceneRT;
        public RenderTexture councilSceneRT;
        public RenderTexture eventSceneRT;
        public Sprite evilTraitBG;
        public Sprite noneEvilTraitBG;
    }


    [System.Serializable]
    public class PrefabPage {
        public GameObject cardDisplay;
        public GameObject campfireStageCard;
        public GameObject fightStageCard;
        public GameObject voteStageCard;
        public GameObject decisionInteraction;
        public GameObject anchorPoint;
        public GameObject hitEffect;
        public GameObject attackEffect;
        public GameObject characterUIRawImage;
    }

    public SceneReferencePage sceneReferencePage = new SceneReferencePage();
    public PrefabPage prefabPage = new PrefabPage();
    public Texturepage texturepage = new Texturepage();
}

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
    public class TexturePage { 
    
    }

    [System.Serializable]
    public class PrefabPage {
        public GameObject cardDisplay;
        public GameObject startagemInteraction;
        public GameObject anchorPoint;
    }

    public TexturePage texturePage = new TexturePage();
    public PrefabPage prefabPage = new PrefabPage();
}

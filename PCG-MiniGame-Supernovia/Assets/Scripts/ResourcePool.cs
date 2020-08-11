using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PCG {
    public class ResourcePool : MonoBehaviour {
        public static ResourcePool _instance = null;
        public static ResourcePool instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<ResourcePool>();
                }
                return _instance;
            }
        }

        [SerializeField]
        private string avatarImgImportDir;
        public List<Texture2D> avatarTextures;
        public List<Sprite> avatarSprites;
        public List<string> namePool = new List<string>();
        public List<string> tittlePool = new List<string>();
        public List<EventCard> eventCards = new List<EventCard>();
        public List<StratagemCard> stratagemCards = new List<StratagemCard>();

        public string GetRandomName() {
            var name = namePool[Random.Range(0, namePool.Count)];
            var tittle = tittlePool[Random.Range(0,tittlePool.Count)];
            return name + tittle;
        }

# if UNITY_EDITOR
        [ContextMenu("导入存档")]
        private void ImportFromArchive() {
            stratagemCards.Clear();
            foreach (var stragemCard in DeckArchive.instance.stratagemCards) {
                stratagemCards.Add(Card.DeepCopy(stragemCard));
            }

            // copy event pool
            eventCards.Clear();
            foreach (var card in DeckArchive.instance.eventCards) {
                eventCards.Add(Card.DeepCopy(card));
            }
        }

        [ContextMenu("导入人物图像")]
        private void ImportAvatarImage() {
            foreach (var tex in Resources.LoadAll<Texture2D>(avatarImgImportDir)) {
                avatarTextures.Add(tex);
            }

            foreach (var sp in Resources.LoadAll<Sprite>(avatarImgImportDir)) {
                avatarSprites.Add(sp);
            }
            //DirectoryInfo d = new DirectoryInfo(avatarImgImportDir);//Assuming Test is your Folder
            //foreach (FileInfo file in d.GetFiles("*.jpg")) {
            //    Debug.Log(avatarImgImportDir + "/" + file.Name);
            //    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImgImportDir + "/" + file.Name);
            //    avatarTextures.Add(texture);
            //    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(avatarImgImportDir + "/" + file.Name);
            //    avatarSprites.Add(sprite);
            //}
            //foreach (FileInfo file in d.GetFiles("*.JPG")) {
            //    Debug.Log(avatarImgImportDir + "/" + file.Name);
            //    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImgImportDir + "/" + file.Name);
            //    avatarTextures.Add(texture);
            //    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(avatarImgImportDir + "/" + file.Name);
            //    avatarSprites.Add(sprite);
            //}

            //foreach (FileInfo file in d.GetFiles("*.png")) {
            //    Debug.Log(avatarImgImportDir + "/" + file.Name);
            //    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImgImportDir + "/" + file.Name);
            //    avatarTextures.Add(texture);
            //    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(avatarImgImportDir + "/" + file.Name);
            //    avatarSprites.Add(sprite);
            //}
            //foreach (FileInfo file in d.GetFiles("*.PNG")) {
            //    Debug.Log(avatarImgImportDir + "/" + file.Name);
            //    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImgImportDir + "/" + file.Name);
            //    avatarTextures.Add(texture);
            //    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(avatarImgImportDir + "/" + file.Name);
            //    avatarSprites.Add(sprite);
            //}
        }
#endif
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCG {
    [System.Serializable]
    public abstract class Card {
        private static Texture2D defaultImage = null;

        public string name = "name";
        public string description = "";

        [SerializeField]
        private string avatarImagePath = "";
        private Texture2D loadedAvatarImage = null;

        public static T DeepCopy<T>(T src) {
            var serilized = JsonUtility.ToJson(src);
            return JsonUtility.FromJson<T>(serilized);
        }


        public Texture2D GetAvatarImage() {
            if (loadedAvatarImage == null) {
                loadedAvatarImage = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImagePath);
            }

            if (loadedAvatarImage) {
                return loadedAvatarImage;
            }
            else {
                if (defaultImage == null) {
                    defaultImage = new Texture2D(100, 100);
                }
                return defaultImage;
            }
        }

        public void SetAvatarImage(Texture2D newAvatarImage) {
            avatarImagePath = AssetDatabase.GetAssetPath(newAvatarImage);
            loadedAvatarImage = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImagePath);
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class Card {
    public string name = "name";
    private static Texture2D defaultImage = null;

    [SerializeField]
    private string avatarImagePath = "";
    private Texture2D loadedAvatarImage = null;


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


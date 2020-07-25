using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 所有卡片编辑器的基类
// 编辑头像，名称，前缀等共通特性
// 子类只需要override OnGUI追加
public abstract class CardEditor : EditorWindow
{
    protected const int WINDOW_WIDTH = 400;

    protected static Dictionary<Card,CardEditor> openingWindows = new Dictionary<Card,CardEditor>();

    protected Card editTarget = null;
    protected Texture2D defaultAvatarImg;

    /// <summary>
    /// 根据卡片类型，实例化对应的编辑器窗口
    /// </summary>
    /// <param name="card"></param>
    public static void ShowWindow(Card card) {
        if (!openingWindows.ContainsKey(card)) {
            CardEditor newWindow = null;
            if (card is CharacterCard) {
                newWindow = CreateInstance<CharacterCardEditor>();
            }
            else if (card is EventCard) {
                newWindow = CreateInstance<EventCardEditor>();
            }
            else if (card is StratagemCard) {
                newWindow = CreateInstance<StratagemCardEditor>();
            }
            else if (card is SubstoryCard) {
                newWindow = CreateInstance<SubstoryCardEditor>();
            }
            else {
                Debug.LogError("unknown type of card");
            }

            if (newWindow) {
                newWindow.Init(card);
                openingWindows[card] = newWindow;
                newWindow.Show();
            }
        }     
    }

    protected virtual void OnGUI() {
        int IMAGE_WIDTH = 200;
        EditorGUILayout.BeginHorizontal();
        // image edit
        DrawCardImageEdit(IMAGE_WIDTH);
        // name edit
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        DrawNameEdit(WINDOW_WIDTH - IMAGE_WIDTH);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DrawDecriptionEdit(WINDOW_WIDTH - IMAGE_WIDTH);
        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();
    }

    protected virtual void Init(Card editTarget) {
        this.editTarget = editTarget;
    }

    private void OnEnable() {
        defaultAvatarImg = new Texture2D(100,100);
    }

    private void OnDisable() {
        EditorUtility.SetDirty(DeckArchive.instance);
        AssetDatabase.Refresh();
        openingWindows.Remove(editTarget);
    }


    private void DrawCardImageEdit(int windowWidth) {
        int IMAGE_HEIGHT = 300;
        // image
        if (editTarget.GetAvatarImage()) {
            var newImg = (Texture2D)EditorGUILayout.ObjectField(
                editTarget.GetAvatarImage(),
                typeof(Texture2D),
                GUILayout.Width(WINDOW_WIDTH),
                GUILayout.Height(IMAGE_HEIGHT));

            if (newImg != editTarget.GetAvatarImage()) {
                editTarget.SetAvatarImage(newImg);
            }
        }
        else {
            var newImg = (Texture2D)EditorGUILayout.ObjectField(
                defaultAvatarImg,
                typeof(Texture2D),
                GUILayout.Width(WINDOW_WIDTH),
                GUILayout.Height(IMAGE_HEIGHT));

            if (newImg != null) {
                editTarget.SetAvatarImage(newImg);
            }
        }
    }

    private void DrawNameEdit(int windowWidth) {
        GUILayout.Label("名称编辑：");
        editTarget.name = EditorGUILayout.TextField(editTarget.name, GUILayout.Width(windowWidth));
    }

    private void DrawDecriptionEdit(int windowWidth) {
        GUILayout.Label("描述编辑：");
        editTarget.description = EditorGUILayout.TextField(editTarget.description, GUILayout.Width(windowWidth),GUILayout.Height(windowWidth));
    }
}


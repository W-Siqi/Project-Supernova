
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class QualiferLibraryEditor : EditorWindow
//{
//    private enum LibType { character,environment}

//    private LibType currentEditType = LibType.character;
//    private string qualiferNameToAdd = "";

//    //[MenuItem("PCG编辑/词缀树")]
//    public static void ShowWindow() {
//        EditorWindow.GetWindow(typeof(QualiferLibraryEditor));
//    }

//    private void OnGUI() {
//        GUILayout.BeginHorizontal();
//        if (currentEditType ==  LibType.character) {
//            GUI.color = Color.green;
//        }
//        if (GUILayout.Button("角色")) {
//            currentEditType = LibType.character;
//        }
//        GUI.color = Color.white;

//        if (currentEditType == LibType.environment) {
//            GUI.color = Color.green;
//        }
//        if (GUILayout.Button("环境")) {
//            currentEditType = LibType.environment;
//        }
//        GUI.color = Color.white;
//        GUILayout.EndHorizontal();

//        switch (currentEditType) {
//            case  LibType.character:
//                EditQualiferLibrary(DeckArchive.instance.characterQualifierLib);
//                break;
//            case LibType.environment:
//                EditQualiferLibrary(DeckArchive.instance.environmentQualifierLib);
//                break;
//            default:
//                throw new System.NotImplementedException();
//                break;
//        }
//    }
//    public void EditQualiferLibrary(QualifierLibrary target) {
//        EditorGUILayout.BeginHorizontal();
//        qualiferNameToAdd = EditorGUILayout.TextField(qualiferNameToAdd);
//        if (GUILayout.Button("+", GUILayout.Width(20))) {
//            target.AddQualifier(qualiferNameToAdd);
//        }
//        EditorGUILayout.EndHorizontal();


//        string toDelete = null;
//        foreach (var qualifierName in target.GetAllQualifiersNames()) {
//            EditorGUILayout.LabelField(qualifierName);
//            if (GUILayout.Button("X", GUILayout.Width(20))) {
//                toDelete = qualifierName;
//            }
//        }
//        if (toDelete != null) {
//            target.RemoveQualifier(toDelete);
//        }
//    }
//}

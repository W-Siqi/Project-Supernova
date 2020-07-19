using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AffairProcessViewer : MonoBehaviour
{
    public class UserInputHandle
    {
        public bool end = false;
    }

    public static AffairProcessViewer instance = null;

    [SerializeField]
    GameObject UIRoot;
    [SerializeField]
    Button endAffairProcess;

    private UserInputHandle currentInputHandle;

    private void Awake()
    {
        instance = this;
        endAffairProcess.onClick.AddListener(()=>OnClickEndAffairProcess());
        EndGovenAffairView();
    }

    public UserInputHandle StartAffairView() {
        UIRoot.SetActive(true);
        currentInputHandle = new UserInputHandle();
        return currentInputHandle;
    }

    public void EndGovenAffairView()
    {
        UIRoot.SetActive(false);
    }

    private void OnClickEndAffairProcess() {
        currentInputHandle.end = true;
        EndGovenAffairView();
    }
}

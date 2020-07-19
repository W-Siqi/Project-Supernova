using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StratagemViewer : MonoBehaviour
{
    public class UserInputHandle {
        public bool decideded = false;
        public bool acceptStratagem = false;
    }

    public static StratagemViewer instance = null;

    [SerializeField]
    GameObject UIRoot;
    [SerializeField]
    Text stratagemDescription;
    [SerializeField]
    Button acceptStrategemButton;
    [SerializeField]
    Button declineStrategemButton;

    private UserInputHandle currentInputHandle;

    private void Awake()
    {
        instance = this;
        EndCouncilView();
        acceptStrategemButton.onClick.AddListener(()=>AcceptCurrentStratagem());
        declineStrategemButton.onClick.AddListener(()=>DeclineCurrentStratagem());
    }

    public void StartCouncilView() {
        UIRoot.SetActive(true);
    }

    public void EndCouncilView()
    {
        UIRoot.SetActive(false);
    }


    public UserInputHandle ViewStratagem(StratagemCard stratagem, CharacterCard provider) {
        var inputHandle = new UserInputHandle();
        currentInputHandle = inputHandle;
        stratagemDescription.text = stratagem.name;
        return inputHandle;
    }



    private void AcceptCurrentStratagem() {
        currentInputHandle.decideded = true;
        currentInputHandle.acceptStratagem = true;
    }

    private void DeclineCurrentStratagem()
    {
        currentInputHandle.decideded = true;
        currentInputHandle.acceptStratagem = false;
    }
}

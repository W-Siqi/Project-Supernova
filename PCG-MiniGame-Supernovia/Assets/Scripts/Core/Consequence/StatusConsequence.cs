using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusConsequence : Consequence
{
    public StatusVector delta = new StatusVector();

    public string CreateDescription() {
        return "导致了状态的改变";
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusConsequence : Consequence
{
    public StoryContext.StatusVector delta = new StoryContext.StatusVector();

    public override void Apply() {
        throw new System.NotImplementedException();
    }
}

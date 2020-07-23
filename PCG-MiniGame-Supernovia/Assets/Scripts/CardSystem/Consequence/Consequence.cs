using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consequence
{
    public bool enabled = true;

    public abstract void Apply();
}

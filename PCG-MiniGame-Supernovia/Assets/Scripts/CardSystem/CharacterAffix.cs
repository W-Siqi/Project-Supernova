using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class  CharacterAffix
{
    public string titleName = "default affix";
}


[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public class AffixDisplayName : Attribute {
    private string name;

    public AffixDisplayName(string name)
    {
        this.name = name;
    }
}

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public class AffixExplain : Attribute
{
    private string explain;

    public AffixExplain(string explain)
    {
        this.explain = explain;
    }
}
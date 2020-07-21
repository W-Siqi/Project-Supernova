using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class  CharacterAffix
{
    public string name {
        get {
            var titleNameAttribute = (AffixTitleName) Attribute.GetCustomAttribute(this.GetType(), typeof(AffixTitleName));
            if (titleNameAttribute != null) {
                return titleNameAttribute.name;
            }

            return this.GetType().Name;
        }
    }
}


[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public class AffixTitleName : Attribute {
    public string name;

    public AffixTitleName(string name)
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
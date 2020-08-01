using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QualiferAlteration {
    public enum Type { add, remove }
    public Qualifier targetQualifier;
    public Type type;

    public static QualiferAlteration CreateByName(string name) {
        return new QualiferAlteration(name);
    }

    public QualiferAlteration() {
        targetQualifier = new Qualifier("NULL");
    }

    public QualiferAlteration(string qualiferName) {
        targetQualifier = new Qualifier(qualiferName);
        type = Type.add;
    }
}

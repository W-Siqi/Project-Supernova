using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 修饰词用 string 作为唯一ID
// TBD: 检测冲突情况
[System.Serializable]
public class QualifierLibrary
{
    [SerializeField]
    private List<Qualifier> qualifiers = new List<Qualifier>();
    [SerializeField]
    private List<string> names = new List<string>();

    // 保证name唯一性
    public void AddQualifier(string name) {
        if (names.Exists((n) => n == name)) {
            return;
        }

        qualifiers.Add(new Qualifier(name));
        names.Add(name);
    }

    public void RemoveQualifier(string name) {
        foreach (var q in qualifiers) {
            if (q.name == name) {
                qualifiers.Remove(q);
                break;
            }
        }

        names.Remove(name);
    }

    public string[] GetAllQualifiersNames() {
        return names.ToArray();
    }

    public string[] GetQualiferNamesWithBlackList(List<Qualifier> blacklist) {
        var candidates = new List<string>();
        foreach (var name in GetAllQualifiersNames()) {
            if (!blacklist.Exists((existed) => existed.name == name)) {
                candidates.Add(name);
            }
        }
        return candidates.ToArray();
    }
}

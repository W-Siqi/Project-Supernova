using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 修饰词，目前环境修饰词和角色修饰词都共用这个类
[System.Serializable]
public struct Qualifier
{
    public string name;
    public Qualifier(string name) {
        this.name = name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 必须提供一个new(T t)的构造函数, (TBD: 重构成模板)
/// </summary>
// 控件类是专门渲染一个单一对象的
// 如PreconditonSetWidget 渲染 PreconditonSet
public abstract class Widget
{
    public abstract void RenderUI();
}

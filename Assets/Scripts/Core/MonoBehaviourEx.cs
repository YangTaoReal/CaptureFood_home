using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
public class MonoBehaviourEx : MonoBehaviour
{
    /// <summary>
    /// auto set every view public property 
    /// </summary>
    /// <param name="go"></param> 
    protected void AutoSetGoProperty<T>(T comp, GameObject go)
    {
        Type tempt = comp.GetType();
        foreach (FieldInfo fi in tempt.GetFields())
        {
            if (!fi.Name.Contains("ui_")) continue;
            Component tempcom = ToolsEx.FindScriptInChild(go, fi.FieldType, fi.Name.Replace("ui_", ""));
            if (tempcom == null)
            {
                Debug.LogError(fi.Name + ". is not find in " + tempt.Name);
                continue;
            } 
            //GenerateLSCODE(tempcom, tempt.Name);   //
            fi.SetValue(comp, tempcom);
        }
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using DG.Tweening;
public abstract class BasePanel : MonoBehaviour,  IObserver
{
    protected string PanelName = "";
    protected List<BaseModel> listModel = new List<BaseModel>();
    public static GameObject LoadGameObject(string name)
    {
        GameObject go = Instantiate(ResourceHelper.GetInstance().Load(name)) as GameObject;
        return go;
    }
    public static void LoadGameObjectAsyc<T>(string name, T t, LoadComplentBack lcb) where T : BasePanel
    {
        if (Util.buildBundle)
            ResourceHelper.GetInstance().LoadAsyc(name, (_go) =>
                {
                    GameObject go = Instantiate(_go) as GameObject;
                    t = go.AddComponent<T>();
                    string[] strarr = name.Split('/');
                    t.PanelName = strarr[strarr.Length - 1];
                    t.InitPro(false);
                    lcb(go);
                });
        else
        {
            GameObject go = Instantiate(ResourceHelper.GetInstance().Load(name)) as GameObject;
            t = go.AddComponent<T>();
            string[] strarr = name.Split('/');
            t.PanelName = strarr[strarr.Length - 1];
            t.InitPro(false);
            lcb(go);
        }
    }

    protected static T LoadObjAsyc<T>(string path) where T : Component
    {
        var obj = Resources.LoadAsync(path).asset as GameObject;
        var com = Instantiate(obj, Vector3.zero, Quaternion.identity, GameCtrl._Ins.UICanvas.transform).AddComponent<T>();
        return com;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_needAni">是否需要动画打开</param>
    protected void InitPro(bool _needAni = false)
    {
        transform.SetParent(GameCtrl._Ins.UICanvas.transform, false);
        transform.localScale = Vector3.one;
        NeedAniOpen = _needAni;
        this.AutoSetGoProperty();
        this.CreateInit();
    }
    public abstract void CreateInit();
    public abstract void ShowInit();
    public abstract void CloseInit();
    public abstract void Notifyed<T>(int id,T data);
    public void Show()
    {
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        PanelUtil.RecordOpenPanel(this);
        if (NeedAniOpen)
            TweenScale(true);
        ShowInit();
    }
    public Tweener ScaleTweener;
    public bool NeedAniOpen;
    private void TweenScale(bool isopen)
    {
        if(ScaleTweener!=null)
            ScaleTweener.Kill();
        if (isopen)
        {
            transform.localScale = Vector3.zero;
            ScaleTweener = transform.DOScale(1, 0.2f);
        }
        else
        {
            ScaleTweener = transform.DOScale(0.2f, 0.2f);
        }
    }
    public void AddModel(BaseModel _model)
    {
        if (!listModel.Contains(_model))
        {
            _model.AddObserver(this);
            listModel.Add(_model);
        }
    }
    public void Close()
    {
        listModel.FindAll((a) => {
            a.RemoveObserver(this);
            return false;
        });
        listModel.Clear();
        //TweenScale(false);
        gameObject.SetActive(false);
        CloseInit();
        PanelUtil.RecordClosePanel(this);
    }
    public void CloseAll()
    { 
        
    }
    protected void AutoSetGoProperty()
    {
        Type tempt = this.GetType();
        foreach (FieldInfo fi in tempt.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
        {
            if (!fi.Name.Contains("ui_")) continue;
            Component tempcom = ToolsEx.FindScriptInChild(this.gameObject, fi.FieldType, fi.Name.Replace("ui_", ""));
            if (tempcom == null)
            {
                Debug.LogError(fi.Name + ". is not find in " + tempt.Name);
                continue;
            }
            fi.SetValue(this, tempcom);
        }
    }
    public bool isOpen()
    {
        return PanelUtil.PaneIsOpen(this);
    }




}

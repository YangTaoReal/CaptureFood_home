using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ResourceHelper:MonoBehaviour {
    private static Dictionary<string, AssetBundle> AllAssetBundle;
    private static AssetBundleManifest MainManiFest;

    private static Dictionary<string, List<string>> AsycAssetBundleNeedLoad;//异步加载某个对象的自身和依赖的映射关系；
    private static Dictionary<string, LoadComplentBack> AsycAssetBundleComPlentBack;//异步加载某个对象完成回调；
    private static Dictionary<string, string> AsycAssetBundleCurName;//异步加载某个对象完成回调；
    public static GameObject GetObjByPath(string path)
    {
        Object gb = Resources.Load(path);
        if (gb == null)
        {
            return null;
        }
        return gb as GameObject;
    }
    public static ResourceHelper Instance;
    public static ResourceHelper GetInstance()
    {
        if (Instance == null)
        {
            if (GameCtrl._Ins!=null)
                Instance = GameCtrl._Ins.gameObject.AddComponent<ResourceHelper>();
            else
                Instance = GameObject.Find("Canvas").gameObject.AddComponent<ResourceHelper>();
            Instance.Init();
        }
        return Instance;
    }
    private void Init()
    {
        if (AllAssetBundle == null)
        {
            //AllAssetBundle = new Dictionary<string, AssetBundle>();
            //string tmpstr = Util.DataPath + Util.AppName;
            //if (File.Exists(tmpstr))
            //{
            //    AssetBundle tmpab = AssetBundle.LoadFromFile(tmpstr);
            //    MainManiFest = (AssetBundleManifest)tmpab.LoadAsset("AssetBundleManifest", typeof(AssetBundleManifest));
            //}
            //else
            //{
            //    MyDebug.Log("PC端？是PC端的话忽略！没生产文件啊！！老大！");
            //}
        }
    }
    public Object Load(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        //string tempname = name.Trim().ToLower();
        string tempname = name;
        GameObject go;
        if (Util.buildBundle)
        {
            go = GetUIRes(name) as GameObject;
        }
        else
        {
            go = Resources.Load(tempname) as GameObject;
        }
        return go;
    }
    public void LoadAsyc(string name,LoadComplentBack _lcb)
    {
        if (string.IsNullOrEmpty(name))
            return;
        if (Util.buildBundle)
        {
            string[] tmpName = name.Split('/');
            int tmpCount = tmpName.Length;
            string curName = tmpName[tmpCount - 1];
            string bundleFristName = tmpName[0].ToLower() + "/" + tmpName[1].ToLower();
            if(AsycAssetBundleNeedLoad == null)
            AsycAssetBundleNeedLoad = new Dictionary<string, List<string>>();
            if(AsycAssetBundleCurName==null)
            AsycAssetBundleCurName = new Dictionary<string, string>();
            if(AsycAssetBundleComPlentBack==null)
            AsycAssetBundleComPlentBack = new Dictionary<string, LoadComplentBack>();

            if (AsycAssetBundleComPlentBack.ContainsKey(bundleFristName))
            {
                Debug.LogError("正在异步加载中：");
                return;
            }
            else
            {
                List<string> Ls = new List<string>();
                Ls.Add(bundleFristName);
                AsycAssetBundleNeedLoad.Add(bundleFristName, Ls);
                AsycAssetBundleCurName.Add(bundleFristName,curName);
                AsycAssetBundleComPlentBack.Add(bundleFristName, _lcb);
                GetUIResAsyc(name);
            }
        }
        else
        {
            GameObject go = Resources.Load(name) as GameObject;
            _lcb(go);
        }
    }

    public Object LoadResourcesAsSprite(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        //if (Util.buildBundle)
        //{
        //    return GetUIRes(name, typeof(Sprite)) as Sprite;
        //}
        string tempname = name.Trim().ToLower();
        Sprite go = Resources.Load(tempname, typeof(Sprite)) as Sprite;
        if (go == null)
        {
            MyDebug.Log("LoadResourcesAsSprite 没有找到对应资源:" + name);
            return null;
        }
       
        go.name = name;
        return go;
    }
    public Sprite[] LoadResourcesAsSpriteList(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        if (Util.buildBundle)
        {
            return GetUISpriteList(name);
        }
        string tempname = name.Trim().ToLower();
        Sprite[] golist = Resources.LoadAll<Sprite>(tempname);
        if (golist == null)
        {
            MyDebug.Log("LoadResourcesAsSprite 没有找到对应资源:" + name);
            return null;
        }
        return golist;
    }
    public Sprite[] GetUISpriteList(string resName)
    {
        string[] tmpName = resName.Split('/');
        int tmpCount = tmpName.Length;
        string curName = tmpName[tmpCount - 1];
        string bundleFristName = tmpName[0].ToLower() + "/" + tmpName[1].ToLower();
        if (!AllAssetBundle.ContainsKey(bundleFristName))
        {
            string tmppath = Util.DataPath + bundleFristName + ".ld";
            if (!File.Exists(tmppath))
            {
                return null;
            }
            AssetBundle tmpBundle = AssetBundle.LoadFromFile(tmppath);
            AllAssetBundle.Add(bundleFristName, tmpBundle);


            if (MainManiFest != null)
                loadManifestDependencies(bundleFristName + ".ld");
            return tmpBundle.LoadAssetWithSubAssets<Sprite>(curName);
        }
        else
        {
            Sprite[] ob = AllAssetBundle[bundleFristName].LoadAssetWithSubAssets<Sprite>(curName);
            //Sprite[] ob = AllAssetBundle[bundleFristName].LoadAsset(curName + ".png");
                if (ob == null)
                    Debug.LogError("ob == null");
                return ob;
        }
    }
    public Object GetUIRes(string resName, System.Type type = null)
    {
        string[] tmpName = resName.Split('/');
        int tmpCount = tmpName.Length;
        string curName = tmpName[tmpCount - 1];
        string bundleFristName = tmpName[0].ToLower() + "/" + tmpName[1].ToLower();
        if (!AllAssetBundle.ContainsKey(bundleFristName))
        {
            string tmppath = Util.DataPath + bundleFristName + ".ld";
            if (!File.Exists(tmppath))
            {
                return null as Object;
            }
            AssetBundle tmpBundle = AssetBundle.LoadFromFile(tmppath);
            AllAssetBundle.Add(bundleFristName, tmpBundle);


            if (MainManiFest != null)
                loadManifestDependencies(bundleFristName + ".ld");

            if (type == null)
            {
                return tmpBundle.LoadAsset(curName);
            }
            else
            {
                return tmpBundle.LoadAsset(curName, type);
            }
        }
        else
        {
            if (type == null)
            {
                return AllAssetBundle[bundleFristName].LoadAsset(curName);
            }
            else
            {
                Object ob = AllAssetBundle[bundleFristName].LoadAsset(curName, type);
                if (ob == null)
                    Debug.LogError("ob == null");
                return ob;
            }
        }
    }
    public void GetUIResAsyc(string resName)
    {
        string[] tmpName = resName.Split('/');
        int tmpCount = tmpName.Length;
        string curName = tmpName[tmpCount - 1];
        string bundleFristName = tmpName[0].ToLower() + "/" + tmpName[1].ToLower();

        if (!AllAssetBundle.ContainsKey(bundleFristName))
        {
            string tmppath = Util.DataPath + bundleFristName + ".ld";
            if (!File.Exists(tmppath))
            {
                LoadComplentBack cmback = null;
                AsycAssetBundleComPlentBack.TryGetValue(bundleFristName, out cmback);
                if (cmback != null)
                    cmback(null);
                AsycAssetBundleComPlentBack.Remove(bundleFristName);
                AsycAssetBundleNeedLoad.Remove(bundleFristName);
                AsycAssetBundleCurName.Remove(bundleFristName);
            }
            if (MainManiFest != null)
                loadManifestDependenciesAsyc(bundleFristName + ".ld");
            StartCoroutine(LoadAsycBundle(tmppath, bundleFristName, bundleFristName));
        }
        else
        {
            LoadComplentBack cmback = null;
            AsycAssetBundleComPlentBack.TryGetValue(bundleFristName, out cmback);
            if (cmback!=null)
                cmback(AllAssetBundle[bundleFristName].LoadAsset(curName));
            AsycAssetBundleComPlentBack.Remove(bundleFristName);
            AsycAssetBundleNeedLoad.Remove(bundleFristName);
            AsycAssetBundleCurName.Remove(bundleFristName);
        }
    }
    /// <summary>
    /// 开始异步加载一个bundle
    /// </summary>
    /// <param name="urlname">url地址</param>
    /// <param name="_realName">bundle名字，用于缓存使用</param>
    /// <param name="MainAboutName">主要bundle，因为可能当前下载的bundle是被依赖所以需要下载的</param>
    /// <returns></returns>
    IEnumerator LoadAsycBundle(string urlname,string _realName,string _MainAboutName)
    {
        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(urlname);
        string MainAboutName = _MainAboutName;
        string realName = _realName;
        yield return bundleLoadRequest;
        var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
        if (myLoadedAssetBundle != null)
        {
            if(AllAssetBundle.ContainsKey(realName))
            {
                AllAssetBundle[realName] = myLoadedAssetBundle;
            }else
            {
                AllAssetBundle.Add(realName, myLoadedAssetBundle);
            }
        }
        if (AsycAssetBundleNeedLoad.ContainsKey(MainAboutName))
        {
            List<string> lst = new List<string>();
            AsycAssetBundleNeedLoad.TryGetValue(MainAboutName, out lst);
            if(lst.Contains(realName))
            {
                lst.Remove(realName);
            }
            if (lst.Count == 0)
            {
                AsycAssetBundleNeedLoad.Remove(MainAboutName);
                LoadComplentBack cmback = null;
                if (AsycAssetBundleComPlentBack.ContainsKey(MainAboutName))
                {
                    AsycAssetBundleComPlentBack.TryGetValue(MainAboutName, out cmback);
                    AsycAssetBundleComPlentBack.Remove(MainAboutName);
                }
                string curname = null;
                if (AsycAssetBundleCurName.ContainsKey(MainAboutName))
                {
                    AsycAssetBundleCurName.TryGetValue(MainAboutName, out curname);
                    AsycAssetBundleCurName.Remove(MainAboutName);
                }
                if (cmback != null && curname!=null)
                    if (AllAssetBundle.ContainsKey(MainAboutName))
                    {
                        cmback(AllAssetBundle[MainAboutName].LoadAsset(curname));
                    }
            }
        }
    }

    public void loadManifestDependencies(string resName)
    {
        string[] allManifest = MainManiFest.GetAllDependencies(resName);
        foreach (string sigMF in allManifest)
        {
            string curstr = Util.DataPath + sigMF;
            string tmpName = sigMF.Replace(".ld", "");
            if (!AllAssetBundle.ContainsKey(tmpName))
            {
                if (File.Exists(curstr))
                {
                    AssetBundle tmpBundle = AssetBundle.LoadFromFile(curstr);
                    AllAssetBundle.Add(tmpName, tmpBundle);
                }
            }
        }
    }
    /// <summary>
    /// 异步加载依赖关系
    /// </summary>
    /// <param name="resName"></param>
    public void loadManifestDependenciesAsyc(string resName)
    {
        string[] allManifest = MainManiFest.GetAllDependencies(resName);
        foreach (string sigMF in allManifest)
        {
            string curstr = Util.DataPath + sigMF;
            string tmpName = sigMF.Replace(".ld", "");
            if (!AllAssetBundle.ContainsKey(tmpName))
            {
                if (File.Exists(curstr))
                {
                    List<string> Ls = null;
                    string MainAboutName = resName.Replace(".ld", "");
                    if (AsycAssetBundleNeedLoad.ContainsKey(MainAboutName))
                        AsycAssetBundleNeedLoad.TryGetValue(MainAboutName,out Ls);
                    if (Ls!=null)
                    {
                        if (!Ls.Contains(tmpName))
                        {
                            Ls.Add(tmpName);
                        }
                    }
                    StartCoroutine(LoadAsycBundle(curstr, tmpName, resName.Replace(".ld", "")));
                }
            }
        }
    }
}
public delegate void LoadComplentBack(Object value);

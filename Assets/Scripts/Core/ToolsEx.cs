﻿/*
 * Class Name: Tools
 * Desc: 一个工具类，提供静态方法
 * Date: 2013-8-30
 * Author：jsw
 * PlatformDependentCompilation.html
 * UNITY_IPHONE
 * UNITY_ANDROID
 * UNITY_WP8
 * UNITY_STANDALONE_WIN
*/
//UNITY_EDITOR	Define for calling Unity Editor scripts from your game code.
//UNITY_EDITOR_WIN	Platform define for editor code on Windows.
//UNITY_EDITOR_OSX	Platform define for editor code on Mac OSX.
//UNITY_STANDALONE_OSX	Platform define for compiling/executing code specifically for Mac OS (This includes Universal, PPC and Intel architectures).
//UNITY_DASHBOARD_WIDGET	Platform define when creating code for Mac OS dashboard widgets.
//UNITY_STANDALONE_WIN	Use this when you want to compile/execute code for Windows stand alone applications.
//UNITY_STANDALONE_LINUX	Use this when you want to compile/execute code for Linux stand alone applications.
//UNITY_STANDALONE	Use this to compile/execute code for any standalone platform (Mac, Windows or Linux).
//UNITY_WEBPLAYER	Platform define for web player content (this includes Windows and Mac Web player executables).
//UNITY_WII	Platform define for compiling/executing code for the Wii console.
//UNITY_IPHONE	Platform define for compiling/executing code for the iPhone platform.
//UNITY_ANDROID	Platform define for the Android platform.
//UNITY_PS3	Platform define for running PlayStation 3 code.
//UNITY_XBOX360	Platform define for executing Xbox 360 code.
//UNITY_FLASH	Platform define when compiling code for Adobe Flash.
//UNITY_BLACKBERRY	Platform define for a Blackberry10 device.
//UNITY_WP8	Platform define for Windows Phone 8.
//UNITY_METRO	Platform define for Windows Store Apps (additionally NETFX_CORE is defined when compiling C# files against .NET Core).
//UNITY_WINRT	Equivalent to UNITY_WP8 |UNITY_METRO
/* 
 * 3.WeChat  
 * 5.PVP
 * 6.TalkingData
 * 7.BACKUP 
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection; 

/// <summary>
/// Tools 已经存在，重命名
/// </summary>
public class ToolsEx
{
    /// <summary>
    /// Get the rootmost object of the specified game object.
    /// </summary>

    public static GameObject GetRoot(GameObject go)
    {
        Transform t = go.transform;

        for (; ; )
        {
            Transform parent = t.parent;
            if (parent == null) break;
            t = parent;
        }
        return t.gameObject;
    }

	/// <summary>
	/// 是否包含激活的子对象
	/// </summary>
	public static bool HasActiveChild( Transform trans )
	{
		if( trans == null )
			return false;
		int len = trans.childCount;
		for( int i=0; i<len; ++i )
		{
			if( trans.GetChild( i ).gameObject.activeSelf )
				return true;
		}
		return false;
	}

    public static Vector3 GetMiddlePosition(List<Vector3> vlist)
    {
        if (vlist == null || vlist.Count == 0 || vlist.Count == 1) return Vector3.zero;
        Vector3 rev = Vector3.zero;
        while(vlist.Count  != 1)
        {
           rev = GetMiddlePosition(vlist[0], vlist[1]);
           vlist.RemoveAt(0);
           vlist[0] = rev;
        }        
        return rev;
    }
    private static Vector3 GetMiddlePosition(Vector3 v1, Vector3 v2)
    { //A+(B-A)/2 = A+(B/2)-(A/2) = (A-A/2)+(B/2) = (1A - (1/2)A) + (B/2) = (1/2)A + (B/2) = (A/2) + (B/2) = (A+B)/2
        Vector3 rev = Vector3.zero;
        rev = v1 + (v2 - v1) / 2.0f;
        return rev;
    }
    /// <summary>
    /// 如果已存则COPY值   此方法仅能在编辑器下使用
    /// </summary>
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = null;
#if UNITY_EDITOR
        copy = destination.GetComponent(type);
        if (copy == null) { copy = destination.AddComponent(type); }
        if (copy == null)
        {
            Debug.LogError(" copy == null");
            return null;
        }
        if (UnityEditorInternal.ComponentUtility.CopyComponent(original))
        {
            if (UnityEditorInternal.ComponentUtility.PasteComponentValues(copy))
            {
                //Debug.Log("DONE");
            }
        }
#endif

#if UNITY_IPHONE || UNITY_ANDROID  

        copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
#endif
        return copy as T;
    }
    
    /// <summary>
    /// Oblique Frustum
    /// </summary>
    /// <param name="horizObl">在1或-1之间</param>
    /// <param name="vertObl"></param>
    /// <param name="cam"></param>
    public static void SetObliqueness(float horizObl, float vertObl, Camera cam)
    {
        Matrix4x4 mat = cam.projectionMatrix;
        mat[0, 2] = horizObl;
        mat[1, 2] = vertObl;
        cam.projectionMatrix = mat;
    }
    /// <summary>
    /// 设置局部旋转
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="rot"></param>
    public static void SetLocalRotation(Transform tf, Vector3 rot)
    {
        Quaternion qua = new Quaternion();
        qua.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
        tf.localRotation = qua;
    }
   
    /// <summary>
    /// 全局种子， 用静态才有用
    /// </summary>
    private static int Seeds = 0;
    /// <summary>
    /// add by jsw 20140120
    /// </summary>
    /// <param name="min">可以取下界值</param>
    /// <param name="max">不能取上界值</param>
    /// <returns></returns>
    public static int GetRandom(int min, int max)
    {
        object obj = new object();
        lock (obj)
        {
            Seeds += Convert.ToInt32(DateTime.Now.Ticks & 0xffff);
            Seeds = Seeds * (Seeds / 7);
            if (max < min)
            {
                return min;
            }
            if (Seeds >= int.MaxValue)
            {
                return 0;
            }
        }
        return new System.Random(Seeds).Next(min, max);
    }
    public static int GetRandomRemove(int min, int max, int remove)
    {
        if (max == min) return 0;
        int r = 0;
        do
        {
            r = GetRandom(min, max);
        }
        while (r == remove);
        return r;
    }
     
    /// <summary>
    /// 不支持Flash或4.6以前版本。 Finds the specified component on the game object or one of its parents.
    /// </summary>

    public static T FindInParents<T>(Transform trans) where T : Component
    {
        if (trans == null) return null;

        return trans.GetComponentInParent<T>();
    }
    /// <summary>
    /// 不支持Flash或4.6以前版本。 Finds the specified component on the game object or one of its parents.
    /// </summary>

    public static T FindInParents<T>(Transform trans, string eName) where T : Component
    {
        if (trans == null) return null;

        foreach (T e in trans.GetComponentsInParent<T>(true))
        {
            if (e.gameObject.name == eName)
            {
                return e;
            }   
        }
 
        return null; 
    }
    /// <summary>
    /// 用于反射不能手动输入类型的
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="type"></param>
    /// <param name="elementName"></param>
    /// <returns></returns>
    public static Component FindScriptInChild(GameObject _go, Type type, string elementName)
    {
        if (_go == null) return null;
        //Debug.LogError("go:" + _go.name + "type:" + type + "eName:" + elementName);
        try
        {

            Component[] tempcom = _go.GetComponentsInChildren(type, true);
            foreach (Component element in tempcom)
            {
                if (element.gameObject.name == elementName)
                {
                    return element;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("go:" + _go.name + "type:" + type + "eName:" + elementName);
        }
        //UnityEngine.Debug.Log(tempcom.Length + " tempcom.Length  elementName:" + elementName + " _go.active :" + _go.activeSelf);

        return null;
    }
    /// <summary>
    /// 获取第一个相同名称的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <param name="elementName"></param>
    /// <returns></returns>
    public static T FindScriptInChilds<T>(GameObject _go, string elementName) where T : Component
    {
        if (_go == null) return null;
        T[] arr = _go.GetComponentsInChildren<T>(true);
        foreach (T element in arr)
        {
            if (element.gameObject.name == elementName)
            {
                return element;
            }            
        }
        return null;
    }

    /// <summary>
    /// 根据组件名称 包含查找（模糊查找）反回对象列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <param name="elementName"></param>
    /// <returns></returns>
    public static List<T> FindScriptsInChild<T>(GameObject _go, string elementName) where T : Component
    {
        if (_go == null) return null;
        List<T> tlist = new List<T>();
        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            if (element.gameObject.name.IndexOf(elementName) != -1)
            {
                tlist.Add(element);
            }
        }
        return tlist;
    }
    /// <summary>
    /// 根据组件名称 包含查找（模糊查找）反回对象列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <param name="elementName"></param>
    /// <returns></returns>
    public static List<T> FindScriptsInChild<T>(GameObject _go) where T : Component
    {
        if (_go == null) return null;
        List<T> tlist = new List<T>();
        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {          
                tlist.Add(element); 
        }
        return tlist;
    }
    /// <summary>
    /// 找到第一个指定类型的脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static T FindScriptInChild<T>(GameObject _go) where T : Component
    {
        if (_go == null) return null;
        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            return element;
        }
        return null;
    }
    /// <summary>
    /// 找到第一个指定类型的脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static T FindScriptInChild<T>(GameObject _go, string name) where T : Component
    {
        if (_go == null) return null;
        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            return element;
        }
        return null;
    }
    /// <summary>
    /// 找到根节点下所有的指定脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static List<T> FindScriptInChilds<T>(GameObject _go) where T : Component
    {
        if (_go == null) return null;
        List<T> tlist = new List<T>();
        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            tlist.Add(element);
        }
        return tlist;
    }
     
    public static GameObject FindGameObject(GameObject rootObject, string goName)
    {
        if (rootObject == null) return null;
        foreach (Transform t in rootObject.GetComponentsInChildren<Transform>(true))
        {
            if (t.gameObject.name == goName)// if (t.gameObject.name.Contains(goName))
            {
                return t.gameObject;
            }
        }
        return null;
    } 
 
    /// <summary>
    /// 通过物体来找到脚本，查找 其子类的
    /// </summary>
    /// <typeparam name="T"> 类型</typeparam>
    /// <param name="_go"> 需要查找的物体</param>
    /// <returns></returns>
    public static T FindScriptInChildren<T>(GameObject _go) where T : Component
    {
        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            return element;
        }
        return null;
    }
    /// <summary>
    /// 通过物体来找到脚本，查找自身和其子类的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static T FindScriptSelf_Children<T>(GameObject _go, string name) where T : Component
    {
        if (_go == null) return null;
        foreach (T element in _go.GetComponents<T>())
        {
            if (element.gameObject.name == name) return element;
        }

        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            if (element.gameObject.name == name)  return element;
        }
        return null;
    }
    /// <summary>
    /// 通过物体来找到脚本，查找自身和其子类的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_go"></param>
    /// <returns></returns>
    public static T FindScriptSelf_Children<T>(GameObject _go) where T : Component
    {
        if (_go == null) return null;
        foreach (T element in _go.GetComponents<T>())
        {
            return element;
        }

        foreach (T element in _go.GetComponentsInChildren<T>(true))
        {
            return element;
        }
        return null;
    }
    /// <summary>
    /// 获取指定数的第几位的值 ，肯定小于10的值 
    /// </summary>
    /// <param name="num"></param>
    /// <param name="wei">0个，1十，2百</param>
    /// <returns>-1 是错误值 </returns>
    public static int GetNumbyNum(int num, int wei)
    {
        string strnum = num.ToString();
        if (wei >= strnum.Length) return -1;
        return Convert.ToInt32(strnum.Substring(wei, 1));
    }

    /// <summary>
    /// 把秒 转换成 时间格式的字符串 如：01:10:56
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns></returns>
    public static string ConverttoData(int seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, seconds);
        return string.Format("{0}:{1}:{2}", ts.Hours <= 9 ? "0" + ts.Hours : ""+ts.Hours ,
             ts.Minutes <= 9 ? "0" + ts.Minutes : "" + ts.Minutes,
             ts.Seconds <= 9 ? "0" + ts.Seconds : "" + ts.Seconds); 
    }

    public static string CoverttoCountdown(int seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, 0, seconds);
        int h = ts.Days * 24 + ts.Hours;
        return string.Format("{0}:{1}:{2}", h <= 9 ? "0" + h : "" + h,
             ts.Minutes <= 9 ? "0" + ts.Minutes : "" + ts.Minutes,
             ts.Seconds <= 9 ? "0" + ts.Seconds : "" + ts.Seconds);
    }

    public static string coverttoDatetime(string timeStamp)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dateTimeStart.Add(toNow).ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string ConverttoDataALL(int sec)
    {
        DateTime dt = new DateTime(0, 0, 0, 0, 0, sec);
        return dt.ToLongTimeString();
    }
    /// <summary>
    /// 删除指定对象的所有子物体 
    /// </summary>
    /// <param name="go"></param>
    public static void ClearChildren(Transform playerInfoNode, string strtemplate)
    {
        if (playerInfoNode == null) return;
        //清空玩家信息列表
        List<GameObject> listToClear = new List<GameObject>();
        for (int i = 0; i < playerInfoNode.childCount; i++)
        {
            GameObject playerInfo = playerInfoNode.GetChild(i).gameObject;
            if (playerInfo.name != strtemplate)
            {//加入删除列表
                listToClear.Add(playerInfo);
            }
        }

        foreach (GameObject gotemp in listToClear)
        {
            GameObject.Destroy(gotemp);
        }
    }

    /// <summary>
    /// 设置指定物体的层级 Layer 
    /// </summary>
    /// <param name="_go"></param>
    /// <param name="_layer"></param>
    public static void SetGameObjectLayer(GameObject _go, int _layer)
    {
        if (_go == null) return;
        _go.layer = _layer;
        Transform trans = _go.transform;
        for (int i = 0, imax = trans.childCount; i < imax; ++i)
        {
            Transform t = trans.GetChild(i);
            SetGameObjectLayer(t.gameObject, _layer);
        }
    }

    /// <summary>
    /// 产生空的物件
    /// </summary>
    /// <param name="_pa"></param>
    /// <returns></returns>
    public static GameObject CreateObj(Transform _pa)
    {
        GameObject _obj = new GameObject();
        if(_pa != null)
        {
            _obj.transform.parent = _pa.transform;
        }
        _obj.transform.localPosition = Vector3.zero;
        _obj.transform.localRotation = Quaternion.identity;
        _obj.transform.localScale = Vector3.one;
        return _obj;
    }
    public static GameObject CreateCube_test(Transform _pa)
    {
        GameObject _obj =   GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (_pa != null)
        {
            _obj.transform.parent = _pa.transform;
        }
        _obj.transform.localPosition = Vector3.zero;
        _obj.transform.localRotation = Quaternion.identity;
        _obj.transform.localScale = Vector3.one;
        return _obj;
    }
     


    private static Material mat_img_gray;
    public static void SetImageGray(Image img)
    {
        if (img == null) return;
        if(mat_img_gray == null)
        {
            mat_img_gray = new Material(Shader.Find("Custom/UGUIGray"));
        }
        img.material = mat_img_gray;      
    }
    public static void SetImageDefault(Image img)
    {
        if (img == null) return;
        img.material = null;      
    }

    public static float angle_360(Vector3 from_, Vector3 to_)
    {

        Vector3 v3 = Vector3.Cross(from_, to_);
        if (v3.z > 0)

            return Vector3.Angle(from_, to_);

        else

            return 360 - Vector3.Angle(from_, to_);

    }
    /// <summary>
    /// 转换坐标
    /// st ： 地图坐标，TileW ：地图块宽度 TileH ：地图块高度，x0，y0 ，地图起始坐标
    /// 地图0,0 坐标Vector2(128,0);
    /// </summary>
    public static Vector2 ConvertToLocalPos(Vector2 st, int TileW, int TileH, int x0, int y0)
    {
        int N = (int)((st.x + st.y) * TileW * 0.5f) + x0;
        int M = (int)((st.y - st.x) * TileH * 0.5f) + y0;
        //string news = System.String.Format("N is  : {0}, M is : {1}",N,M);
        return new Vector2(N, M);
    }
    /// <summary>
    /// 直接赋值Transform
    /// </summary>
    /// <param name="ori"></param>
    /// <param name="target"></param>
    public static void SetTranform(Transform ori, Transform target)
    {
        target.localPosition = ori.localPosition;
        target.localRotation = ori.localRotation;
        target.localScale = ori.localScale;
    }
    /// <summary>
    /// 点绕某个向量旋转angle后的坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 RotateByDir(Vector3 p, Vector3 n, float angle)
    {
        float nx = n.x;
        float ny = n.y;
        float nz = n.z;
        double sin = Math.Sin(angle);
        double cos = Math.Cos(angle);
        double i_cos = (1 - cos);

        double x = p.x * (nx * nx * i_cos + cos) + p.y * (nx * ny * i_cos - nz * sin) + p.z * (nx * nz * i_cos + ny * sin);
        double y = p.x * (nx * ny * i_cos + nz * sin) + p.y * (ny * ny * i_cos + cos) + p.z * (ny * nz * i_cos - nx * sin);
        double z = p.x * (nx * nz * i_cos - ny * sin) + p.y * (ny * nx * i_cos + nx * sin) + p.z * (nz * nz * i_cos + cos);
        p = new Vector3((float)x, (float)y, (float)z);
        return p;
    }

    ///<summay>
    ///继承MonoBehaviour的，自动查找UI 
    /// </summary>
    public static void AutoSetUIProperty<T>(T view, GameObject WinPrefab)
    {
        Type tempt = view.GetType();
        foreach (FieldInfo fi in tempt.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
        {
            if (!fi.Name.Contains("ui_")) continue;
            //Debug.LogError(fi.FieldType.Name + ". fi.PropertyType.Name.." + fi.Name); 
            Component tempcom = ToolsEx.FindScriptInChild(WinPrefab, fi.FieldType, fi.Name.Replace("ui_", ""));
            if (tempcom == null)
            {
                Debug.LogError(fi.Name + ". is not find in " + tempt.Name);
                continue;
            }
            // GenerateLSCODE(tempcom, tempt.Name);   //
            fi.SetValue(view, tempcom);
            //Debug.LogError(fi.Name);
        }
    }  



    ///<summary>
    ///当数字达到10W以上转换为10万
    ///</summary>
    public static string intTransformstring(int count)
    {
        string append = string.Empty;
        string str = count.ToString();
        if (str.Length >= 6)
        {
            append = (count / 10000).ToString() + "万";

        }
        else
        {
            append = count.ToString();
        }
        return append;
    }

    public static string intTransformStringKTM(int count)
    {
        string appennd = string.Empty;
        string str = count.ToString();
        if (str.Length >= 4 && str.Length < 7)
            appennd = (count / 1000).ToString() + "K";
        else if (str.Length >= 7)
            appennd = (count / 1000000).ToString() + "M";
        else
            appennd = count.ToString(); 
        return appennd;
    }

	///<summary>
	///当数字达到1000上转换为1k
	///</summary>
	public static string intTransformstringK(int count)
	{
		string append = string.Empty;
		string str = count.ToString();
		if (str.Length >= 4)
		{
			append = (count / 1000).ToString() + "k";
			
		}
		else
		{
			append = count.ToString();
		}
		return append;
	}

    /// <summary>
    /// 当排行榜的 名次小于等于6位数的时候 显示 第xxxxxx名
    ///            名次大于等于6位数的时候 显示 xxxxxxxx名
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    public static string rankTransformstring(int rank)
    {

        string append = string.Empty;
        string str = rank.ToString();
        if (str.Length <= 5)
        {
            append ="第"+rank+ "名";

        }
        else
        {
            append = (rank / 10000).ToString() + "名";
        }
        return append;
    }

    public static Dictionary<int, int> PosToClientPos;    // key  真实位置,   客户端显示位置. 自己位置始终为1
    public static Dictionary<int, int> LocalPos;          // key 客户端显示位置     真实的位置
 
    public static void SetOtherPosition(int BottomPos)
    {
        PosToClientPos = new Dictionary<int, int>();
        LocalPos = new Dictionary<int, int>();
        switch (BottomPos)
        {
            case 1:
                PosToClientPos.Add(1, 1);
                PosToClientPos.Add(2, 2);
                PosToClientPos.Add(3, 3);
                PosToClientPos.Add(4, 4);
                LocalPos.Add(1, 1);
                LocalPos.Add(2, 2);
                LocalPos.Add(3, 3);
                LocalPos.Add(4, 4);
                break;
            case 2:
                PosToClientPos.Add(2, 1);
                PosToClientPos.Add(3, 2);
                PosToClientPos.Add(4, 3);
                PosToClientPos.Add(1, 4);
                LocalPos.Add(1, 2);
                LocalPos.Add(2, 3);
                LocalPos.Add(3, 4);
                LocalPos.Add(4, 1);
                break;
            case 3:
                PosToClientPos.Add(3, 1);
                PosToClientPos.Add(4, 2);
                PosToClientPos.Add(1, 3);
                PosToClientPos.Add(2, 4);
                LocalPos.Add(1, 3);
                LocalPos.Add(2, 4);
                LocalPos.Add(3, 1);
                LocalPos.Add(4, 2);
                break;
            case 4: // PosToClientPos.Add(1, 4);  PosToClientPos.Add(2, 1); PosToClientPos.Add(3, 2); PosToClientPos.Add(4, 3);
                PosToClientPos.Add(4, 1);
                PosToClientPos.Add(1, 2);
                PosToClientPos.Add(2, 3);
                PosToClientPos.Add(3, 4);
                LocalPos.Add(1, 4);
                LocalPos.Add(2, 1);
                LocalPos.Add(3, 2);
                LocalPos.Add(4, 3);
                break;
            default:
                break;
        }
    }

}

//We are improving the documentation on Custom Defines, 
//and it's important to update these threads with the correct way of using them in the current versions of Unity.
//If you want to modify only global defines, you should use Scripting Define Symbols in Player Settings, because this will cover all the compilers.
//If you choose the .rsp files instead, you'll have to provide one file for every compiler Unity uses, 
//and you won't know when one or another compiler is used. To do this you must add a text file with the extra directives to the "Assets/" folder:

//[td]C#[/td]
//[td]<Project Path>/Assets/smcs.rsp[/td]

//[td]C# - Editor Scripts[/td]
//[td]<Project Path>/Assets/gmcs.rsp[/td]

//[td]UnityScript[/td]
//[td]<Project Path>/Assets/us.rsp[/td]

//[td]Boo[/td]
//[td]<Project Path>/Assets/boo.rsp[/td]


//As an example, if you include the single line "-define:UNITY_DEBUG" 
//in your smcs.rsp file the define UNITY_DEBUG will exist as a global define for C# scripts,
//except for Editor scripts. Every time you make changes to .rsp files you will need to recompile for them to be effective.
//You can do this by updating or reimporting a single script (.js, .cs or .boo) file. All this information is being added to our documentation.
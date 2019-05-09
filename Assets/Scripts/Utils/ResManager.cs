using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    public static ResManager _Ins;
    private string imagePath = "UI/";

    private void Awake()
    {
        _Ins = this;
        Debug.Log($"ResMana Awake");
    }

    public Sprite LoadSpriteAsync(string name)
    {
        ResourceRequest request = Resources.LoadAsync<Sprite>(imagePath + name);
        if (request.asset == null)
            return null;
        return request.asset as Sprite;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <returns>The load.</returns>
    /// <param name="path">Path.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T Load<T>(string path) where T : Object
    {
        if(string.IsNullOrEmpty(path))
        {
            Debug.LogError("path is null");
            return null;
        }
        T Obj;
        Obj = Resources.Load<T>(path);
        return Obj;
    }
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="path">Path.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T LoadAsync<T>(string path) where T : Object
    {
        if(string.IsNullOrEmpty(path))
        {
            Debug.LogError("path is null");
            return null;
        }
        //T Obj  = null;
        var request = Resources.LoadAsync(path);

        return (request.asset as T);
    }

    //IEnumerator LoadAsync(string path)
    //{
    //    ResourceRequest load = Resources.LoadAsync(path);
    //    yield return load;
    //    if(load.isDone)
    //    {
            
    //    }
    //}
}

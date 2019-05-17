using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class Util
{
    public const string AppName = "lsg";               //应用程序名称

    public const string AssetDir = "PackAgerAssetBundle/";           //导出Bundle目录
    //public const string NetUpdateUrl = "http://120.92.78.239/statics/upload/";           //服务器地址目录
    public const string NetUpdateUrl = "file:///D:/TestAssetBundle/";
    public static string updateUrl
    {
        get{
            if (Application.isMobilePlatform)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        return NetUpdateUrl+ "ANDROID/";
                    case RuntimePlatform.IPhonePlayer:
                        return NetUpdateUrl + "IOS/";
                }
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                return NetUpdateUrl + "IOS/";
            }
            return NetUpdateUrl + "PC/";
        }
    }
    public static bool buildBundle = false;
    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + game + "/";
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return Application.dataPath.Substring(0, i + 1) + game + "/";
            }
            return "c:/" + game + "/" + game + "/";
            //return Application.streamingAssetsPath + "/" + game + "/";
        }
    }
    public static string DataRootPath
    {
        get
        {
            string game = AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/";
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return Application.dataPath.Substring(0, i + 1)+ "/";
            }
            return "c:/" + game + "/";
            //return Application.streamingAssetsPath + "/";
        }
    }
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            long me = fs.Length;
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
}
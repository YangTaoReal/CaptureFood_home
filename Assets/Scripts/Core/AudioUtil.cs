using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AudioUtil : MonoBehaviour
{
    public const int LAYER_BACKGROUD = 1;//背景声音
    public const int LAYER_SKILL = 10;
    public const int LAYER_NPC = 11;
    public const int LAYER_FIGHT_RESULT = 12;
    public const int LAYER_UI = 13;//UI声音

    private static Dictionary<int, AudioSource> dictAudioSource = new Dictionary<int, AudioSource>();
    private static Dictionary<string, AudioClip> dictAudioClip = new Dictionary<string, AudioClip>();
    private static Transform transAudioRoot;

    public static void Play(string audioName, bool loop)
    {
        Play(13, "Sounds/" + audioName, loop);
    }
    public static void PlayEscape(string Name, bool loop)
    {
        //if(!GameController.isMuteEffect)
            Play(13, "Sounds/" + Name, loop);
    }
    public static void PlayButton()
    {
        //if (!GameController.isMuteEffect)
            Play(13, "Sounds/Button_Click",false);
    }
    public static void PlayBg(string name, bool loop = true)
    {
        //if(!GameController.isMuteBGM)
        //    Play(LAYER_BACKGROUD, "Sounds/" + name, loop);
        //else
            //Stop(LAYER_BACKGROUD); 
    }

    public static void StopBg()
    {
        Stop(LAYER_BACKGROUD);
    }

    //public  void PlayBgAsync(string name ,bool loop = true)
    //{
    //    if (string.IsNullOrEmpty(name) || name == "0")
    //        return;
    //    if(GameController.isPlayMusic)
    //        StartCoroutine(PlayAsync(LAYER_BACKGROUD,"Sound/"+name, loop));
    //        //Play(LAYER_BACKGROUD, "Sound/" + name, loop);
    //    else
    //        Stop(LAYER_BACKGROUD); 

    //}

    IEnumerator PlayAsync(int layer,string audioName, bool loop)
    {
            
        if(!dictAudioClip.ContainsKey(audioName))
        {
            ResourceRequest re = Resources.LoadAsync<AudioClip>(audioName);
            yield return re;
            if(re.isDone)
            {
                
                dictAudioClip[audioName] = re.asset as AudioClip;
            }
        }
        AudioSource ObjaudioSource = GetAudioSource(layer,loop);
        //if(ObjaudioSource.clip == dictAudioClip[audioName] && ObjaudioSource.isPlaying)
        //{
        //    ObjaudioSource.time = 0;
        //    ObjaudioSource.volume = dictAudioSource[layer].volume;
        //    yield return 0;
        //}
        ObjaudioSource.Stop();
        ObjaudioSource.clip = dictAudioClip[audioName];
        ObjaudioSource.volume = dictAudioSource[layer].volume;
        ObjaudioSource.Play();
    }
   
    public static void Play(int layer, string audioName, bool loop)
    {
        if (string.IsNullOrEmpty(audioName) || audioName == "0")
        {
            return;
        }
        //UnityEngine.Debug.Log("play audio:" + audioName);
        if (!dictAudioClip.ContainsKey(audioName))
        {
            dictAudioClip[audioName] = (AudioClip)Resources.Load(audioName);
        }


        AudioSource ObjaudioSource = GetAudioSource(layer, loop);
        if (ObjaudioSource.clip == dictAudioClip[audioName] && ObjaudioSource.isPlaying)
        {
            ObjaudioSource.time = 0;
            ObjaudioSource.volume = dictAudioSource[layer].volume;
            return;
        }
        if (ObjaudioSource == null)
        {
            Debug.LogError("dictAudioSource[layer] == null");
        }
        ObjaudioSource.Stop();
        ObjaudioSource.clip = dictAudioClip[audioName];
        ObjaudioSource.loop = loop;
        ObjaudioSource.volume = dictAudioSource[layer].volume;
        ObjaudioSource.Play();
    }

    public static AudioSource GetAudioSource(int layer, bool loop)
    {
        if (!dictAudioSource.ContainsKey(layer))
        {
            GameObject go = new GameObject("AudioSource_" + layer);
            GameObject.DontDestroyOnLoad(go);
            dictAudioSource[layer] = go.AddComponent<AudioSource>();
            if (transAudioRoot == null)
            {
                GameObject goRoot = new GameObject("AudioRoot");
                GameObject.DontDestroyOnLoad(goRoot);
                transAudioRoot = goRoot.transform;
            }
            go.transform.parent = transAudioRoot;
            return dictAudioSource[layer];
        }
        if (dictAudioSource[layer].isPlaying)
        {
            if (loop)
            {
                //背景音乐特殊处理
                return dictAudioSource[layer];
            }
            for (int i = 100; i < 2000; i = i + 100)
            {
                int index = layer + i;
                if (!dictAudioSource.ContainsKey(index))
                {
                    GameObject go = new GameObject("AudioSource_" + index);
                    GameObject.DontDestroyOnLoad(go);
                    dictAudioSource[index] = go.AddComponent<AudioSource>();
                    if (transAudioRoot == null)
                    {
                        GameObject goRoot = new GameObject("AudioRoot");
                        GameObject.DontDestroyOnLoad(goRoot);
                        transAudioRoot = goRoot.transform;
                    }
                    go.transform.parent = transAudioRoot;
                    return dictAudioSource[index];
                }
                else if (!dictAudioSource[index].isPlaying)
                {
                    return dictAudioSource[index];
                }
            }
        }
        return dictAudioSource[layer];
    }
    public static void Play(AudioClip audio)
    {
        Play(1, audio, false);
    }
    public static void Play(int layer, AudioClip audio, bool loop)
    {
        if (audio == null)
        {
            return;
        }
        string audioName = audio.name;
        //UnityEngine.Debug.Log("play audio:" + audioName);
        if (!dictAudioClip.ContainsKey(audioName))
        {
            dictAudioClip[audioName] = audio;
        }
        if (!dictAudioSource.ContainsKey(layer))
        {
            GameObject go = new GameObject("AudioSource_" + layer);
            GameObject.DontDestroyOnLoad(go);
            dictAudioSource[layer] = go.AddComponent<AudioSource>();
            if (transAudioRoot == null)
            {
                GameObject goRoot = new GameObject("AudioRoot");
                GameObject.DontDestroyOnLoad(goRoot);
                transAudioRoot = goRoot.transform;
            }
            go.transform.parent = transAudioRoot;
        }
        if (dictAudioSource[layer].clip == dictAudioClip[audioName] && dictAudioSource[layer].isPlaying)
        {
            dictAudioSource[layer].time = 0;
            return;
        }
        dictAudioSource[layer].gameObject.transform.position = Camera.main.transform.position;
        dictAudioSource[layer].Stop();
        dictAudioSource[layer].clip = dictAudioClip[audioName];
        dictAudioSource[layer].loop = loop;
        dictAudioSource[layer].Play();
    }
    public static GameObject GetLayerGameObject(int layer)
    {
        if (dictAudioSource.ContainsKey(layer))
        {
            return dictAudioSource[layer].gameObject;
        }
        return null;
    }
    public static void SetVolume(int layer, float value)
    {
        if (!dictAudioSource.ContainsKey(layer))
        {
            GameObject go = new GameObject("AudioSource_" + layer);
            GameObject.DontDestroyOnLoad(go);
            dictAudioSource[layer] = go.AddComponent<AudioSource>();
            if (transAudioRoot == null)
            {
                GameObject goRoot = new GameObject("AudioRoot");
                GameObject.DontDestroyOnLoad(goRoot);
                transAudioRoot = goRoot.transform;
            }
            go.transform.parent = transAudioRoot;
        }
        dictAudioSource[layer].volume = value;
    }
    public static bool IsPlay(string audioName)
    {
        foreach (AudioSource audioSource in dictAudioSource.Values)
        {
            if (audioSource.isPlaying && dictAudioClip.ContainsKey(audioName) && dictAudioClip[audioName] == audioSource.clip)
            {
                return true;
            }
        }
        return false;
    }
    public static void StopAll()
    {
        foreach (AudioSource audioSource in dictAudioSource.Values)
        {
            audioSource.Stop();
        }
    }
    public static void StopOtherBeiJingEffect()
    {
        Stop(2);
        Stop(3);
    }
    public static void Stop(int layer)
    {
        if (dictAudioSource.ContainsKey(layer) && dictAudioSource[layer] != null)
        {
            dictAudioSource[layer].Stop();
        }
    }
    public static void Stop(int layer, string audioName)
    {
        if (!dictAudioSource.ContainsKey(layer))
        {
            return;
        }
        if (dictAudioSource[layer] != null && dictAudioSource[layer].clip == dictAudioClip[audioName])
        {
            dictAudioSource[layer].Stop();
        }
    }
    private static bool _isAudioOff = false;
    /// <summary>
    /// 是否关闭所有声音
    /// </summary>
    public static bool isAudioOff
    {
        get { return _isAudioOff; }
        set
        {
            _isAudioOff = value;
            AudioListener.volume = value ? 0 : 1;
            PlayerPrefs.SetInt("isAudioOff", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}

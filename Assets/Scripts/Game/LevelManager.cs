using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _Ins;

    private void Awake()
    {
        _Ins = this;
    }

    // 开始关卡
    public void StartGame(int level)
    {

    }

    
}

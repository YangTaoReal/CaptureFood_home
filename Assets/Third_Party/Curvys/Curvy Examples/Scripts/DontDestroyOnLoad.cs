// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

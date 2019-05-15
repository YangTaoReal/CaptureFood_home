﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SWS;
using UnityEditor;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl _Ins;

    public FoodCtrl foodCtrl;
    public PathManager mapCurve;    // 当前地图的曲线

    void Awake()
    {
        _Ins = this;
        InitGame();
    }
    public EventCenter EC = new EventCenter();

    private Camera _uiCamera;
    public Camera UIcamera
    {
        get { 
            if(null == _uiCamera)
            {
                _uiCamera = UICanvas.worldCamera;
            }
            return _uiCamera;
        }
    }
    private Canvas _uiCanvas;
    public Canvas UICanvas
    {
        get
        {
            if(null == _uiCanvas)
            {
                _uiCanvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
            }
            return _uiCanvas;
        }
    }


    private void InitGame()
    {
        DOTween.defaultEaseType = Ease.Linear;
        StartGame();
    }
    void Update()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("start game");
        foodCtrl.StartGame();
        Player._Ins.InitPlayer();
    }

    //public BGCcCursor CreateCursor()
    //{
    //    var cursor = Undo.AddComponent<BGCcCursor>(mapCurve.gameObject);
    //    return cursor;
    //}

    //public BGCcCursorChangeLinear CreateCursorLinear(BGCcCursor cursor)
    //{
    //    var linear = Undo.AddComponent<BGCcCursorChangeLinear>(cursor.gameObject);
        
    //    return linear;
    //}

    //public BGCcCursorObjectTranslate CreateCursorTranslate(BGCcCursor cursor)
    //{
    //    var traslate = Undo.AddComponent<BGCcCursorObjectTranslate>(cursor.gameObject);
    //    return traslate;
    //}
}

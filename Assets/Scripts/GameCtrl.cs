using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl _Ins;

    public FoodCtrl foodCtrl;
    public BGCurve mapCurve;    // 当前地图的曲线

    void Awake()
    {
        _Ins = this;
        InitGame();
    }
    public EventCenter EC = new EventCenter();


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

    public BGCcCursor CreateCursor()
    {
        var cursor = Undo.AddComponent<BGCcCursor>(mapCurve.gameObject);
        return cursor;
    }

    public BGCcCursorChangeLinear CreateCursorLinear(BGCcCursor cursor)
    {
        var linear = Undo.AddComponent<BGCcCursorChangeLinear>(cursor.gameObject);
        
        return linear;
    }

    public BGCcCursorObjectTranslate CreateCursorTranslate(BGCcCursor cursor)
    {
        var traslate = Undo.AddComponent<BGCcCursorObjectTranslate>(cursor.gameObject);
        return traslate;
    }
}

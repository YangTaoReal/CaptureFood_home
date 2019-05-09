using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl _Ins;

    public FoodCtrl foodCtrl;
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
        foodCtrl.CreateOneFood(foodCtrl.GetOneFoodData());
        foodCtrl.StartMoveFoods();
        Player._Ins.InitPlayer();
    }

}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public Text ui_ShowText;
    public Text ui_Title;
    public Text ui_CurrScore;
    public Text ui_Target1;
    public Text ui_Target2;
    public Text ui_Target3;

    public Transform ui_TargetTR;
    public Transform ui_ScoreTR;
    public Transform ui_ConveyorTR;
    public FoodCtrl foodCtrl;

    public Button ui_PauseBtn;
    public Button ui_TestBtn;

   

    private int capturedWrongCount;     // 抓住错误食物的次数
    private int capturedRightCount;     // 抓正确食物的次数
    private int lastFoodId;             // 上一个食物的id 以此来判断是否和上一次食物一样
    private QS_LevelDataData currLevelData;
    private List<int> targetIDList = new List<int>();
    private Dictionary<int, TargetItem> targetItemDic = new Dictionary<int, TargetItem>();
    private int timerID;    // 时间模式倒计时id
    private bool isTimePause;


    private static MainPanel _ins;
    public static MainPanel _Ins
    {
        get
        {
            if (null == _ins)
            {
                _ins = LoadObjAsyc<MainPanel>("Prefabs/UI/MainPanel");
                _ins.PanelName = "MainPanel";
                _ins.InitPro();
            }
            return _ins;
        }
    }

    public override void CloseInit()
    {
        GameCtrl._Ins.EC.OnRefreshCurrDishNum -= OnRefreshDishNum;
        GameCtrl._Ins.EC.OnCheckCaptureFood -= OnCheckCapturedFood;
        //ResetUI();
    }

    public override void CreateInit()
    {
        foodCtrl = transform.Find("Node/Foods").GetComponent<FoodCtrl>();
        InitEvent();
    }

    public override void Notifyed<T>(int id, T data)
    {
    }

    public override void ShowInit()
    {
        GameCtrl._Ins.EC.OnRefreshCurrDishNum += OnRefreshDishNum;
        GameCtrl._Ins.EC.OnCheckCaptureFood += OnCheckCapturedFood;
    }

    public void BeginGame()
    {
        currLevelData = GameCtrl._Ins.GetCurrLevelData();
        ui_ScoreTR.gameObject.SetActive(false);
        if (GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
        {
            ui_Title.text = "剩余盘数";

            RefreshTargetItem(currLevelData.Target);
            ui_ShowText.text = currLevelData.Totalnum.ToString();
        }
        else
        {
            ui_Title.text = "剩余时间";
            ui_ShowText.text = "";


            TransformTimeFormat(GameCtrl._Ins.timer);  // GameCtrl._Ins.timer
            ui_ScoreTR.gameObject.SetActive(true);
            ui_Target1.text = GameCtrl._Ins.scoreGroup[0];
            ui_Target2.text = GameCtrl._Ins.scoreGroup[1];
            ui_Target3.text = GameCtrl._Ins.scoreGroup[2];

            //Debug.Log(timeLevelData);
        }
        foodCtrl.StartGame();
        base.Show();
    }

    private void TransformTimeFormat(int time)
    {
        //DateTime.
        TimerUtil.RemoveTimeOutWithCallBack(timerID);
        TimerUtil.RemoveTimeOut(timerID);

        TimeSpan ts = new TimeSpan(0,0,time);
        TimeSpan timer = new TimeSpan(0,0,1);
        timerID = TimerUtil.SetTimeOut(1f,()=> {
            if (isTimePause)
                return;
            ts = ts.Subtract(timer);
            ui_ShowText.text = ts.ToString(@"mm\:ss");
            //Debug.Log($"当前倒计时:{ts.ToString(@"mm\:ss")}");
            if (ts.TotalSeconds <= 0)
            {
                TimerUtil.RemoveTimeOutWithCallBack(timerID);
                TimerUtil.RemoveTimeOut(timerID);
                Debug.Log($"倒计时结束，游戏结束");
                GameCtrl._Ins.EC.OnGameOver(GameCtrl._Ins.CurrPattern, false);
            }
        },-1);

    }

    private void RefreshTargetItem(string target)
    {
        string[] targets = target.Split('|');
        for (int i = 0; i < targets.Length; i++)
        {
            string[] info = targets[i].Split(',');
            TargetItem item = Instantiate(Resources.Load<TargetItem>("Prefabs/UI/TargetItem"),Vector3.zero,Quaternion.identity,ui_TargetTR);

            //Debug.Log($"0:{info[0]},1:{info[1]}");
            item.InitItem(info[0],info[1]);
            targetItemDic.Add(int.Parse(info[0]),item);
            targetIDList.Add(int.Parse(info[0]));
        }
    }
    #region 观测事件
    private void OnRefreshDishNum(int num)
    {
        //Debug.Log($"到达终点或者被抓住的盘子数量:{num},剩余盘子数量:{currLevelData.Totalnum - num}");
        if(GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
            ui_ShowText.text = currLevelData.Totalnum - num + "";
    }

    private void OnCheckCapturedFood(FoodItem item)
    {
        if (GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
        {
            bool isCapturedWrong = true;    // 是否没有抓到正确的food  连续三次没有抓中的话  触发惩罚机制
            for (int i = 0; i < targetIDList.Count; i++)
            {
                if (item.foodInfo.ID == targetIDList[i])
                {
                    RefreshTargetItem(targetIDList[i]);
                    isCapturedWrong = false;
                    break;
                }
            }
            if (isCapturedWrong == true)
            {
                capturedWrongCount++;
                if (capturedWrongCount == 3)
                {
                    // 触发惩罚机制
                    Debug.Log($"连续抓错食物数量3次,触发惩罚机制");
                    TriggerPunishment();
                    capturedWrongCount = 0;
                }
            }
            else
            {
                // 每次抓对了就将错误次数清零 因为要求是连续三次抓错才触发惩罚
                capturedWrongCount = 0;
            }
        }
        else
        {
            GameCtrl._Ins.AddScore(item.foodInfo.Score);
            ui_CurrScore.text = GameCtrl._Ins.CurrScore.ToString();
            if (lastFoodId == 0)
            {
                lastFoodId = item.foodInfo.ID;
                capturedRightCount++;
            }
            else
            {
                if (lastFoodId == item.foodInfo.ID)
                    capturedRightCount++;
                else
                {
                    capturedRightCount = 1;
                    lastFoodId = item.foodInfo.ID;
                }
            }
            //Debug.Log($"抓住同一食物的次数:{capturedRightCount}");
            if(capturedRightCount == int.Parse(GameCtrl._Ins.GetDisperseData("10003")))
            {
                Debug.Log($"连续{capturedRightCount}次抓住同一食物，触发奖励效果");
                TriggerAward();
                capturedRightCount = 0;
            }
        }
    }
    #endregion

    private void RefreshTargetItem(int foodID)
    {
        TargetItem item;
        if(targetItemDic.TryGetValue(foodID,out item))
        {
            item.RefreshNum();
        }
        bool isWin = true;
        foreach (var target in targetItemDic)
        {
            //Debug.Log($"target.value.isReachGoal:{target.Value.isReachGoal}");
            if(target.Value.isReachGoal == false)
            {
                isWin = false;
                break;
            }
        }
        if (isWin)
        {
            //Debug.Log($"所有的目标都达到了,isWin={isWin}");
            GameCtrl._Ins.InvokeGameOver(true);
        }
    }

    public void ResetData()
    {
        capturedWrongCount = 0;
        foodCtrl.ResetAll();
        currLevelData = null;
        targetIDList.Clear();
        foreach (var item in targetItemDic)
        {
            Destroy(item.Value.gameObject);
        }

        targetItemDic.Clear();
    }

    private void InitEvent()
    {
        ui_PauseBtn.onClick.AddListener(()=> {

            if (GameCtrl._Ins.IsPause == false)
            {
                GameCtrl._Ins.IsPause = true;
                Time.timeScale = 0;
                //ui_PauseBtn.GetComponentInChildren<Text>().text = "取消";
            }
            else
            {
                GameCtrl._Ins.IsPause = false;
                Time.timeScale = 1;
                //ui_PauseBtn.GetComponentInChildren<Text>().text = "暂停";
            }
            //Debug.Log($"是否暂停:{GameCtrl._Ins.IsPause}");
        });

        ui_TestBtn.onClick.AddListener(()=> {

            //TriggerPunishment();
            TriggerAward();
        });
    }

    // 触发惩罚机制
    private void TriggerPunishment()
    {

        foodCtrl.TriggerPunishment(()=> {

            //Debug.Log("惩罚结束的回调");
            foodCtrl.PunishmentOver();
        });

    }

    private void TriggerAward()
    {
        isTimePause = true;
        foodCtrl.TriggerAward(() =>
        {
            foodCtrl.AwardOver();
            isTimePause = false;
        });
    }
}

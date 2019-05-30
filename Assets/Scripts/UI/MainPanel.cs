using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public Text ui_ShowText;
    public Text ui_Title;
    public Transform ui_TargetTR;
    public Transform ui_ConveyorTR;
    public FoodCtrl foodCtrl;

    public Button ui_PauseBtn;
    public Button ui_TestBtn;

    private int capturedWrongCount;     // 抓住错误食物的次数
    private QS_LevelDataData currLevelData;
    private List<int> targetIDList = new List<int>();
    private Dictionary<int, TargetItem> targetItemDic = new Dictionary<int, TargetItem>();


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
        base.Show();
        currLevelData = GameCtrl._Ins.GetCurrLevelData();
        foodCtrl.StartGame();
        if (GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
        {
            ui_Title.text = "剩余盘数";

            RefreshTargetItem(currLevelData.Target);
            ui_ShowText.text = currLevelData.Totalnum.ToString();
        }
        else
        {
            ui_Title.text = "剩余时间";
            //ui_ShowText.text = 
            //TimerUtil.SetTimeOut(1f,);
        }
    }

    private string TransformTimeFormat(int time)
    {
        //DateTime.
        return "";
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
        ui_ShowText.text = currLevelData.Totalnum - num + "";
    }

    private void OnCheckCapturedFood(FoodItem item)
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
        if(isCapturedWrong == true)
        {
            capturedWrongCount++;
            if(capturedWrongCount == 3)
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
                ui_PauseBtn.GetComponentInChildren<Text>().text = "取消";
            }
            else
            {
                GameCtrl._Ins.IsPause = false;
                Time.timeScale = 1;
                ui_PauseBtn.GetComponentInChildren<Text>().text = "暂停";
            }
            //Debug.Log($"是否暂停:{GameCtrl._Ins.IsPause}");
        });

        ui_TestBtn.onClick.AddListener(()=> {

            TriggerPunishment();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public Text ui_ChallengeNum;
    public Transform ui_TargetTR;
    public Transform ui_ConveyorTR;
    public FoodCtrl foodCtrl;

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
        RefreshTargetItem(currLevelData.Target);
        ui_ChallengeNum.text = "0/" + currLevelData.Totalnum;
        foodCtrl.StartGame();
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
        //Debug.Log($"刷新盘子数量:{num}");
        ui_ChallengeNum.text = num + "/" + currLevelData.Totalnum;
    }

    private void OnCheckCapturedFood(FoodItem item)
    {
        for (int i = 0; i < targetIDList.Count; i++)
        {
            if (item.foodInfo.ID == targetIDList[i])
            {
                RefreshTargetItem(targetIDList[i]);
                break;
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
            GameCtrl._Ins.GameOver(true);
        }
    }

    public void ResetUI()
    {
        foodCtrl.ResetAll();
        currLevelData = null;
        targetIDList.Clear();
        foreach (var item in targetItemDic)
        {
            Destroy(item.Value.gameObject);
        }
        targetItemDic.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public Text ui_ChallengeNum;

    public FoodCtrl foodCtrl;
   
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
    }

    public void BeginGame()
    {
        base.Show();
        foodCtrl.StartGame();
    }
    #region 观测事件
    private void OnRefreshDishNum(int num)
    {
        //Debug.Log($"刷新盘子数量:{num}");
        ui_ChallengeNum.text = num + "/100";
    }
    #endregion
}

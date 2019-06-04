using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
   
    public Button ui_ChallengeBtn;
    public Button ui_TimeBtn;

    private static StartPanel _ins;
    public static StartPanel _Ins
    {
        get
        {
            if (null == _ins)
            {
                _ins = LoadObjAsyc<StartPanel>("Prefabs/UI/StartPanel");
                _ins.PanelName = "StartPanel";
                _ins.InitPro();
            }
            return _ins;
        }
    }

    public override void CloseInit()
    {
    }

    public override void CreateInit()
    {
        InitEvent();
    }

    public override void Notifyed<T>(int id, T data)
    {
    }

    public override void ShowInit()
    {
    }

    private void InitEvent()
    {
        ui_ChallengeBtn.onClick.AddListener(()=> {
            Debug.Log("开始挑战模式");
            SelectPanel._Ins.ShowPanel(GamePattern.Challenge);
            Close();
        });

        ui_TimeBtn.onClick.AddListener(()=> {

            SelectPanel._Ins.ShowPanel(GamePattern.Time);
            Close();
        });
    }
}

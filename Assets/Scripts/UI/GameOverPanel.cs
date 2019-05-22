using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{

    public Button ui_NextLevelBtn;
    public Button ui_BackToMainBtn;
    public Button ui_ShareBtn;

    private static GameOverPanel _ins;
    public static GameOverPanel _Ins
    {
        get
        {
            if (null == _ins)
            {
                _ins = LoadObjAsyc<GameOverPanel>("Prefabs/UI/GameOverPanel");
                _ins.PanelName = "GameOverPanel";
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

    }
}

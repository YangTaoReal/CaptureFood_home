using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : BasePanel
{
    public Transform ui_ChallengePattern;
    public Transform ui_TimePattern;

    public Button ui_ChinaBtn;
    public Button ui_JapanBtn;
    public Button ui_FranceBtn;
    public Button ui_EasyBtn;
    public Button ui_MediumBtn;
    public Button ui_DifficultBtn;
    public Button ui_BackBtn;


    private static SelectPanel _ins;
    public static SelectPanel _Ins
    {
        get
        {
            if (null == _ins)
            {
                _ins = LoadObjAsyc<SelectPanel>("Prefabs/UI/SelectPanel");
                _ins.PanelName = "SelectPanel";
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
        ui_BackBtn.onClick.AddListener(()=> {

            Close();
            StartPanel._Ins.Show();
        });
        ui_EasyBtn.onClick.AddListener(()=> {

            GameCtrl._Ins.ReadTimePatternData("10004");
            GameCtrl._Ins.StartGame(GamePattern.Time);
            Close();
        });

        ui_MediumBtn.onClick.AddListener(()=> {
            GameCtrl._Ins.ReadTimePatternData("10005");
            GameCtrl._Ins.StartGame(GamePattern.Time);
            Close();
        });

        ui_DifficultBtn.onClick.AddListener(() =>
        {
            GameCtrl._Ins.ReadTimePatternData("10006");
            GameCtrl._Ins.StartGame(GamePattern.Time);
            Close();
        });

        ui_ChinaBtn.onClick.AddListener(()=> {

            GameCtrl._Ins.StartGame(GamePattern.Challenge);
            Close();
        });

        ui_JapanBtn.onClick.AddListener(() => {

            GameCtrl._Ins.StartGame(GamePattern.Challenge);
            Close();
        });

        ui_FranceBtn.onClick.AddListener(() => {

            GameCtrl._Ins.StartGame(GamePattern.Challenge);
            Close();
        });
    }

    public void ShowPanel(GamePattern pattern)
    {
        ui_ChallengePattern.gameObject.SetActive(false);
        ui_TimePattern.gameObject.SetActive(false);
        if(pattern == GamePattern.Challenge)
        {
            ui_ChallengePattern.gameObject.SetActive(true);
        }
        else
        {
            ui_TimePattern.gameObject.SetActive(true);
        }

        base.Show();
    }
}

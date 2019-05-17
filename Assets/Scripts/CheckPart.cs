using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPart : MonoBehaviour
{
    private float timer = 0.1f;
    private float curTime;
    private bool hasChecked;
    private bool isNeedCheck;

    private void Start()
    {
        //Debug.Log("check part start");
        GameCtrl._Ins.EC.OnHandComeBackOver += OnPlayerComeBackOver;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isNeedCheck)
            return;
        if(collision.gameObject.tag.Equals("Food"))
        {
            GameCtrl._Ins.EC.OnCaptureFood?.Invoke(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isNeedCheck)
            return;
        if (collision.gameObject.tag.Equals("Food"))
        {
            GameCtrl._Ins.EC.OnCaptureFood?.Invoke(collision.gameObject);
        }
    }

    private void Update()
    {
        if(Player._Ins.isBacking && !hasChecked)
        {
            isNeedCheck = true;
            curTime += Time.deltaTime;
            if(curTime > timer)
            {
                isNeedCheck = false;
                hasChecked = true;
                curTime = 0;
            }
        }
    }

    private void OnPlayerComeBackOver()
    {
        hasChecked = false;
    }

}

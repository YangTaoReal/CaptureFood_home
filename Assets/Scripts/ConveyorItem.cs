using System.Collections;
using System.Collections.Generic;
using SWS;
using UnityEngine;
using UnityEngine.Events;

public class ConveyorItem : MonoBehaviour
{
    public splineMove splineMove;
    public bool isUsing;

    public void InitItem()
    {
        splineMove.pathContainer = GameCtrl._Ins.mapCurve;
        //splineMove.speed = GameCtrl._Ins.GetCurrLevelData().Movespeed;
        splineMove.StartMove();     // 事件必须在 startMove() 调用了在添加
        //Debug.Log($"曲线事件长度 = {splineMove.events.Count}");
        UnityEvent myEvent = splineMove.events[splineMove.events.Count - 1];
        myEvent.RemoveAllListeners();
        myEvent.AddListener(() => {

            //Debug.Log($"履带到达曲线终点,pointIndex = {splineMove.events.Count - 1}");
            ResetItem();
            //GameCtrl._Ins.EC.OnFoodArriveEndPoint?.Invoke(this);
        });
    }

    public void RefreshItem()
    {
        isUsing = true;
        gameObject.SetActive(true);
        splineMove.speed = GameCtrl._Ins.GetCurrLevelData().Movespeed;
        splineMove.StartMove();
    }

    public void ResetItem()
    {
        isUsing = false;
        splineMove.speed = 0;
        gameObject.SetActive(false);
    }
}

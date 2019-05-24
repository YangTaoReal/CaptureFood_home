using System.Collections;
using System.Collections.Generic;
using SWS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour
{
    [SerializeField]
    public QS_FoodItemData foodInfo;

    public bool isUsing;
    //public bool isMoving;
    public splineMove splineMove;
    public BoxCollider2D boxcollider;
    public Image foodImg;
    public float speed = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void InitItem()
    {
        splineMove = GetComponent<splineMove>();
        if (null == splineMove)
            splineMove = gameObject.AddComponent<splineMove>();
        boxcollider = GetComponent<BoxCollider2D>();
        foodImg = GetComponent<Image>();
        splineMove.pathContainer = GameCtrl._Ins.mapCurve;
        splineMove.speed = GameCtrl._Ins.GetCurrLevelData().Movespeed;
        splineMove.pathMode = DG.Tweening.PathMode.TopDown2D;
        splineMove.loopType = splineMove.LoopType.loop;
        //Debug.Log($"{transform.name} = {transform.position},vector[0]:{splineMove.pathContainer.GetPathPoints()[0]}");
        //transform.position = splineMove.pathContainer.GetPathPoints()[0];
        //Debug.Log($"{transform.name} = {transform.position},vector[0]:{splineMove.pathContainer.GetPathPoints()[0]}");
        splineMove.StartMove();     // 事件必须在 startMove() 调用了在添加
        //Debug.Log($"曲线事件长度 = {splineMove.events.Count}");
        UnityEvent myEvent = splineMove.events[splineMove.events.Count - 1];
        myEvent.RemoveAllListeners();
        myEvent.AddListener(()=> {

            //Debug.Log($"到达曲线终点,pointIndex = {splineMove.events.Count - 1}");
            ResetItem();
            GameCtrl._Ins.EC.OnFoodArriveEndPoint?.Invoke(this);
        });

        
    }

    public void RefreshItem(QS_FoodItemData data)
    {
        foodImg.sprite = Resources.Load<Sprite>("UI/" + data.ID);
        isUsing = true;
        foodInfo = data;
        gameObject.SetActive(true);
        splineMove.enabled = true;
        boxcollider.enabled = true;
        splineMove.speed = GameCtrl._Ins.GetCurrLevelData().Movespeed;   //data.Movespeed;
        splineMove.StartMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("BornTrigger"))
        {
            GameCtrl._Ins.EC.OnTriggerBornFodd?.Invoke();
        }
    }

    public void ResetItem()
    {
        isUsing = false;
        gameObject.SetActive(false);
        boxcollider.enabled = false;
        splineMove.speed = 0;
        transform.SetParent(MainPanel._Ins.foodCtrl.transform);
        foodInfo.FOODSTATE = FoodState.Free;

    }

    public void OnCaptured()
    {
        // 被抓住
        foodInfo.FOODSTATE = FoodState.Captured;
        splineMove.Stop();
        splineMove.enabled = false;
        boxcollider.enabled = false;
        //splineMove.speed = 0;
        //splineMove.Stop();
    }

    #region   检测事件

    #endregion
}

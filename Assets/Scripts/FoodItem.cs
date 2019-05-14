using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [SerializeField]
    public FoodData foodInfo;

    public bool isUsing;
    public bool isMoving;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void InitItem()
    {
        // 创建出来的item 需要创建cursor之间的关联
        //cursor = GameCtrl._Ins.CreateCursor();
        //var list = GameCtrl._Ins.mapCurve.GetComponents<BGCcCursor>();
        //cursorTranslate = GameCtrl._Ins.CreateCursorTranslate(list[list.Length - 1]);
        //cursorLinear = GameCtrl._Ins.CreateCursorLinear(list[list.Length - 1]);
        //cursorTranslate.SetParent(cursor);
        //cursorLinear.SetParent(cursor);
        ////cursorLinear.OverflowControl = BGCcCursorChangeLinear.OverflowControlEnum.Stop;
        //cursorTranslate.ObjectToManipulate = transform;
        //cursorLinear.PointReached += (sender, e) => {

        //    if(e.PointIndex == GameCtrl._Ins.mapCurve.PointsCount - 1)
        //    {
        //        //Debug.Log($"到达点的index = {e.PointIndex}");
        //        cursorLinear.Speed = 0;
        //        ResetItem();
        //    }
        //};
    }
    public void RefreshItem(FoodData data)
    {
        isUsing = true;
        foodInfo = data;
        gameObject.SetActive(true);
        //cursorLinear.Speed = 5;
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
        isMoving = false;
    }

    public void OnCaptured()
    {
        // 被抓住
        foodInfo.state = FoodState.Captured;
        
    }
}

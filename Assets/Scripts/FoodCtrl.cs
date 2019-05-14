using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using DG.Tweening;
using BansheeGz;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using UnityEditor;

public class FoodCtrl : MonoBehaviour
{
    public Transform bornTR;
    public Transform resetTR;
    public Transform bornTriggerTR;

    public float minDis;

    public List<FoodItem> foodList = new List<FoodItem>();

    private Vector3 currPos;
    private bool isCanBorn;
    public bool isCanCreateFood;

    private void OnEnable()
    {
        GameCtrl._Ins.EC.OnTriggerBornFodd += OnTriggerBorn;
    }

    private void OnDisable()
    {
        GameCtrl._Ins.EC.OnTriggerBornFodd -= OnTriggerBorn;

    }

    public void InitFoodCtrl()
    {

    }

    public FoodItem CreateOneFood(FoodData foodData)
    {
        bool isFind = false;
        FoodItem food = null;
        for (int i = 0; i < foodList.Count; i++)
        {
            if(foodList[i].isUsing == false)
            {
                food = foodList[i];
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            var requet = Resources.LoadAsync("Foods/Food_" + foodData.foodID);
            var obj = Instantiate(requet.asset, bornTR.position, Quaternion.identity, transform) as GameObject;food = obj.GetComponent<FoodItem>();
            //Debug.Log("bornPos:{bornTR.position},recttran:{(bornTR as RectTransform).anchoredPosition}");
            if (null == food)
                food = obj.AddComponent<FoodItem>();
            food.InitItem();
            foodList.Add(food);
        }
        (food.transform as RectTransform).anchoredPosition = (bornTR as RectTransform).anchoredPosition;
        food.RefreshItem(foodData);
        //var cursorTr = mapCurve.GetComponent<BGCcCursorObjectTranslate>();
        //var linner = mapCurve.GetComponent<BGCcCursorChangeLinear>();
        //cursorTr.ObjectToManipulate = food.transform;

        //mapCurve
        //var cursor = mapCurve.GetComponent<BGCcCursor>();
        //var und1 = Undo.AddComponent<BGCcCursorObjectTranslate>(mapCurve.gameObject);
        //var und2 = Undo.AddComponent<BGCcCursorObjectTranslate>(mapCurve.gameObject);
        //var list = mapCurve.GetComponents<BGCcCursorObjectTranslate>();
        //Debug.Log("list.Count = {list.Length},und.name = {und1}");
        return food;
    }

    public FoodData GetOneFoodData()
    {
        FoodData food = new FoodData();
        food.foodID = Random.Range(1000,1005);
        food.moveSpeed = 3f;
        food.state = FoodState.Free;
        return food;
    }

    public void StopFoods()
    {

    }

    public void StartMoveFoods()
    {

    }

    // 检测是否超过了边界
    public void CheckIfReset()
    {
        
    }

    private void Update()
    {
        //if(isCanMove)
        //{
        //    for (int i = 0; i < foodList.Count; i++)
        //    {
        //        FoodItem item = foodList[i];
        //        //if (!item.isUsing)
        //            //return;
        //        if (item.foodInfo.state == FoodState.Free)
        //        {
        //            //item.isMoving = true; 
        //            Vector3 curr = foodList[i].transform.position;

        //            float t = Vector3.Distance(curr, resetTR.position) / item.foodInfo.moveSpeed;
        //            //Debug.Log("时间t:{t}");
        //            //item.transform.DOMove(resetTR.position, t).SetEase(Ease.Linear).OnComplete(() =>
        //            //{
        //            //    item.ResetItem(bornTR.position);
        //            //});
        //            item.transform.position = Vector3.MoveTowards(curr, resetTR.position, Time.deltaTime * foodList[i].foodInfo.moveSpeed);
        //            if(item.transform.position == resetTR.position)
        //            {
        //                item.ResetItem(bornTR.position);
        //            }
        //        }
        //    }
        //}
    }

    #region  订阅事件相关
    private void OnTriggerBorn()
    {
        //Debug.Log("生成food");
        //CreateOneFood(GetOneFoodData());

    }


    #endregion

    public void StartGame()
    {
        //currMapSpline = GameObject.FindWithTag("Map").transform.GetChild(0).GetComponent<CurvySpline>();
        //Debug.Log("CurrMapSpline = {CurrMapSpline}");
        CreateOneFood(GetOneFoodData());
        //StartMoveFoods();
        TimerUtil.SetTimeOut(1f,()=> {
            CreateOneFood(GetOneFoodData());
        },-1);
        isCanBorn = true;
        //StartCoroutine(StartBornFood());
    }

    // 开始每隔一段时间生成一个食物，食物沿着曲线运动
    IEnumerator StartBornFood()
    {
        CreateOneFood(GetOneFoodData());
        while(isCanBorn)
        {
            yield return new WaitForSeconds(1f);
            CreateOneFood(GetOneFoodData());
        }
        yield return null;
    }
}

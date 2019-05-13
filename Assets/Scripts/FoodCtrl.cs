using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FoodCtrl : MonoBehaviour
{
    public Transform bornTR;
    public Transform resetTR;
    public Transform bornTriggerTR;

    public float minDis;

    public List<FoodItem> foodList = new List<FoodItem>();

    private Vector3 currPos;
    private bool isCanMove;
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

    public void CreateOneFood(FoodData foodData)
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
            //Debug.Log($"bornPos:{bornTR.position},recttran:{(bornTR as RectTransform).anchoredPosition}");
            if (null == food)
                food = obj.AddComponent<FoodItem>();
            foodList.Add(food);
        }
        (food.transform as RectTransform).anchoredPosition = (bornTR as RectTransform).anchoredPosition;
        food.InitItem(foodData);
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
        isCanMove = false;
    }

    public void StartMoveFoods()
    {
        isCanMove = true;
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
        //            //Debug.Log($"时间t:{t}");
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
        //Debug.Log($"生成food");
        CreateOneFood(GetOneFoodData());

    }


    #endregion

    public void StartGame()
    {
        //currMapSpline = GameObject.FindWithTag("Map").transform.GetChild(0).GetComponent<CurvySpline>();
        //Debug.Log($"CurrMapSpline = {CurrMapSpline}");
        CreateOneFood(GetOneFoodData());
        //StartMoveFoods();
    }
}

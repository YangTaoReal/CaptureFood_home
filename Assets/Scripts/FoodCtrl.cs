﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 生成食物
/// </summary>
public class FoodCtrl : MonoBehaviour
{
    public Transform bornTR;
    //public Transform resetTR;
    //public Transform bornTriggerTR;

    public float minDis;

    public List<FoodItem> foodList = new List<FoodItem>();
    public bool isCanCreateFood;

    private List<QS_FoodItemData> foodDatas = new List<QS_FoodItemData>(); 
    private Vector3 currPos;
    private int foodDataIndex = 0;
    private bool isCanBorn;
    private int timerID;

    private List<int> targetList = new List<int>();
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

    public FoodItem CreateOneFood(QS_FoodItemData foodData)
    {
        if (null == foodData)
            return null;
        bool isFind = false;
        FoodItem food = null;
        for (int i = 0; i < foodList.Count; i++)
        {
            if(foodList[i].isUsing == false && foodList[i].foodInfo.FOODSTATE == FoodState.Free)
            {
                food = foodList[i];
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            var requet = Resources.Load("Prefabs/Foods/FoodItem");
            var obj = Instantiate(requet, bornTR.position, Quaternion.identity, transform) as GameObject;
            food = obj.GetComponent<FoodItem>();
            //Debug.Log("bornPos:{bornTR.position},recttran:{(bornTR as RectTransform).anchoredPosition}");
            if (null == food)
                food = obj.AddComponent<FoodItem>();
            food.InitItem();
            foodList.Add(food);
        }
        food.RefreshItem(foodData);
        return food;
    }

    public QS_FoodItemData GetOneFoodData()
    {
        if (foodDataIndex > foodDatas.Count - 1)
        {
            //Debug.LogError($"挑战模式的数据已经用完了");
            return null;
        }
        QS_FoodItemData food = foodDatas[foodDataIndex];
        foodDataIndex++;

        //food.foodID = Random.Range(1001,1006);
        //food.moveSpeed = 3f;
        //food.state = FoodState.Free;
        return food;
    }

    public void StopFoods()
    {

    }

    public void StartMoveFoods()
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

    private void OnCheckCaptureFood(FoodItem item)
    {

    }

    #endregion
    /// <summary>
    /// 根据游戏类型 生成食物
    /// </summary>
    public void StartGame()
    {
        // 挑战模式的话 需要将tartget食物和普通的食物进行洗牌算法
        if(GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
        {
            // 将需要的数据填充进FoodDataList中，在洗乱
            targetList.Clear();
            RiffleFoodData();
            CreateOneFood(GetOneFoodData());
            timerID = TimerUtil.SetTimeOut(1f, () => {
                CreateOneFood(GetOneFoodData());
            }, -1);
        }
        else  // 时间模式
        {

        }
        //currMapSpline = GameObject.FindWithTag("Map").transform.GetChild(0).GetComponent<CurvySpline>();
        //Debug.Log("CurrMapSpline = {CurrMapSpline}");
        //CreateOneFood(GetOneFoodData());
        //TimerUtil.SetTimeOut(1f,()=> {
        //    CreateOneFood(GetOneFoodData());
        //},-1);

        //StartCoroutine(StartBornFood());
    }

    /// <summary>
    /// 开始洗牌操作 将数据随机打乱
    /// </summary>
    private void RiffleFoodData()
    {
        foodDatas.Clear();
        var LevelData = GameCtrl._Ins.GetCurrLevelData();
        //string[] target = LevelData.Target.Split('|');
        //for (int i = 0; i < target.Length; i++)
        //{
        //    string[] goal = target[i].Split(',');
        //    int id = int.Parse(goal[0]);
        //    int num = int.Parse(goal[1]);
        //    AddDataToFoodDatas(id, num);
        //}
        string[] pool = LevelData.Pool.Split('|');
        for (int i = 0; i < pool.Length; i++)
        {
            string[] goal = pool[i].Split(',');
            int id = int.Parse(goal[0]);
            int num = int.Parse(goal[1]);
            AddDataToFoodDatas(id, num);
        }

        System.Random random = new System.Random();
        // 洗牌
        int index;
        QS_FoodItemData iTmp;
        // 洗两次 想洗几次都可以
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < foodDatas.Count; i++)
            {
                index = Random.Range(0, foodDatas.Count);
                iTmp = foodDatas[i];
                foodDatas[i] = foodDatas[index];
                foodDatas[index] = iTmp;
            }
        }

        //Debug.Log($"===============洗牌后====表中数据个数:{foodDatas.Count}======================");
        //for (int i = 0; i < foodDatas.Count; i++)
        //{
        //    Debug.Log($"第{i + 1}个食物:{foodDatas[i].ID}");
        //}
    }

    private void AddDataToFoodDatas(int foodID,int num)
    {
        var data = GameCtrl._Ins.GetFoodItemData(foodID);
        if(data != null)
        {
            for (int i = 0; i < num; i++)
            {
                foodDatas.Add(data);
            }
        }
    }

    public void ResetAll()
    {
        TimerUtil.RemoveTimeOutWithCallBack(timerID);
        TimerUtil.RemoveTimeOut(timerID);
        for (int i = 0; i < foodList.Count; i++)
        {
            foodList[i].ResetItem();

        }
    }
}

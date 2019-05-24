using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

/// <summary>
/// 生成食物
/// </summary>
public class FoodCtrl : MonoBehaviour
{
    public Transform bornTR;
    public Transform ui_ConveyorTR;
    //public Transform resetTR;
    //public Transform bornTriggerTR;

    public float minDis;

    public List<FoodItem> foodList = new List<FoodItem>();
    public List<ConveyorItem> conveyorList = new List<ConveyorItem>();

    private List<QS_FoodItemData> foodDatas = new List<QS_FoodItemData>(); 
    private Vector3 currPos;
    private int foodDataIndex = 0;
    //private bool isCanBorn;
    private int foodTimerID;
    private int conveyorTimerID;
    private List<int> targetList = new List<int>();

    private void OnEnable()
    {
        GameCtrl._Ins.EC.OnTriggerBornFodd += OnTriggerBorn;
        //GameCtrl._Ins.EC.OnResetGameData += OnResetGameData;
    }

    private void OnDisable()
    {
        GameCtrl._Ins.EC.OnTriggerBornFodd -= OnTriggerBorn;
        //GameCtrl._Ins.EC.OnResetGameData -= OnResetGameData;

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
        GameCtrl._Ins.CurrFoodNum++;
        return food;
    }

    public QS_FoodItemData GetOneFoodData()
    {
        if (foodDataIndex > foodDatas.Count - 1)
        {
            GameCtrl._Ins.IsFoodDataRunOut = true;
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

        // 开始移动履带
        StartBornConveyor();

        // 挑战模式的话 需要将tartget食物和普通的食物进行洗牌算法
        if (GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
        {
            // 将需要的数据填充进FoodDataList中，在洗乱
            targetList.Clear();
            RiffleFoodData();
            StartBornFood();


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
                index = UnityEngine.Random.Range(0, foodDatas.Count);
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
        StopBornFood();
        StopBornConveyor();
        for (int i = 0; i < foodList.Count; i++)
        {
            foodList[i].ResetItem();
        }
        // 回收所有的履带
        for (int i = 0; i < conveyorList.Count; i++)
        {
            conveyorList[i].ResetItem();
        }
        // 清空数据
        foodDatas.Clear();
        foodDataIndex = 0;
        targetList.Clear();
    }

    public void StartBornFood()
    {
        CreateOneFood(GetOneFoodData());
        foodTimerID = TimerUtil.SetTimeOut(GameCtrl._Ins.CurrLevelData.Foodrate, () => {
            CreateOneFood(GetOneFoodData());
        }, -1);
    }

    public void StopBornFood()
    {
        TimerUtil.RemoveTimeOutWithCallBack(foodTimerID);
        TimerUtil.RemoveTimeOut(foodTimerID);
    }

    public void StopBornConveyor()
    {
        TimerUtil.RemoveTimeOutWithCallBack(conveyorTimerID);
        TimerUtil.RemoveTimeOut(conveyorTimerID);
    }

    private void CreateConveyor()
    {
        bool isFind = false;
        ConveyorItem item = null;
        for (int i = 0; i < conveyorList.Count; i++)
        {
            if (!conveyorList[i].isUsing)
            {
                item = conveyorList[i];
                isFind = true;
                break;
            }
        }

        if (!isFind)
        {

            var co = Resources.Load<ConveyorItem>("Prefabs/Foods/ConveyorItem");
            item = Instantiate(co, Vector3.zero, Quaternion.identity, ui_ConveyorTR);
            item.InitItem();
            conveyorList.Add(item);
        }

        item.RefreshItem();

    }

    public void StartBornConveyor()
    {
        CreateConveyor();
        conveyorTimerID = TimerUtil.SetTimeOut(GameCtrl._Ins.CurrLevelData.Conveyorrate, () => {

            CreateConveyor();
        }, -1);
    }

    public void TriggerPunishment(Action CallBack = null)
    {
        StopBornFood();
        StopBornConveyor();
        for (int i = 0; i < foodList.Count; i++)
        {
            if(foodList[i].isUsing)
                foodList[i].splineMove.Pause();
        }
        for (int i = 0; i < conveyorList.Count; i++)
        {
            conveyorList[i].splineMove.Pause();
        }
        // 开始效果
        List<FoodItem> punishList = new List<FoodItem>();
        for (int i = 0; i < foodList.Count; i++)
        {
            if(GameCtrl._Ins.CheckIfInView(foodList[i].transform.position))
            {
                //foodList[i].GetComponent<Image>().color = Color.red;
                punishList.Add(foodList[i]);

            }
        }
        if(punishList.Count == 0)
        {
            Debug.Log($"punishList.Count = 0,直接return了");
            CallBack?.Invoke();
            return;
        }

        Vector2 pos1,pos2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, new Vector2(Screen.width / 2, Screen.height / 2),
            GameCtrl._Ins.UIcamera, out pos1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, new Vector2(Screen.width, Screen.height),
            GameCtrl._Ins.UIcamera, out pos2);
        for (int i = 0; i < punishList.Count; i++)
        {
            punishList[i].boxcollider.enabled = false;
            punishList[i].transform.DOLocalMove(pos1, 0.5f);
        }
        TimerUtil.SetTimeOut(0.5f,()=> {

            for (int i = 0; i < punishList.Count; i++)
            {
                var item = punishList[i];

                int index = i;
                TimerUtil.SetTimeOut(i * 0.2f,()=> {
                    if(index != punishList.Count - 1)
                    {
                        item.transform.DOLocalMove(pos2, 0.5f).OnComplete(() => {

                            item.ResetItem();
                            Debug.Log($"惩罚废除了dishArriveNum++");
                            GameCtrl._Ins.DishArriveNum++;
                            GameCtrl._Ins.EC.OnRefreshCurrDishNum?.Invoke(GameCtrl._Ins.DishArriveNum);
                        });
                    }
                    else
                    {
                        // 最后一个加上回调
                        item.transform.DOLocalMove(pos2, 0.5f).OnComplete(() => {

                            item.ResetItem();
                            Debug.Log($"惩罚废除了dishArriveNum++");
                            GameCtrl._Ins.DishArriveNum++;
                            GameCtrl._Ins.EC.OnRefreshCurrDishNum?.Invoke(GameCtrl._Ins.DishArriveNum);
                            CallBack?.Invoke();
                        });
                    }
                });
            }
        });
    }

    public void PunishmentOver()
    {
        StartBornFood();
        StartBornConveyor();
        for (int i = 0; i < foodList.Count; i++)
        {
            if (foodList[i].isUsing)
                foodList[i].splineMove.Resume();
        }
        for (int i = 0; i < conveyorList.Count; i++)
        {
            if (conveyorList[i].isUsing)
                conveyorList[i].splineMove.Resume();
        }
    }
}

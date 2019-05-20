using System.Collections;
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

    private List<FoodData> foodDatas = new List<FoodData>(); 
    private Vector3 currPos;
    private bool isCanBorn;

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
            if(foodList[i].isUsing == false && foodList[i].foodInfo.state == FoodState.Free)
            {
                food = foodList[i];
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            var requet = Resources.Load("Prefabs/Foods/FoodItem");
            var obj = Instantiate(requet, bornTR.position, Quaternion.identity, transform) as GameObject;food = obj.GetComponent<FoodItem>();
            //Debug.Log("bornPos:{bornTR.position},recttran:{(bornTR as RectTransform).anchoredPosition}");
            if (null == food)
                food = obj.AddComponent<FoodItem>();
            food.InitItem();
            foodList.Add(food);
        }
        food.RefreshItem(foodData);
        return food;
    }

    public FoodData GetOneFoodData()
    {
        FoodData food = new FoodData();
        food.foodID = Random.Range(1001,1006);
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
    /// <summary>
    /// 根据游戏类型 生成食物
    /// </summary>
    public void StartGame()
    {
        // 挑战模式的话 需要将tartget食物和普通的食物进行洗牌算法
        if(GameCtrl._Ins.CurrPattern == GamePattern.Challenge)
        {
            // 将需要的数据填充进FoodDataList中，在洗乱
            RiffleFoodData();
        }
        else  // 时间模式
        {

        }
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

    /// <summary>
    /// 开始洗牌操作 将数据随机打乱
    /// </summary>
    private void RiffleFoodData()
    {
        List<int> datas = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            datas.Add(i + 1);
        }
        System.Random random = new System.Random();
        // 洗牌
        int index;
        int iTmp;
        for (int i = 0; i < datas.Count; i++)
        {
            index  = Random.Range(0, datas.Count);
            iTmp = datas[i];
            datas[i] = datas[index];
            datas[index] = iTmp;
        }
        Debug.Log("===============洗牌后==========================");
        for (int i = 0; i < datas.Count; i++)
        {
            Debug.Log($"第{i + 1}张牌:{datas[i]}");
        }
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

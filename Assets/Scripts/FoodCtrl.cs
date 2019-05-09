using System.Collections;
using System.Collections.Generic;
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

    public void CreateOneFood(FoodData foodData)
    {
        var requet = Resources.LoadAsync("Foods/Food_" + foodData.foodID);
        var obj = Instantiate(requet.asset,bornTR.position,Quaternion.identity,transform) as GameObject;
        var food = obj.GetComponent<FoodItem>();
        if (null == food)
            food = obj.AddComponent<FoodItem>();
        food.InitItem(foodData);
        foodList.Add(food);
    }

    public FoodData GetOneFoodData()
    {
        FoodData food = new FoodData();
        food.foodID = Random.Range(1000,1005);
        food.moveSpeed = 2f;
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
        if(isCanMove)
        {
            for (int i = 0; i < foodList.Count; i++)
            {
                Vector3 curr = foodList[i].transform.position;
                foodList[i].transform.position = Vector3.MoveTowards(curr, resetTR.position, Time.deltaTime * foodList[i].foodInfo.moveSpeed);
            }
        }
    }

    #region  订阅事件相关
    private void OnTriggerBorn()
    {
        CreateOneFood(GetOneFoodData());
    }


    #endregion
}

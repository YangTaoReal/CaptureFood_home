using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public FoodData foodInfo;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitItem(FoodData data)
    {
        foodInfo = data;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("BornTrigger"))
        {
            Debug.Log($"触发生成");
            GameCtrl._Ins.EC.OnTriggerBornFodd?.Invoke();
        }
    }
}

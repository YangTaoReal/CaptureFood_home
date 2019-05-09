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

    public void InitItem(FoodData data)
    {
        isUsing = true;
        foodInfo = data;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("BornTrigger"))
        {
            GameCtrl._Ins.EC.OnTriggerBornFodd?.Invoke();
        }
    }

    public void ResetItem(Vector3 pos)
    {
        isUsing = false;
        gameObject.SetActive(false);
        transform.position = pos;
        isMoving = false;
    }

    public void OnCaptured()
    {
        // 被抓住
        foodInfo.state = FoodState.Captured;
        
    }
}

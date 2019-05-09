using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player _Ins;

    public Image handImg;
    public Image checkPart;

    public float upSpeed = 10;
    public float downSpeed = 20;
    public bool isCaptured;     // 已经抓住物体
    private bool isPress;
    private Vector2 sizeData;
    // Start is called before the first frame update
    private void Awake()
    {
        _Ins = this;
    }
    void Start()
    {
        sizeData = handImg.rectTransform.sizeDelta;
    }

    public void InitPlayer()
    {
        InitGameEvent();
    }
    private void InitGameEvent()
    {
        GameCtrl._Ins.EC.OnCaptureFood += OnCapturedFood;

    }

    private void OnDisable()
    {
        GameCtrl._Ins.EC.OnCaptureFood -= OnCapturedFood;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }
    void Update()
    {
        if (isPress && !isCaptured)
        {
            OnPress();
            GameCtrl._Ins.EC.OnPress?.Invoke();
        }

    }

    public void OnPointDown()
    {
        //Debug.Log($"按下.....");
        isPress = true;
    }

    public void OnPointUp()
    {
        isPress = false;
        ComeBack();
    }

    public void OnPress()
    {
        //Debug.Log($"press中");
        //if (!isCaptured)
            //return;
        var size = handImg.rectTransform.sizeDelta;
        handImg.rectTransform.sizeDelta = new Vector2(size.x, size.y +  upSpeed);

    }

    public void ComeBack(Action CallBack = null)
    {
        float t = (handImg.rectTransform.sizeDelta.y - sizeData.y) / downSpeed;
        //Debug.Log($"结束press,time:{t}");
        handImg.rectTransform.DOSizeDelta(sizeData, t).OnComplete(()=> {

            isCaptured = false;
            CallBack?.Invoke();
        });
    }

    #region   订阅事件

    private void OnCapturedFood(GameObject obj)
    {
        if (!isCaptured)
        {
            var item =obj.GetComponent<FoodItem>();
            Debug.Log($"抓住了obj:{obj.name}");
            isCaptured = true;
            isPress = false;
            item.transform.SetParent(checkPart.transform);
            item.foodInfo.state = FoodState.Captured;
            ComeBack(()=> {

                Destroy(item.gameObject);
            });
        }

    }

    #endregion

}

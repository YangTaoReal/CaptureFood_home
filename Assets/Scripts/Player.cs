using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public static Player _Ins;

    public Image handImg;
    public Image checkImg;
    public Image playerImg;
    public Transform captureTR;

    public float upSpeed = 10;
    public float downSpeed = 20;
    public bool isCaptured;     // 已经抓住物体
    public bool isBacking;
    private bool isPress;
    private bool isDraging;
    private Vector2 sizeData;
    private Vector2 offset;
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
        if (isPress && !isBacking && !isDraging)
        {
            OnPress();
            GameCtrl._Ins.EC.OnPress?.Invoke();
        }

    }

    public void OnPointDown(BaseEventData eventData)
    {
        //Debug.Log("按下.....");
        isPress = true;
    }

    public void OnPointUp(BaseEventData eventData)
    {
        isPress = false;
        ComeBack();
    }

    public void OnPress()
    {
        //Debug.Log("press中");
        //if (!isCaptured)
            //return;
        var size = handImg.rectTransform.sizeDelta;
        handImg.rectTransform.sizeDelta = new Vector2(size.x, size.y +  Time.deltaTime * upSpeed);

    }

    public void ComeBack(Action CallBack = null)
    {
        isBacking = true;
        float t = (handImg.rectTransform.sizeDelta.y - sizeData.y) / downSpeed;
        //Debug.Log("结束press,time:{t}");
        handImg.rectTransform.DOSizeDelta(sizeData, t).OnComplete(()=> {

            isCaptured = false;
            isPress = false;
            isBacking = false;
            CallBack?.Invoke();
            GameCtrl._Ins.EC.OnHandComeBackOver?.Invoke();
        });
    }

    #region   订阅事件

    private void OnCapturedFood(GameObject obj)
    {
        if (isPress)
            return;
        if (!isCaptured)
        {
            var item =obj.GetComponent<FoodItem>();
            //Debug.Log($"抓住了obj:{obj.name}");
            isCaptured = true;
            item.OnCaptured();
            item.transform.SetParent(checkImg.transform);
            item.transform.localPosition  = captureTR.localPosition;
            //Vector2 screenPos = GameCtrl._Ins.UIcamera.WorldToScreenPoint(captureTR.position);
            //Vector2 pos;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(checkPart.rectTransform, screenPos,
            //    GameCtrl._Ins.UIcamera, out pos);
            //(item.transform as RectTransform).anchoredPosition = pos;
            ComeBack(()=> {

                item.ResetItem();
                GameCtrl._Ins.EC.OnCheckCaptureFood?.Invoke(item);
                //Destroy(item.gameObject);
            });
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //offset = (Vector3)eventData.position - 
        offset = Vector2.zero;
        isDraging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameCtrl._Ins.IsPause == true)
            return;
        //Debug.Log($"delta:{eventData.delta},scrollDelta:{eventData.scrollDelta}");
        //transform.localPosition = new Vector3(transform.localPosition.x + eventData.delta.x, transform.localPosition.y, transform.localPosition.z);
        Vector2 mousePos = eventData.position;
        Vector2 uguiPos = new Vector2();
        var parent = transform.parent as RectTransform;
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, mousePos, eventData.enterEventCamera, out uguiPos);
        if(isRect)
        {
            var pos1 = playerImg.rectTransform.anchoredPosition;
            playerImg.rectTransform.anchoredPosition = new Vector2((uguiPos + offset).x, pos1.y);
            var pos2 = playerImg.transform.position;
            //RectTransformUtility.
            var screen1 = RectTransformUtility.WorldToScreenPoint(GameCtrl._Ins.UIcamera, pos2);
            var screen2 = RectTransformUtility.WorldToScreenPoint(GameCtrl._Ins.UIcamera, pos2);
            //Debug.Log($"screen1:{screen1},,screen2:{screen2}");
            if (screen1.x < 0)
            {
                playerImg.rectTransform.anchoredPosition = pos1;
            }
            if(screen2.x > Screen.width)
            {
                playerImg.rectTransform.anchoredPosition = pos1;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 mouseDown = eventData.position; //记录鼠标按下时的屏幕坐标 
        Vector2 mouseUguiPos = new Vector2(); //定义一个接收返回的ugui坐标 
        //RectTransformUtility.ScreenPointToLocalPointInRectangle()：把屏幕坐标转化成ugui坐标 
        //canvas：坐标要转换到哪一个物体上，这里img父类是Canvas，我们就用Canvas 
        //eventData.enterEventCamera：这个事件是由哪个摄像机执行的 
        //out mouseUguiPos：返回转换后的ugui坐标 
        //isRect：方法返回一个bool值，判断鼠标按下的点是否在要转换的物体上 
        var parent = transform.parent as RectTransform;
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, mouseDown, eventData.enterEventCamera, out mouseUguiPos); 
        if (isRect) //如果在 
        { 
            //计算图片中心和鼠标点的差值
            offset = transform.GetComponent<Image>().rectTransform.anchoredPosition - mouseUguiPos; 
        }
        isDraging = true;
    }

    #endregion

}

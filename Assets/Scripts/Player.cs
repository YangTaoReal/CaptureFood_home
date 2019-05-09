using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Image handImg;
    public float upSpeed = 10;
    public float downSpeed = 20;

    private bool isPress;
    private Vector2 sizeData;
    // Start is called before the first frame update
    void Start()
    {
        sizeData = handImg.rectTransform.sizeDelta;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }
    void Update()
    {
        if (isPress)
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
        float t = (handImg.rectTransform.sizeDelta.y - sizeData.y) / downSpeed;
        //Debug.Log($"结束press,time:{t}");
        handImg.rectTransform.DOSizeDelta(sizeData, t);
    }

    public void OnPress()
    {
        //Debug.Log($"press中");
        var size = handImg.rectTransform.sizeDelta;
        handImg.rectTransform.sizeDelta = new Vector2(size.x, size.y +  upSpeed);

    }

}

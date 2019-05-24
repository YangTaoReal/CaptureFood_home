using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetItem : MonoBehaviour
{

    public Image targetImg;
    public Text targetText;
    public bool isReachGoal = false;   // 当所有的目标都达到 挑战成功

    private int currNum = 0;
    private int targetNum;
    public void InitItem(string id,string num)
    {
        targetImg.sprite = Resources.Load<Sprite>("UI/" + id);
        targetNum = int.Parse(num);
        targetText.text = currNum + "/" + targetNum;
    }

    public void RefreshNum()
    {
        currNum++;
        //Debug.Log($"刷新target数量:currNum = {currNum}");
        if(currNum >= targetNum)
        {
            isReachGoal = true;
        }

        targetText.text = currNum + "/" + targetNum;
    }


}

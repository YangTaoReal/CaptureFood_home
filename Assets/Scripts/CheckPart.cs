using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPart : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Food"))
        {
            GameCtrl._Ins.EC.OnCaptureFood?.Invoke(collision.gameObject);
        }
    }
}

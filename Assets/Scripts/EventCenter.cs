﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    public Action OnPress = () => { };
    public Action OnTriggerBornFodd = () => { };
    public Action<GameObject> OnCaptureFood = (obj)=>{};
    public Action OnHandComeBackOver = () => { };
    public Action<FoodItem> OnFoodArriveEndPoint = (food) => { };
    public Action<int> OnRefreshCurrDishNum = (dishNum) => { };
}

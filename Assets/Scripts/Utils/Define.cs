﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

}

public class FoodData
{
    public int foodID;
    public float moveSpeed;
    public FoodState state;
}

public enum FoodState
{
    Free,
    Captured,   // 被抓住
}

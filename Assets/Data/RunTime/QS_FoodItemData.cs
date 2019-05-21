using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class QS_FoodItemData
{
  [SerializeField]
  int id;
  public int ID { get {return id; } set { id = value;} }
  
  [SerializeField]
  int score;
  public int Score { get {return score; } set { score = value;} }
  
  [SerializeField]
  float movespeed;
  public float Movespeed { get {return movespeed; } set { movespeed = value;} }
  
  [SerializeField]
  FoodState foodstate;
  public FoodState FOODSTATE { get {return foodstate; } set { foodstate = value;} }
  
}
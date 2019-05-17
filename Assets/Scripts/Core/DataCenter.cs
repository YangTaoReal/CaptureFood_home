using UnityEngine;
using System.Collections;

public class DataCenter
{
    private int _bestScore;
    public int BestScore
    {
        get { return PlayerPrefs.GetInt("BestScore",0); }
        set { 
        
            _bestScore = value;
            PlayerPrefs.SetInt("BestScore",_bestScore);
        }
    }

    private int _currScore;
    public int CurrScore
    {
        get { return _currScore; }
        set
        {
            _currScore = value;
        }
    }


}

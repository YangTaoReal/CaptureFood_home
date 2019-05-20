using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SWS;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl _Ins;

    public int dishArriveNum;
    public GameObject mapObj;
    public PathManager mapCurve;    // 当前地图的曲线
    public SpriteShapeController mapShape;
    public QS_LevelData levelData;

    private int _currLevel = 0;   // 当前关卡
    public int CurrLevel
    {
        get { return _currLevel;}
        set
        {
            _currLevel = value;
        }
    }

    private GamePattern _currPattern;
    public GamePattern CurrPattern
    {
        get { return _currPattern; }
        set
        {
            _currPattern = value;
        }
    }
    void Awake()
    {
        _Ins = this;
        StartPanel._Ins.Show();
        InitGame();
    }
    public EventCenter EC = new EventCenter();

    private Camera _uiCamera;
    public Camera UIcamera
    {
        get { 
            if(null == _uiCamera)
            {
                _uiCamera = UICanvas.worldCamera;
            }
            return _uiCamera;
        }
    }
    private Canvas _uiCanvas;
    public Canvas UICanvas
    {
        get
        {
            if(null == _uiCanvas)
            {
                _uiCanvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
            }
            return _uiCanvas;
        }
    }


    private void InitGame()
    {
        DOTween.defaultEaseType = Ease.Linear;
        GameCtrl._Ins.EC.OnFoodArriveEndPoint += OnOneFoodArriveEnd;
        //StartGame();
    }
    void Update()
    {
        
    }

    public void StartGame(GamePattern pattern)
    {
        Debug.Log("start game");
        CurrPattern = pattern;
        CreatePathBySpriteShape();
        MainPanel._Ins.BeginGame();
        Player._Ins.InitPlayer();
        //mapCurve.Create()
    }

    public void CreatePathBySpriteShape()
    {
        //GameObject newPath = new GameObject("Path7 (Runtime Creation)");
        mapCurve = mapObj.AddComponent<PathManager>();
        List<Vector3> pointList = new List<Vector3>();
        for (int i = 0; i < mapShape.spline.GetPointCount() - 1; i++)
        {
            pointList.Add(mapShape.spline.GetPosition(i));
        }

        Transform[] wayPoints = new Transform[pointList.Count];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            GameObject obj = new GameObject("WayPoint" + i);
            wayPoints[i] = obj.transform;
            wayPoints[i].position = pointList[i];
        }
        mapCurve.Create(wayPoints, true);
        //mapCurve.gameObject.AddComponent<PathRenderer>();
        //mapCurve.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        Debug.Log($"生成结束");
    }

    //public BGCcCursor CreateCursor()
    //{
    //    var cursor = Undo.AddComponent<BGCcCursor>(mapCurve.gameObject);
    //    return cursor;
    //}

    //public BGCcCursorChangeLinear CreateCursorLinear(BGCcCursor cursor)
    //{
    //    var linear = Undo.AddComponent<BGCcCursorChangeLinear>(cursor.gameObject);

    //    return linear;
    //}

    //public BGCcCursorObjectTranslate CreateCursorTranslate(BGCcCursor cursor)
    //{
    //    var traslate = Undo.AddComponent<BGCcCursorObjectTranslate>(cursor.gameObject);
    //    return traslate;
    //}

    #region   观测事件
    private void OnOneFoodArriveEnd(FoodItem item)
    {
        //Debug.Log("盘子到达终点");
        dishArriveNum++;
        if(dishArriveNum == 100)
        {
            Debug.Log("game over");
        }
        EC.OnRefreshCurrDishNum?.Invoke(dishArriveNum);
    }

    #endregion
}

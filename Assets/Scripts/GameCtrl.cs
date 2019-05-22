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
    public EventCenter EC = new EventCenter();
    public int dishArriveNum;
    //public GameObject mapObj;

    public BezierPathManager mapCurve;    // 当前地图的曲线
    public SpriteShapeController mapShape;
    public QS_LevelData QS_LevelDatas;
    public QS_FoodItem QS_FoodItemDatas;

    private bool _isAllFoodArrive;
    public bool IsAllFoodArrive
    {
        get { return _isAllFoodArrive; }
        set { _isAllFoodArrive = value; }
    }

    private bool _isFoodDataRunOut;  // 盘子是否全部生成完毕
    public bool IsFoodDataRunOut
    {
        get { return _isFoodDataRunOut; }
        set { _isFoodDataRunOut = value; }
    }

    private QS_LevelDataData _currLevelData;
    public QS_LevelDataData CurrLevelData
    {
        get { return _currLevelData; }
        set { _currLevelData = value; }
    }

    private int _currFoodNum;
    public int CurrFoodNum
    {
        get { return _currLevel; }
        set {
            _currFoodNum = value;
        }
    }

    private int _currLevel = 0;   // 当前关卡
    public int CurrLevel
    {
        get { return _currLevel;}
        set
        {
            _currLevel = value;
            //PlayerPrefs.SetInt("CurrLevel",value);
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
        AdjustScreenSize();
        StartPanel._Ins.Show();
        InitGame();
    }

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
        CurrLevel = PlayerPrefs.GetInt("CurrLevel",1);
        CurrLevelData = GetCurrLevelData();
        CurrPattern = pattern;
        CreatePathBySpriteShape();
        MainPanel._Ins.BeginGame();
        Player._Ins.InitPlayer();
    }

    public void CreatePathBySpriteShape()
    {
        //GameObject newPath = new GameObject("Path7 (Runtime Creation)");
        //mapCurve = mapObj.AddComponent<PathManager>();
        mapCurve = mapShape.GetComponentInChildren<BezierPathManager>();
        List<Vector3> pointList = new List<Vector3>();
        //List<Vector3> leftList = new List<Vector3>();
        //List<Vector3> rightList = new List<Vector3>();
        for (int i = 0; i < mapShape.spline.GetPointCount() - 1; i++)
        {
            pointList.Add(mapShape.spline.GetPosition(i));
            //leftList.Add(mapShape.spline.GetLeftTangent(i));
            //rightList.Add(mapShape.spline.GetLeftTangent(i));
        }
        //BezierPoint[] beziers = new BezierPoint[pointList.Count];
        Transform[] wayPoints = new Transform[pointList.Count];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            GameObject obj = new GameObject("WayPoint" + i);
            //GameObject left = new GameObject("left" + i);
            //GameObject right = new GameObject("right" + i);
            //left.transform.SetParent(obj.transform);
            //right.transform.SetParent(obj.transform);
            wayPoints[i] = obj.transform;
            wayPoints[i].position = pointList[i];

            //wayPoints[i] = obj.transform;
            //wayPoints[i].position = pointList[i];
            //var l = wayPoints[i].GetChild(0);
            //l = left.transform;
            //wayPoints[i].GetChild(0).transform.position = leftList[i];
            //var r =wayPoints[i].GetChild(1);
            //r = right.transform;
            //wayPoints[i].GetChild(1).transform.position = rightList[i];
        }
        //mapCurve.Create(wayPoints, true);
        //mapCurve.gameObject.AddComponent<PathRenderer>();
        //mapCurve.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        //Debug.Log($"生成结束");
    }

    #region   观测事件
    private void OnOneFoodArriveEnd(FoodItem item)
    {
        //Debug.Log("盘子到达终点");
        dishArriveNum++;
        if(IsFoodDataRunOut )
        {
            bool isAllArrive = true;
            for (int i = 0; i < MainPanel._Ins.foodCtrl.foodList.Count; i++)
            {
                if (MainPanel._Ins.foodCtrl.foodList[i].isUsing)
                {
                    isAllArrive = false; 
                    break;
                }
            }
            if(isAllArrive)
                IsAllFoodArrive = true;
        }
        if (IsFoodDataRunOut && IsAllFoodArrive)
        {
            Debug.Log("game over");
            MainPanel._Ins.Close();

            GameOver(false);
        }
        EC.OnRefreshCurrDishNum?.Invoke(dishArriveNum);
    }

    #endregion

    public QS_LevelDataData GetCurrLevelData()
    {
        return QS_LevelDatas.dataArray[CurrLevel - 1];
    }

    public QS_FoodItemData GetFoodItemData(int FoodID)
    {
        for (int i = 0; i < QS_FoodItemDatas.dataArray.Length; i++)
        {
            if (QS_FoodItemDatas.dataArray[i].ID == FoodID)
                return QS_FoodItemDatas.dataArray[i];
        }
        return null;
    }

    private void OnChallengeWin()
    {
        Debug.Log($"恭喜挑战胜利");
        MainPanel._Ins.ResetUI();
        //StartPanel._Ins.Show();

    }

    public void AdjustScreenSize()
    {
        // 开发的标准尺寸
        const float devHeight = 19.2f;
        const float devWidth = 10.8f;

        float screenHeight = Screen.height;
        float orthographicSize = Camera.main.orthographicSize;

        float aspectRatio = Screen.width * 1.0f / Screen.height;    // 实际的宽高比
        // 实际的宽高比和摄像机的orthographicSize计算得出摄像机的宽度值
        float cameraWidth = orthographicSize * 2 * aspectRatio;
        Debug.Log($"适配屏幕cameraWidth:{cameraWidth}");
        if(cameraWidth < devWidth)  // 如果摄像机宽度 小于设计的尺寸宽度
        {
            orthographicSize = devWidth / (2 * aspectRatio);
            Camera.main.orthographicSize = orthographicSize;
            UIcamera.orthographicSize = orthographicSize;
        }
    }

    public void GameOver(bool isWin)
    {
        EC.OnGameOver?.Invoke(CurrPattern,isWin);
    }
}

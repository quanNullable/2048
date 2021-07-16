using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
  //   [HideInInspector]
  public Text score;

  //   [HideInInspector]
  public Text best;
  public Transform gridParent;
  private Dictionary<int, int> gridConf = new Dictionary<int, int>(){
      {4,100},{5,80},{6,65}
  };
  public GameObject gridPrefab;
  public GameObject numPrefab;

  private List<MyGrid> canCreateNumGrids = new List<MyGrid>();

  private int row;
  private int col;

  private Vector3 pointerDownPos, pointerUpPos;

  private MyGrid[][] myGrids = null;

  private bool needCreateNumber = false;

  private int currentScore, bestScore;
  private Button btnLast;

  private HistoryData lastStepData;
  public void OnLastStepClick()
  {
    BackToLastStep();
    btnLast.interactable = false;
  }

  public void OnBackClick()
  {
    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);

  }
  public void OnReStartClick()
  {
    btnLast.interactable = false;
    ResetScore();
    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        if (myGrids[i][j].HasNumber())
        {
          GameObject.Destroy(myGrids[i][j].GetNumber().gameObject);
          myGrids[i][j].SetNumber(null);
        }

      }
    }
    CreateNumber();
  }

  public void InitGrid()
  {
    int gridNum = PlayerPrefs.GetInt(Consts.GameModel, 4);
    GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
    gridLayoutGroup.constraintCount = gridNum;
    gridLayoutGroup.cellSize = new Vector2(gridConf[gridNum], gridConf[gridNum]);

    row = gridNum;
    col = gridNum;
    myGrids = new MyGrid[gridNum][];
    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        if (myGrids[i] == null)
        {
          myGrids[i] = new MyGrid[gridNum];
        }
        myGrids[i][j] = CreateGrid();
      }
    }
  }

  private MyGrid CreateGrid()
  {
    GameObject grid = GameObject.Instantiate(gridPrefab, gridParent);
    return grid.GetComponent<MyGrid>();
  }

  private void CreateNumber()
  {
    canCreateNumGrids.Clear();
    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        MyGrid grid = myGrids[i][j];
        if (!grid.HasNumber())
        {
          canCreateNumGrids.Add(grid);
        }
      }
    }
    if (canCreateNumGrids.Count == 0)
    {
      return;
    }
    int index = Random.Range(0, canCreateNumGrids.Count);
    MyGrid myGrid = canCreateNumGrids[index];
    CreateNumber(myGrid, 0);
  }
  private void CreateNumber(MyGrid grid, int number)
  {
    GameObject num = GameObject.Instantiate(numPrefab, grid.transform);
    num.GetComponent<Number>().Init(grid);
    if (number != 0)
    {
      num.GetComponent<Number>().SetNumber(number);
    }
  }
  private void Awake()
  {
    InitPanelMessage();
    UpdateBest(bestScore);
    InitGrid();
    CreateNumber();
  }

  private void Start() {
    AudioManager._instance.PlayMusic();
  }
  private void InitPanelMessage()
  {
    bestScore = PlayerPrefs.GetInt(Consts.BestScore, 0);
    lastStepData = new HistoryData();
    btnLast = transform.Find("Btn_Last").GetComponent<Button>();
    btnLast.onClick.AddListener(this.OnLastStepClick);
    btnLast.interactable = false;
  }

  public void OnPointerDown()
  {
    pointerDownPos = Input.mousePosition;
  }
  public void OnPointerUp()
  {
    pointerUpPos = Input.mousePosition;
    if (Vector3.Distance(pointerUpPos, pointerDownPos) < 100)
    {
      Debug.Log("无效的移动,忽略!");
      return;
    }

    lastStepData.UpdateData(currentScore, bestScore, myGrids);
    btnLast.interactable = true;

    MoveType moveType = CaculateMoveType();
    MoveNumber(moveType);
    if (needCreateNumber)
    {
      CreateNumber();
    }
    ResetNumberStatus();
    needCreateNumber = false;
    if (CheckIfLose())
    {
      GameLose();
    }
  }

  private MoveType CaculateMoveType()
  {
    MoveType type;
    if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerUpPos.y - pointerDownPos.y))
    {
      if (pointerUpPos.x - pointerDownPos.x > 0)//向右移动
      {
        type = MoveType.Right;
      }
      else
      {
        type = MoveType.Left;
      }
    }
    else
    {
      if (pointerUpPos.y - pointerDownPos.y > 0)//向上移动
      {
        type = MoveType.Up;
      }
      else
      {
        type = MoveType.Down;

      }
    }
    return type;
  }

  private void MoveNumber(MoveType moveType)
  {
    switch (moveType)
    {
      case MoveType.Left:
        for (int i = 0; i < row; i++)
        {
          for (int j = 1; j < col; j++)
          {
            MyGrid grid = myGrids[i][j];
            if (grid.HasNumber())
            {
              Number number = grid.GetNumber();
              for (int m = j - 1; m >= 0; m--)//和左面的格子比较
              {
                if (this.HandleNumber(number, myGrids[i][m]))//有数字
                {
                  break;
                }
              }
            }

          }
        }

        break;
      case MoveType.Right:
        for (int i = 0; i < row; i++)
        {
          for (int j = col - 2; j >= 0; j--)
          {
            MyGrid grid = myGrids[i][j];
            if (grid.HasNumber())
            {
              Number number = grid.GetNumber();
              for (int m = j + 1; m < col; m++)//和右面的格子比较
              {
                if (this.HandleNumber(number, myGrids[i][m]))//有数字
                {
                  break;
                }
              }
            }

          }
        }
        break;
      case MoveType.Up:
        for (int j = 0; j < col; j++)
        {
          for (int i = 1; i < row; i++)
          {
            MyGrid grid = myGrids[i][j];
            if (grid.HasNumber())
            {
              Number number = grid.GetNumber();
              for (int m = i - 1; m >= 0; m--)//和上面的格子比较
              {
                if (this.HandleNumber(number, myGrids[m][j]))//有数字
                {
                  break;
                }
              }
            }

          }
        }
        break;
      case MoveType.Down:
        for (int j = 0; j < col; j++)
        {
          for (int i = row - 2; i >= 0; i--)
          {
            MyGrid grid = myGrids[i][j];
            if (grid.HasNumber())
            {
              Number number = grid.GetNumber();
              for (int m = i + 1; m < row; m++)//和下面的格子比较
              {
                if (this.HandleNumber(number, myGrids[m][j]))//有数字
                {
                  break;
                }
              }
            }

          }
        }
        break;
      default:
        break;
    }
  }


  private bool HandleNumber(Number current, MyGrid grid)//处理数字
  {
    if (grid.HasNumber())//有数字
    {
      if (current.CanMerge(grid.GetNumber()))
      {//能合并
        current.DestroyOnMoveEnd(grid);
        grid.GetNumber().Merge();
        needCreateNumber = true;
      }
      return true;
    }
    else
    {//无数字 移过去
      current.MoveToGrid(grid);
      needCreateNumber = true;
    }
    return false;
  }

  private void ResetNumberStatus()
  {//重置所有数字状态为可合并
    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        MyGrid grid = myGrids[i][j];
        if (grid.HasNumber())
        {
          grid.GetNumber().status = NumberStatus.Normal;
        }
      }
    }
  }



  public bool CheckIfLose()
  {
    if (canCreateNumGrids.Count > 1)
    {
      return false;
    }
    for (int i = 0; i < row; i += 2)
    {
      for (int j = 0; j < col; j++)
      {
        MyGrid up = GetGridByIndex(i - 1, j);
        MyGrid down = GetGridByIndex(i + 1, j);
        MyGrid left = GetGridByIndex(i, j - 1);
        MyGrid right = GetGridByIndex(i, j + 1);
        MyGrid current = myGrids[i][j];
        if (up != null && current.GetNumber().CanMerge(up.GetNumber()))
        {
          return false;
        }
        if (down != null && current.GetNumber().CanMerge(down.GetNumber()))
        {
          return false;
        }
        if (left != null && current.GetNumber().CanMerge(left.GetNumber()))
        {
          return false;
        }
        if (right != null && current.GetNumber().CanMerge(right.GetNumber()))
        {
          return false;
        }
      }
    }
    return true;
  }

  public void UpdateScore(int score)
  {
    this.score.text = score.ToString();
  }

  public void UpdateBest(int score)
  {
    this.best.text = score.ToString();
  }


  public void AddScore(int score)
  {
    currentScore += score;
    this.UpdateScore(currentScore);
    if (currentScore > bestScore)
    {
      bestScore = currentScore;
      PlayerPrefs.SetInt(Consts.BestScore, bestScore);
      this.UpdateBest(bestScore);
    }
  }

  private void ResetScore()
  {
    currentScore = 0;
    this.UpdateScore(currentScore);
  }

  private MyGrid GetGridByIndex(int i, int j)
  {
    if (i >= 0 && i < row && j >= 0 && j < col)
    {
      return myGrids[i][j];
    }
    return null;
  }

  public void BackToLastStep()
  {
    currentScore = lastStepData.score;
    UpdateScore(currentScore);

    if (bestScore != lastStepData.best)
    {
      bestScore = lastStepData.best;
      UpdateBest(bestScore);
      PlayerPrefs.SetInt(Consts.BestScore, bestScore);
    }
    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        if (lastStepData.numbers[i][j] == 0)
        {
          if (myGrids[i][j].HasNumber())
          {
            GameObject.Destroy(myGrids[i][j].GetNumber().gameObject, 0.2f);
            myGrids[i][j].SetNumber(null);
          }

        }
        else
        {
          if (myGrids[i][j].HasNumber())//修改数字
          {
            myGrids[i][j].GetNumber().SetNumber(lastStepData.numbers[i][j]);
          }
          else
          {//创建数字
            CreateNumber(myGrids[i][j], lastStepData.numbers[i][j]);
          }
        }

      }
    }
  }
  public void GameWin()
  {
    GameObject.Find("Canvas").transform.Find("WinPanel").GetComponent<WinPanel>().Show(true);
  }

  public void GameLose()
  {
    GameObject.Find("Canvas").transform.Find("LosePanel").GetComponent<LosePanel>().Show(true);

  }

  #region 
  //test
  #endregion
}

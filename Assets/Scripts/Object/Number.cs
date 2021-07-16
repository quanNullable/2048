using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
  private Image bg;
  private Text text;
  private MyGrid inGrid;//所在的格子
  public NumberStatus status;
  private float spawnScaleTime;
  private float mergeScaleTime;
  private float mergeScaleBackTime;
  private float moveTime;
  private Vector3 moveStartPos;
  public Color[] bgColors;
  public List<int> numValues;

  private void Awake()
  {
    bg = transform.GetComponent<Image>();
    text = transform.Find("Text").GetComponent<Text>();
  }

  public void Init(MyGrid grid)
  {
    grid.SetNumber(this);
    this.SetGrid(grid);
    this.SetNumber(numValues[0]);
    status = NumberStatus.Normal;
    this.PlaySpawnAnim();
  }

  public void SetGrid(MyGrid grid)
  {
    this.inGrid = grid;
  }

  public MyGrid GetGrid()
  {
    return inGrid;
  }

  public void SetNumber(int num)
  {
    this.text.text = num.ToString();
    this.bg.color = this.bgColors[numValues.IndexOf(num)];
  }

  public int GetNumber()
  {
    return int.Parse(this.text.text);
  }

  public void MoveToGrid(MyGrid grid)
  {
    transform.SetParent(grid.transform);
    // transform.localPosition = Vector3.zero;
    moveStartPos = transform.localPosition;
    PlayMoveAnim();
    this.GetGrid().SetNumber(null);
    grid.SetNumber(this);
    this.SetGrid(grid);
  }

  public void Merge()
  {
    this.CheckIfWin();
    this.status = NumberStatus.Merged;
    this.PlayMergeAnim();
    AudioManager._instance.PlaySound();
  }


  public void CheckIfWin()
  {
    GamePanel gamePanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
    gamePanel.AddScore(this.GetNumber());
    int number = this.GetNumber() * 2;
    this.SetNumber(number);
    if (number == numValues[numValues.Count - 1])
    {
      gamePanel.GameWin();
    }
  }

  public void DestroyOnMoveEnd(MyGrid grid)
  {
    transform.SetParent(grid.transform);
    moveStartPos = transform.localPosition;
    PlayMoveAnim();
    GameObject.Destroy(this.gameObject, 0.2f);
    this.GetGrid().SetNumber(null);
  }

  public bool CanMerge(Number number)
  {
    return status == NumberStatus.Normal && this.GetNumber() == number.GetNumber();
  }

  private void Update()
  {
    if (spawnScaleTime <= 1)//创建时变大
    {
      spawnScaleTime += Time.deltaTime * 4;
      transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnScaleTime);
    }
    else if (moveTime <= 1)
    { //移动动画
      moveTime += Time.deltaTime * 4;
      transform.localPosition = Vector3.Lerp(moveStartPos, Vector3.one, moveTime);
    }
    else
    {
      if (mergeScaleTime <= 1 && mergeScaleBackTime == 0)//合并时先变大
      {
        mergeScaleTime += Time.deltaTime * 4;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.2f, mergeScaleTime);
      }
      else if (mergeScaleTime >= 1 && mergeScaleBackTime <= 1)//合并时再变小
      {
        mergeScaleBackTime += Time.deltaTime * 4;
        transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, mergeScaleBackTime);
      }
    }


  }

  public void PlaySpawnAnim()
  {
    spawnScaleTime = 0;
    mergeScaleBackTime = 1;
    mergeScaleTime = 1;
    moveTime = 1;
  }
  public void PlayMergeAnim()
  {
    mergeScaleTime = 0;
    mergeScaleBackTime = 0;
    spawnScaleTime = 1;
    moveTime = 1;
  }

  public void PlayMoveAnim()
  {
    mergeScaleTime = 1;
    mergeScaleBackTime = 1;
    spawnScaleTime = 1;
    moveTime = 0;

  }
}

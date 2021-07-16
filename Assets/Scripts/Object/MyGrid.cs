using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
  private Number number;
  public bool HasNumber()
  {
    return number != null;
  }

  public Number GetNumber()
  {
    return number;
  }

  public void SetNumber(Number num)
  {
    this.number = num;
  }
}

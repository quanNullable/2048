using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModelPanel : BaseView
{
  public void OnModelSelectClick(int count)
  {
    PlayerPrefs.SetInt(Consts.GameModel, count);
    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
  }


}

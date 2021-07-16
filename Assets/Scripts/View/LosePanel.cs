using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : BaseView
{

  public void OnReStartClick()
  {
    GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().OnReStartClick();
    Show(false);
  }

  public void OnBackClick()
  {
    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
  }
}

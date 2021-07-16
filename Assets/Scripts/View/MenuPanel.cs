using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{

  public void OnStartClick()
  {
    GameObject.Find("Canvas").transform.Find("SelectModelPanel").GetComponent<SelectModelPanel>().Show(true);
  }

  public void OnSetClick()
  {
    GameObject.Find("Canvas").transform.Find("SetPanel").GetComponent<SetPanel>().Show(true);
  }


  public void OnExitClick()
  {
    // if (Application.isEditor)
    // {
    //   UnityEditor.EditorApplication.isPlaying = false;
    // }
    // else
    // {
    //   Application.Quit();
    // }


    //unity 预处理指令
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
  }

}

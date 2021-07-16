using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPanel : BaseView
{
  public void OnCloseClick()
  {
    Show(false);
  }

  public void OnSoundValueChange(float value)
  {
    AudioManager._instance.OnSoundVolumeChange(value / 100);
    PlayerPrefs.SetFloat(Consts.SoundValue, value);
  }

  public void OnMusicValueChange(float value)
  {
    AudioManager._instance.OnMusicVolumeChange(value / 100);
    PlayerPrefs.SetFloat(Consts.MusicValue, value);
  }

  public override void Show(bool show)
  {
    base.Show(show);
    if (show)
    {
      GameObject.Find("Canvas/SetPanel/Sound/Slider").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat(Consts.SoundValue, 50);
      GameObject.Find("Canvas/SetPanel/Music/Slider").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat(Consts.MusicValue, 50);
    }
  }

}

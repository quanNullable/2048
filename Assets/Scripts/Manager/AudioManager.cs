using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager _instance;
  private AudioSource music, sound;
  public AudioClip musicClip, soundClip;

  private void Awake()
  {
    _instance = this;
    music = transform.Find("Music").GetComponent<AudioSource>();
    sound = transform.Find("Sound").GetComponent<AudioSource>();
    music.volume = PlayerPrefs.GetFloat(Consts.MusicValue, 50) / 100;
    music.clip = musicClip;
    music.loop = true;
    sound.volume = PlayerPrefs.GetFloat(Consts.SoundValue, 50) / 100;
  }


  public void PlayMusic()
  {
    music.Play();
  }

  public void PlaySound()
  {
    sound.PlayOneShot(soundClip);
  }

  public void OnMusicVolumeChange(float value)
  {
    music.volume = value;
  }

  public void OnSoundVolumeChange(float value)
  {
    sound.volume = value;
  }
}

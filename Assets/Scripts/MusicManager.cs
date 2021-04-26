using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   [SerializeField]private AudioSource beginLoop;
   [SerializeField]private AudioSource mainLoop;
   [SerializeField]private AudioSource endLoop;
   
   private bool _mute = false;

   public bool Mute => _mute;

   private AudioSource _audioSource;
   private static MusicManager _singleton;

   public static MusicManager Singleton => _singleton;

   private void Start()
   {

   }

   public void MuteSounds()
   {
      this._mute = true;
      this.beginLoop.mute = _mute;
      this.mainLoop.mute = _mute;
      this.endLoop.mute = _mute;
   }

   public void UnmuteSounds()
   {
      this._mute = false;
      this.beginLoop.mute = _mute;
      this.mainLoop.mute = _mute;
      this.endLoop.mute = _mute;
   }
   
   private void Awake()
   {
      if (_singleton != null && _singleton != this)
         Destroy(this.gameObject);
      else
      {
         _singleton = this;
         DontDestroyOnLoad(this);
      }
   }

   public void StartBackgroundMusic()
   {
      //EndBackgroundMusic();
      if (!endLoop.isPlaying)
      {
         beginLoop.PlayDelayed(0f);
      }
      else
      {
         var endLoopClip = endLoop.clip;
         beginLoop.PlayDelayed(1.0f * endLoopClip.samples/endLoopClip.frequency - endLoop.time);
      }

      var beginLoopClip = beginLoop.clip;
      mainLoop.loop = true;
      mainLoop.PlayDelayed((float)beginLoopClip.samples / beginLoopClip.frequency);
   }

   public void EndBackgroundMusic()
   {
      var mainLoopClip = mainLoop.clip;
      mainLoop.loop = false;
      if(mainLoop.isPlaying)
         endLoop.PlayDelayed(((float)mainLoopClip.samples/mainLoopClip.frequency) - mainLoop.time);
      else
      {
         endLoop.PlayDelayed(0f);
      }
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
   private bool _isPaused;

   private static PauseManager _singleton;

   public bool IsPaused => _isPaused;

   public static PauseManager Singleton => _singleton;

   private void Awake()
   {
      _singleton = this;
   }

   public void PauseGame()
   {
      
      _isPaused = true;
      Time.timeScale = 0.0f;
   }

   public void UnPauseGame()
   {
      _isPaused = false;
      Time.timeScale = 1.0f;
   }
   
   
}

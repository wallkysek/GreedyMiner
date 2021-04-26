using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour
{
    private float _elapsedTime = 0f;
    void Start()
    {
        PauseManager.Singleton.PauseGame();
    }

    private void Update()
    {
        _elapsedTime += Time.unscaledDeltaTime;
        if (_elapsedTime > 4.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        PauseManager.Singleton.UnPauseGame();
    }
}

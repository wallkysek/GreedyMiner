    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
    
public class MenuUIEvents : MonoBehaviour
{
    [SerializeField] private Sprite[] _soundSprtes;
    [SerializeField] private Image _soundImage;
    private void Start()
    {
        MusicManager.Singleton.StartBackgroundMusic();
    }

    public void OnNewGameBtnClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MuteSounds()
    {
        if (MusicManager.Singleton.Mute)
        {
            MusicManager.Singleton.UnmuteSounds();
            _soundImage.sprite = _soundSprtes[0];
        }
        else
        {
            MusicManager.Singleton.MuteSounds();
            _soundImage.sprite = _soundSprtes[1];
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

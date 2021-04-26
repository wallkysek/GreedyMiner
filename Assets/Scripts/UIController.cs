using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _depthText;
    [SerializeField] private Slider _trainSlider;
    [SerializeField] private GameObject _pause;
    [SerializeField] private Sprite[] _audioSprites;
    [SerializeField] private Image _audio;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        ReRender();
        Interact();
    }

    void Interact()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ShowPause();
        }
    }
    
    public void MuteSounds()
    {
        if (MusicManager.Singleton.Mute)
        {
            MusicManager.Singleton.UnmuteSounds();
            _audio.sprite = _audioSprites[0];
        }
        else
        {
            MusicManager.Singleton.MuteSounds();
            _audio.sprite = _audioSprites[1];
        }
    }

    void ShowSounds()
    {
        if (MusicManager.Singleton.Mute)
        {
            _audio.sprite = _audioSprites[1];
        }
        else
        {
            _audio.sprite = _audioSprites[0];
        }
    }

    void ShowPause()
    {
        PauseManager.Singleton.PauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowSounds();
        _pause.SetActive(true);
    }

    public void Continue()
    {
        _pause.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        PauseManager.Singleton.UnPauseGame();
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        _pause.SetActive(false);
        PauseManager.Singleton.UnPauseGame();
        SceneManager.LoadScene("MenuScene");
    } 
    
    
    void ReRender()
    {
        float goldAmount = Mathf.Floor(ResourceManager.Singleton.GoldAmount);

        _goldText.text = goldAmount.ToString();
        _depthText.text = GetText();
        _trainSlider.maxValue = ResourceManager.Singleton.MaxHealthAmount;
        _trainSlider.value = ResourceManager.Singleton.HealthAmount;
    }

    string GetText()
    {
        if (DepthManager.Singleton.Depth < 1000f)
        {
            return Mathf.Floor(DepthManager.Singleton.Depth) + " m";
        }
        else
        {
            var x = Mathf.Floor(DepthManager.Singleton.Depth) / 1000f;

            return x.ToString("F1") + " km";
        }
    }
}
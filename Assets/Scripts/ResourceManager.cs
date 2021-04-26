using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private float goldThresholdPoint = 0f;
    
    private float _goldAmount = 1f;
    private float _lastGoldThreshold;
    private float _healthAmount;

    private float _playerOreDamage = 1f; 
    private float _playerDamage = 1f;
    private float _playerMovementSpeed = 3.5f;
    private float _playerShootingRate = 1.2f;
    private float _cartDamage = 1f;
    private float _cartShootingRate = 3f;
    public enum ThresholdTrend
    {
        DOWN, UP
    }
    
    public delegate void GoldThresholdReached(ThresholdTrend trend);

    public event GoldThresholdReached goldThresholdReached;
    
    
    public float GoldAmount => _goldAmount;
    public float HealthAmount => _healthAmount;
    public float MaxHealthAmount => maxHealth;
    
    public float PlayerDamage => _playerDamage;
    public float PlayerOreDamage => _playerOreDamage;
    public float PlayerMovementSpeed => _playerMovementSpeed;

    public float PlayerShootingRate => _playerShootingRate;
    public float CartShootingRate => _cartShootingRate;
    public float CartDamage => _cartDamage;
    
    [SerializeField]
    private float maxHealth;

    private static ResourceManager _singleton;

    public static ResourceManager Singleton => _singleton;

    private void Awake()
    {
        _singleton = this;
        _lastGoldThreshold = 0;
        _healthAmount = maxHealth;
    }

    public void AddGold(float amount)
    {
        _goldAmount += amount * DepthManager.Singleton.GetModifier();
        
        var goldThresholdDiff = _lastGoldThreshold - _goldAmount;
        if (Mathf.Abs(goldThresholdDiff) >= goldThresholdPoint)
        {
            goldThresholdReached?.Invoke((goldThresholdDiff >= 0)? ThresholdTrend.DOWN : ThresholdTrend.UP);
            _lastGoldThreshold += goldThresholdPoint;
        }
    }

    public void SubtractGold(float amount)
    {
        var finalAmount = _goldAmount - amount;
        if (finalAmount < 0)
        {
            _goldAmount = 0;
        }
        else
        
        {
            _goldAmount = finalAmount;
        }


        if (_goldAmount < _lastGoldThreshold)
        {
            goldThresholdReached?.Invoke(ThresholdTrend.DOWN);
            _lastGoldThreshold -= goldThresholdPoint;
        }
    }

    public void AddHealth(float amount)
    {
        var finalAmount = _healthAmount + amount;
        if (finalAmount > maxHealth)
        {
            _healthAmount = maxHealth;
        }
        else
        {
            _healthAmount = finalAmount;
        }
    } 
    public void SubtractHealth(float amount)
    {
        var finalAmount = _healthAmount - amount;
        if (finalAmount < 0)
        {
            _healthAmount = 0;
            Analytics.CustomEvent("GameOver", new Dictionary<string, object>
            {
                { "MaxDepth", DepthManager.Singleton.Depth},
            });
            
            MusicManager.Singleton.EndBackgroundMusic();
            SceneManager.LoadScene("DeathScene");
        }
        else
        {
            _healthAmount = finalAmount;
        }
    }

    public void AddDamage()
    {
        _playerDamage += 1f;
    }
    
    public void AddMovementSpeed()
    {
        _playerMovementSpeed += 0.5f;
    }
    
    public void AddShootingRate()
    {
        
        _playerShootingRate /= 1.15f;
    }
    
    public void AddCartShootingRate()
    {
        _cartShootingRate /= 1.2f;
    }
    
    public void AddCartDamage()
    {
        _cartDamage += 1f;
    }

    public void AddOreDamage()
    {
        _playerOreDamage += 1;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthManager : MonoBehaviour
{
    [SerializeField] private float _depth = 1.0f;
    public float Depth => _depth;

    private static DepthManager _singleton;

    public static DepthManager Singleton => _singleton;
    
    
    [SerializeField]
    private float depthIncrementThreshold;

    private float _currentDepthIncrementThreshold;
    public delegate void DepthIncremented();
    public event DepthIncremented DepthIncrementedByThreshold;

    private void Awake()
    {
        _singleton = this;
        _currentDepthIncrementThreshold = depthIncrementThreshold;
    }

    public void IncreaseDepth(float depth)
    {
        _depth += depth * ResourceManager.Singleton.GoldAmount;

        if (_depth > _currentDepthIncrementThreshold)
        {
            _currentDepthIncrementThreshold += depthIncrementThreshold * GetModifier();
            DepthIncrementedByThreshold?.Invoke();
            Debug.Log($"Called depth threshold, next is at {_currentDepthIncrementThreshold}");
            
        }
    }

    public float GetModifier()
    {
        if (_depth < 10000f)
            return 0.5f;
        if (_depth < 7500f)
            return 0.75f;
        if (_depth < 20000)
            return 1.05f;
        if (_depth < 30000)
            return 1.55f;
        if (_depth < 50000)
            return 1.85f;
        if (_depth < 750000)
            return 1.95f;
        if(_depth < 100000)
            return 2.35f;
        if (_depth < 125000)
            return 3f;
        if (_depth < 150000)
            return 3.35f;
        if (_depth < 175000)
            return 3.75f;
        if (_depth < 200000)
            return 4.15f;
        if (_depth < 225000)
            return 4.55f;
        if (_depth < 250000)
            return 5f;
        if (_depth < 275000)
            return 5.5f;
        if (_depth < 300000)
            return 6f;
        if (_depth < 325000)
            return 6.5f;
        if (_depth < 350000)
            return 7f;
        return 8f;
    }
}

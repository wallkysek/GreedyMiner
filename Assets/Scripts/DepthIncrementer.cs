using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthIncrementer : MonoBehaviour
{
    private Vector3 _lastPosition;
    private void Awake()
    {
        _lastPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        var transformPosition = this.transform.position;
        var depthIncrease = Vector3.Distance(_lastPosition, transformPosition) * 
                            ResourceManager.Singleton.GoldAmount;
        DepthManager.Singleton.IncreaseDepth(
            depthIncrease
        );
        _lastPosition = transformPosition;
    }
}

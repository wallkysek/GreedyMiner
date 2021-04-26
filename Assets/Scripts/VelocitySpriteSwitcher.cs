using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VelocitySpriteSwitcher : MonoBehaviour
{
    private Vector3 _latePosition;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField]
    private Sprite leftVeloSprite;
    [SerializeField]
    private Sprite downVeloSprite;

    private void Start()
    {
        _latePosition = this.transform.position;
        if (!TryGetComponent(out _spriteRenderer))
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        var currentPosition = this.transform.position;
        Vector3 direction = (currentPosition - _latePosition).normalized;
        if (Vector3.Distance(direction, Vector3.down) < Vector3.Distance(direction, Vector3.left))
        {
            _spriteRenderer.sprite = downVeloSprite;
        }
        else
        {
            _spriteRenderer.sprite = leftVeloSprite;
        }

        _latePosition = currentPosition;

    }
}

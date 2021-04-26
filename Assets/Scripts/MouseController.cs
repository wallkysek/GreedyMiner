using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private GameObject _shootingCursor;
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (!Cursor.visible)
        {
            
            _shootingCursor.SetActive(true);
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _shootingCursor.transform.position = cursorPos;
        }
        else
        {
            _shootingCursor.SetActive(false);
        }
        
    }
}

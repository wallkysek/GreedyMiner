using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player attributes")] 
    [SerializeField] private float _playerSpeed;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] public Animator _playerAnimator;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _gun;
    [SerializeField] private ParticleSystem _playerDust;
    private Vector2 _playerMovement;
    private Vector3 _mousePosition;
    private float _timer = 0.5f;
    
    [HideInInspector] public bool _canMine = false;
    [HideInInspector] public GameObject _ore = null;
    
    
    // Update is called once per frame
    void Update()
    {
        
        Movement();
        Mining();
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    void FixedUpdate()
    {
        _playerRb.MovePosition(_playerRb.position + ResourceManager.Singleton.PlayerMovementSpeed * Time.fixedDeltaTime * _playerMovement);
    }

    void Movement()
    {
        _playerMovement.x = Input.GetAxisRaw("Horizontal");
        _playerMovement.y = Input.GetAxisRaw("Vertical");
        
        if (_mousePosition.x < transform.position.x)
        {
            _player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(_mousePosition.x > transform.position.x)
        {
            _player.transform.localScale = new Vector3(1, 1, 1);
        }
        
        
        if (_playerMovement != Vector2.zero)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                GameObject newGo = Instantiate(_playerDust.gameObject, transform.position, Quaternion.identity);
                Destroy(newGo,1f);
                _timer = 0f;
            }
            
            _playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            _playerAnimator.SetBool("isMoving", false);
        }
    }

    
    
    void Mining()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canMine)
        {
            _gun.SetActive(false);
            _player.GetComponent<PlayerAnimator>()._ore = _ore;
            _playerAnimator.SetBool("isMining", true);
        }
    }
    
}

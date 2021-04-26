
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(AudioSource))]
public class GunController : MonoBehaviour
{
    
    [Header("Gun attributes")] 
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private ParticleSystem _shootParticle;

    private AudioSource _gunAudio;
    
    private Vector3 _mousePosition;
    public bool _canShoot = true;
    // Start is called before the first frame update
    void Awake()
    {
        _shootParticle.gameObject.SetActive(false);
        TryGetComponent(out _gunAudio);
    }

    // Update is called once per frame
    void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rotation();
    }

    void FixedUpdate()
    {
        Shooting();
    }

    void Shooting()
    {
        if (Input.GetMouseButton(0))
        {
            if (_canShoot)
            {
                _canShoot = false;
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload()
    {
        StartCoroutine(Shoot());
        yield return new WaitForSeconds(ResourceManager.Singleton.PlayerShootingRate);
        _canShoot = true;
    }
    
    
    IEnumerator Shoot()
    {
        var shape = _shootParticle.shape;
        shape.rotation = new Vector3(0, 0, GetRotation());
        _shootParticle.gameObject.SetActive(true);
        _shootParticle.Play();
        if (_gunAudio)
        {
            _gunAudio.mute = MusicManager.Singleton.Mute;
            _gunAudio.Play();
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(_firePoint.position, (_mousePosition- _firePoint.position),Mathf.Infinity);

        if (hitInfo)
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    EnemyController enemy = hitInfo.transform.GetComponent<EnemyController>();
                    enemy.Shot(ResourceManager.Singleton.PlayerDamage);
                }


                _lineRenderer.SetPosition(0, Vector3.zero);
                float x = Vector3.Distance(_firePoint.position, hitInfo.point);
                _lineRenderer.SetPosition(1, new Vector3(x,0,0));
            }
            else
            {
                _lineRenderer.SetPosition(0, Vector3.zero);
                _lineRenderer.SetPosition(1, new Vector3(100,0,0));
            }
        }
        else
        {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, new Vector3(100,0,0));
        }

        _lineRenderer.enabled = true;
        
        Camera.main.GetComponent<Animator>().SetBool("isShaking",true);
        yield return new WaitForSeconds(0.02f);
        _lineRenderer.enabled = false;
        
        Camera.main.GetComponent<Animator>().SetBool("isShaking",false);

    }

    float GetRotation()
    {
        if (_player.transform.localScale.x == -1)
        {
            return 125f; 
        }
        else
        {
            return -125f; 
        }
    }
    
    void Rotation()
    {
        Vector3 lookDir = _mousePosition - _gun.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(GetAngle(angle), Vector3.forward);
        _gun.transform.rotation = rotation;
    }
    
    float GetAngle(float angle)
    {
        if(_player.transform.localScale == new Vector3(-1, 1, 1))
        {
            return angle + 180;
        }else
        {
            return angle;
        }
    }
    
}

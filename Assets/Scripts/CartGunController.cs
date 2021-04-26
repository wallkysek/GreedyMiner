using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartGunController : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;

    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private AudioSource _cartAudio;
    
    private bool _canShoot = true;
    private GameObject _target = null;
    
    void Update()
    {
        CalculatedNearestTarget();
        Rotation();
        ValidShoot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.gameObject);
        }
    }

    void CalculatedNearestTarget()
    {
        if(_enemies.Count == 0) return;


        float distance = 500f;
        for (int i = 0; i < _enemies.Count; i++)
        {
            float newDistance = Vector3.Distance(transform.position, _enemies[i].transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                _target = _enemies[i];
            }
        }
    }
    
    
    void ValidShoot()
    {
        if (_canShoot)
        {
            _canShoot = false;
            StartCoroutine(Reload());
        }
    }
    
    IEnumerator Reload()
    {
            if (_target != null) StartCoroutine(Shoot());
            yield return new WaitForSeconds(ResourceManager.Singleton.CartShootingRate);
            _canShoot = true;
    }
    
    IEnumerator Shoot()
    {
        //_shootParticle.gameObject.SetActive(true);
        //_shootParticle.Play();
        
        if (_cartAudio)
        {
            _cartAudio.mute = MusicManager.Singleton.Mute;
            _cartAudio.Play();
        }

        EnemyController enemy = _target.GetComponent<EnemyController>();
        enemy.Shot(ResourceManager.Singleton.CartDamage);
        
        _lineRenderer.SetPosition(0, Vector3.zero);
        float x = Vector3.Distance(_firePoint.position, _target.transform.position);
        _lineRenderer.SetPosition(1, new Vector3(x * -1,0,0));
        
        
        _target = null;
        _lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        _lineRenderer.enabled = false;
    }
    
    
    void Rotation()
    {
        if(!_target) return;
        
        Vector3 lookDir = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
    
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy attributes")]
    [SerializeField] public float _enemyHp;

    [SerializeField] public float _enemyMovementSpeed;
    [SerializeField] public float _enemyDamage;
    [SerializeField] public ParticleSystem _enemyDeathParticle;
    [SerializeField] public GameObject _enemy;

    private GameObject _train;
    private GameObject[] _cargoTrains;
    private GameObject _target;
    private Vector2 _targetPos;
    private bool canAttack = true;

    void Awake()
    {
        _enemyDeathParticle.Stop();
        _train = GameObject.FindGameObjectWithTag("MainTrain");
        _cargoTrains = GameObject.FindGameObjectsWithTag("CargoTrain");
        _enemyDamage += DepthManager.Singleton.GetModifier() / 2;
        _enemyHp += GetModifier();
        FindTarget();
    }

    float GetModifier()
    {
        if (DepthManager.Singleton.GetModifier() > 1)
        {
            if (gameObject.name == "BigEnemy(Clone)")
            {
                return DepthManager.Singleton.GetModifier() * 1.75f;
            }
            else if(gameObject.name == "Enemy(Clone)")
            {
                return DepthManager.Singleton.GetModifier() * 1.25f;
            }
        }
        return DepthManager.Singleton.GetModifier();
    }
    
    void FindTarget()
    {
        if (ResourceManager.Singleton.GoldAmount < 10f)
        {
            _target = _train;
        }
        else
        {
            int random = Random.Range(0, _cargoTrains.Length);
            _target = _cargoTrains[random];
        }

        _targetPos = RandomPointOnUnitCircle(1f);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Shot(float damage)
    {
        _enemyHp -= damage;
        if (_enemyHp < 1f)
        {
            _enemyDeathParticle.Play();
            _enemy.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }

    void Movement()
    {
        CalculateTarget();
        var x = _targetPos.x + _target.transform.position.x + 0.5f;
        var y = _targetPos.y + _target.transform.position.y + 0.5f;
        Vector3 targetPos = new Vector3(x, y, 0f);
        gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos,
            _enemyMovementSpeed * Time.deltaTime * DepthManager.Singleton.GetModifier());

        if (Vector3.Distance(gameObject.transform.position, targetPos) <= 0.5f)
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(Attack());
            }
        }
    }

    void CalculateTarget()
    {
        if (ResourceManager.Singleton.GoldAmount < 10f)
        {
            _target = _train;
        }
        else
        {
            int random = Random.Range(0, _cargoTrains.Length);
            if(!_cargoTrains[random]) _cargoTrains = GameObject.FindGameObjectsWithTag("CargoTrain");
            else random = Random.Range(0, _cargoTrains.Length);
            _target = _cargoTrains[random];
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        if (_target.CompareTag("MainTrain")) ResourceManager.Singleton.SubtractHealth(_enemyDamage);
        else ResourceManager.Singleton.SubtractGold(1);
        canAttack = true;
    }

    Vector2 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;
        return new Vector2(x, y);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Enemy spawn attributes")] 
    [SerializeField] private GameObject[] _enemyPrefab;
    void Start()
    {
        
        StartCoroutine(GenerateEnemies());
    }
    
    
    IEnumerator GenerateEnemies()
    {
        
        while (true)
        {
            int max = 1 + Mathf.FloorToInt(DepthManager.Singleton.GetModifier() / 2) ;
            for (int i = 0; i < max; i++)
            {
                Vector2 spawnPos = RandomPointOnUnitCircle(10f);
                SpawnEnemy(spawnPos);
            }
            Debug.Log(3f - DepthManager.Singleton.GetModifier() / 4.5f);
            yield return new WaitForSeconds(3f - DepthManager.Singleton.GetModifier() / 4.5f);
        }
    }

    void SpawnEnemy(Vector2 spawnPos)
    {
        int random = Random.Range(0, _enemyPrefab.Length);
        EnemyController newEnemy =
            Instantiate(_enemyPrefab[random], spawnPos, Quaternion.identity).GetComponent<EnemyController>();
        
    }
    
    
    Vector2 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range (0f, Mathf.PI * 2);
        float x = Mathf.Sin (angle) * radius;
        float y = Mathf.Cos (angle) * radius;
        return new Vector2 (x + transform.position.x, y + transform.position.y);
        
    }
}

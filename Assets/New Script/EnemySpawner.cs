using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyPrefab;
    private BoxCollider boxCollider;
    
    public GameObject[] spawnEffect;
    public EnemyMovement[] allEnemy;

    public GameObject spawnContent;
    public int spawnLimit = 10;
    private int spawnNum;
    
    public float spawnTime = 5f;
    bool spawnLock = true;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        spawnContent.SetActive(false);
    }

    void Update()
    {
        allEnemy = GameObject.FindObjectsOfType<EnemyMovement>();

        if(spawnLock == true && allEnemy.Length < spawnLimit)
            StartCoroutine(Spawn_Enemy());
    }

    IEnumerator Spawn_Enemy()
    {
        spawnLock = false;
        Vector3 randomPosition = GetRandomPointInBox();

        int randomColor = Random.Range(0, enemyPrefab.Length);
        float randomSize = Random.Range(1f, 1.4f);
            
        if (enemyPrefab != null)
        {
            GameObject enemy = Instantiate(enemyPrefab[randomColor], randomPosition, Quaternion.identity);
            enemy.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            //spawnEffect[Random.Range(0, spawnEffect.Length)].transform.position = randomPosition;
        }

        yield return new WaitForSeconds(spawnTime);
        spawnLock = true;
    }

    Vector3 GetRandomPointInBox()
    {
        Vector3 size = boxCollider.size;
        Vector3 center = boxCollider.center;

        Vector3 worldCenter = boxCollider.transform.TransformPoint(center);
        Vector3 halfExtents = size * 0.5f;

        float randomX = Random.Range(-halfExtents.x, halfExtents.x);
        float randomY = Random.Range(-halfExtents.y, halfExtents.y);
        float randomZ = Random.Range(-halfExtents.z, halfExtents.z);

        Vector3 localRandomPosition = new Vector3(randomX, randomY, randomZ);
        Vector3 worldRandomPosition = boxCollider.transform.TransformPoint(localRandomPosition);

        return worldRandomPosition;
    }
}

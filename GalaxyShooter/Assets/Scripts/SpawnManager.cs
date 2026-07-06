using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private bool _stopSpawning = false;

    /*
    [SerializeField]
    private GameObject _tripleShotPowerupPrefab;
    */

    [SerializeField]
    private GameObject[] powerups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning() 
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (!_stopSpawning) // or (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.25f, 9.25f), 9, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            // Instantiate() returns a reference to the objects its created. To reach it and set its parent, we need to save it
            /* into a variable or of course you can add .transform.parent at the end of it etc. but this is a much better way
             * in case it returns null
             */
            if(newEnemy != null)
            {
                newEnemy.transform.parent = _enemyContainer.transform;
                // the reason we add .transform instead of writing just _enemyContainer is that
                // _enemyContainer is a GameObject
                // .parent is a type Transform
            }
            yield return new WaitForSeconds(5.0f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        // every 3 -7 seconds, spawn in a powerup
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.25f, 9.25f), 9, 0);
            Instantiate(powerups[Random.Range(0,3)], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds( Random.Range(3,8) ); // 3 inclusive, 8 exclusive -> (3,4,5,6,7) seconds
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}

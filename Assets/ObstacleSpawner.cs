using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleSpawner : MonoBehaviour
{
    public GameObject spawnerPrefab;
    public List<GameObject> spawnLocation = new List<GameObject>();
    public float timeToSpawn;
    private float currentTimeToSpawn;
    public float destroyDelay = 2f;
    public bool isRandomized;
    public bool isTimer;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnObject());
        //currentTimeToSpawn = timeToSpawn
        
    }

    /*
    IEnumerator SpawnObject()
    {
        //Spawn obsticle
        //Vector2 randomSpawnPosition = new Vector2(Random.Range(-30, 65), Random.Range(-35, 30));
        //GameObject spawnInstance = Instantiate(spawnerPrefab, spawnLocation.transform);

        //Destroy obsticle
        yield return new WaitForSeconds(destroyDelay);
        Destroy(spawnInstance);
    }
    */

    public void SpawnObject()
    {
        int index = isRandomized ? Random.Range(0, spawnLocation.Count) : 0;
        if (spawnLocation.Count > 0)
        {
            Instantiate(spawnerPrefab, spawnLocation[index].transform);
        }

    }

    private void UpdateTimer()
    {
        if (currentTimeToSpawn > 0)
        {
            currentTimeToSpawn -= Time.deltaTime;
        }
        else
        {
            SpawnObject();
            currentTimeToSpawn = timeToSpawn;
        }
    }

    void Update()
    {
        if (isTimer)
        {
            UpdateTimer();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Make this into a scriptable object.
    [System.Serializable]
    public class Wave
    {
        public GameObject[] paths;
        public int smallSpawnAmount;
        public int largeSpawnAmount;
        public int bossSpawnAmount;
    }
    [SerializeField] private GameObject pooledObjectPath;
    

    [SerializeField] private float enemySpawnInterval;
    [SerializeField] private float waveSpawnInterval;
    [SerializeField] private GameObject smallShipPrefab;
    [SerializeField] private GameObject largeShipPrefab;
    [SerializeField] private GameObject bossShipPrefab;
    

    [SerializeField] private Formation smallFormation;
    [SerializeField] private Formation largeFormation;
    [SerializeField] private Formation bossFormation;

    [SerializeField] private List<Wave> waveList = new List<Wave>();

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private int currentWave;
    private int smallShipID = 0;
    private int largeShipID = 0;
    private int bossShipID = 0;
    private List<Path> pathList = new List<Path>();
    private bool spawnComplete;


    private void Start()
    {
        Invoke("StartSpawn", 0.5f);
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWave < waveList.Count)
        {
            if (currentWave == waveList.Count-1)
            {
                spawnComplete = true;
            }
            

            for (int ii = 0; ii < waveList[currentWave].paths.Length; ii++)
            {
                GameObject newPathObj = Instantiate(waveList[currentWave].paths[ii], transform.position, Quaternion.identity);
                Path newPath = newPathObj.GetComponent<Path>();
                pathList.Add(newPath);
            }


            // Small ships first
            for (int ii = 0; ii < waveList[currentWave].smallSpawnAmount; ii++)
            {
                GameObject newSmallShip = Instantiate(smallShipPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyBehaviour shipBehaviour = newSmallShip.GetComponent<EnemyBehaviour>();

                shipBehaviour.SpawnSetup(pathList[PathFinder()], smallShipID, smallFormation, pooledObjectPath);
                smallShipID++;

                spawnedEnemies.Add(newSmallShip);
                GameManager.instance.AddEnemy();

                yield return new WaitForSeconds(enemySpawnInterval);
            }
            // large ships next
            for (int ii = 0; ii < waveList[currentWave].largeSpawnAmount; ii++)
            {
                GameObject newLargeShip = Instantiate(largeShipPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyBehaviour shipBehaviour = newLargeShip.GetComponent<EnemyBehaviour>();

                shipBehaviour.SpawnSetup(pathList[PathFinder()], largeShipID, largeFormation, pooledObjectPath);
                largeShipID++;

                spawnedEnemies.Add(newLargeShip);
                GameManager.instance.AddEnemy();

                yield return new WaitForSeconds(enemySpawnInterval);
            }

            for (int ii = 0; ii < waveList[currentWave].bossSpawnAmount; ii++)
            {
                GameObject newBossShip = Instantiate(bossShipPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyBehaviour shipBehaviour = newBossShip.GetComponent<EnemyBehaviour>();

                shipBehaviour.SpawnSetup(pathList[PathFinder()], bossShipID, bossFormation, pooledObjectPath);
                bossShipID++;

                spawnedEnemies.Add(newBossShip);
                GameManager.instance.AddEnemy();

                yield return new WaitForSeconds(enemySpawnInterval);
            }


            yield return new WaitForSeconds(waveSpawnInterval);
            
            currentWave++;

            foreach(Path path in pathList)
            {
                Destroy(path.gameObject);
            }
            pathList.Clear();
        }
        Invoke("CheckEnemyState", 1f);
    }

    private void CheckEnemyState()
    {
        bool inFormation = false;
        for (int ii = spawnedEnemies.Count-1; ii >= 0; ii--)
        {
            if(spawnedEnemies[ii].GetComponent<EnemyBehaviour>().enemyState != EnemyBehaviour.EnemyStates.IDLE)
            {
                inFormation = false;
                Invoke("CheckEnemyState", 1f);
                break;
            }
        }

        inFormation = true;

        if (inFormation)
        {
            StartCoroutine(smallFormation.ActivateSpread());
            StartCoroutine(largeFormation.ActivateSpread());
            StartCoroutine(bossFormation.ActivateSpread());
            CancelInvoke("CheckEnemyState");
        }
    }

    private void StartSpawn()
    {
        StartCoroutine(SpawnWaves());
        CancelInvoke("StartSpawn");
    }

    private int PathFinder()
    {
        return (smallShipID + largeShipID + bossShipID) % pathList.Count;
    }

    private void OnValidate()
    {
        int currentSmallShipAmount = 0;
        for (int ii = 0; ii < waveList.Count; ii++)
        {
            currentSmallShipAmount += waveList[ii].smallSpawnAmount;
        }
    }

    private void Reporting()
    {
        if(spawnedEnemies.Count == 0 && spawnComplete)
        {
            GameManager.instance.WinGame();
        }
    }
    public void UpdateEnemies(GameObject _enemy)
    {
        spawnedEnemies.Remove(_enemy);
        Reporting();
    }




}

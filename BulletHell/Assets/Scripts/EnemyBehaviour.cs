using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    //---------------------------------------------
    // Serialized variables
    //---------------------------------------------

   
    [SerializeField] [Range(0f, 1500f)] float enemyHealth = 200f;
    [SerializeField] [Range(0f, 800f)] public int score = 50;

    // Bullet Settings
    [SerializeField] [Range(0f, 1f)]    public float fireRate = 0.2f;
    [SerializeField] [Range(0f, 100f)]  public float bulletSpeed = 20f;
    [SerializeField] [Range(0, 8)]      public int fireAmount = 2;
    [SerializeField] [Range(0, 8)]      public float bulletDamage = 5f;
    [SerializeField] [Range(0, 200)]    public float rotationSpeed = 20f;

    // Animation Settings
    [SerializeField] [Range(0f, 15f)]   private float animSpeed = 2f;
    [SerializeField] [Range(0f, 1f)]    private float animReachDist = 0.2f;
    [SerializeField] [Range(1f, 30f)]   private float animRotSpeed = 5f;
    [SerializeField] private bool animUseBezier = false;

    // Extra Options
    // Debugging Options
    [SerializeField] private bool noDeath = false;
    [SerializeField] private bool noRotation = false;
    // Health Settings
    [SerializeField] private Image healthBar;
    // Animation Settings
    [SerializeField] private GameObject enemySprite = null;
    [SerializeField] private Transform bulletSpawnPoint = null;
    // Bullet Settings
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] [Range(0, 500)] private int pooledAmount = 20;
    [SerializeField] private bool dynamicList = true;
    [SerializeField] private GameObject explosion = null;


    //---------------------------------------------
    // Public variables
    //---------------------------------------------
    public enum EnemyStates { INCOMING, FORMATION, IDLE, DIVING }
    
    public Path followPath; // Custom waypoint Pather
    public int currentWayPointID = 0; // Current waypoint
    public int enemyID; // Current ID for formation
    public Formation formation; // Formation set via the SpawnManager
    public EnemyStates enemyState;

    //---------------------------------------------
    // Private variables
    //---------------------------------------------

    private GameObject bulletParent = null;
    private Transform playerTransform = null;
    private List<GameObject> pooledBullets = null;
    private float nextBullet;

    private float _startHealth = 0f;

    private float dist;
    
    //---------------------------------------------
    //Initialise values
    //---------------------------------------------

    private void Start()
    {
        _startHealth = enemyHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        pooledBullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.parent = bulletParent.transform;
            obj.SetActive(false);
            pooledBullets.Add(obj);
        }
    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyStates.INCOMING:
                {
                    MoveOnPath(followPath);
                }
                break;
            case EnemyStates.FORMATION:
                {
                    MoveToFormation();
                }
                break;
            case EnemyStates.IDLE:
                {
                    //SpawnBullet();
                }
                break;
            case EnemyStates.DIVING:
                {
                    MoveOnPath(followPath);
                    //SpawnBullet();
                }
                break;
        }
    }

    //---------------------------------------------
    // Bullet methods
    //---------------------------------------------

    public void ApplyDamage(float damage)
    {
        enemyHealth -= damage;
        healthBar.fillAmount = enemyHealth / _startHealth;
        if (enemyHealth <= 0f)
        {
            if (!noDeath) // Debugging purposes
            {
                Instantiate(explosion, transform.position, Quaternion.identity);

                if(enemyState == EnemyStates.IDLE )
                { 
                       GameManager.instance.AddScore(score);
                }else 
                {
                    GameManager.instance.AddScore(score * 2);
                }
                
                for (int ii = 0; ii < formation.enemyFormation.Count; ii++)
                {
                    if(formation.enemyFormation[ii].e_Index == enemyID)
                    {
                        formation.enemyFormation.Remove(formation.enemyFormation[ii]);
                    }
                }
                
                SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
                spawnManager.UpdateEnemies(gameObject);
                

                Destroy(gameObject);
            }
        }
    }

    private void SpawnBullet()
    {
        if(Time.time > nextBullet)
        {
            nextBullet = Time.time + fireRate;
            Vector3 targetDir = (playerTransform.position - bulletSpawnPoint.position).normalized;
            float rotZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            bulletSpawnPoint.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);// = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);
            BulletFire(bulletSpawnPoint);
        }
    }

    public void BulletFire(Transform firePoint)
    {
        GameObject obj = pooledObjects();

        if (obj == null) return;

        obj.transform.position = firePoint.position;
        obj.transform.rotation = firePoint.rotation;
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = firePoint.up * bulletSpeed;
        obj.GetComponent<Bullet>().SetDamage(bulletDamage);
    }


    public void Fire()
    {
        float angleStep = 360f / (float)fireAmount;
        float angle = 0f;
        Vector3 firePos = bulletSpawnPoint.position;
        for (int i = 0; i <= fireAmount; i++)
        {
            GameObject bullet = pooledObjects();
            if (bullet == null) break;



            float xDir = Mathf.Sin(angle * Mathf.PI / 180f);
            float yDir = Mathf.Cos(angle * Mathf.PI / 180f);

            float xy = Mathf.Atan2(xDir, yDir) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, xy));


            bullet.transform.position = firePos;
            bullet.transform.rotation = bulletSpawnPoint.rotation * rotation;

            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;

            angle += angleStep;
        }

    }



    private GameObject pooledObjects()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
            if (dynamicList)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.transform.parent = bulletParent.transform;
                obj.SetActive(false);
                pooledBullets.Add(obj);
                return obj;
            }
        }
        return null;
    }

    void OnCollisionEnter2D(Collision2D cold)
    {
        Collider2D col = cold.collider;
        if (col != null && col.tag == "Player")
        {
            col.SendMessage("ApplyDamage", 50f);
            ApplyDamage(100000);
        }
    }

    //---------------------------------------------
    // Called methods
    //---------------------------------------------

    public void SpawnSetup(Path _path, int _ID, Formation _formation, GameObject _bulletParent)
    {
        followPath = _path;
        enemyID = _ID;
        formation = _formation;
        bulletParent = _bulletParent;
    }

    public void DiveSetup(Path _path)
    {
        followPath = _path;
        transform.SetParent(null);
        enemyState = EnemyStates.DIVING;
    }

    

    

    //---------------------------------------------
    // Animation methods
    //---------------------------------------------

    void MoveToFormation()
    {
        transform.position = Vector3.MoveTowards(transform.position, formation.GetVector(enemyID), animSpeed * Time.deltaTime);

        Vector3 targetDir = (formation.GetVector(enemyID) - transform.position).normalized;
        if (targetDir != Vector3.zero)
        {
            float rotZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, animRotSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, formation.GetVector(enemyID)) <= 0.001f)
        {
            transform.SetParent(formation.gameObject.transform);
            transform.eulerAngles = Vector3.zero;

            formation.enemyFormation.Add(new Formation.EnemyFormation(enemyID, transform.localPosition.x, transform.localPosition.y, gameObject));

            enemyState = EnemyStates.IDLE;
        }
    }

    private void MoveOnPath(Path _path)
    {

        if (animUseBezier)
        {
            PathMovement(_path.bezierObjList[currentWayPointID], _path.bezierObjList.Count);
        }
        else
        {
            PathMovement(_path.pathObjList[currentWayPointID].position, _path.pathObjList.Count);
        }
    }

    private void PathMovement(Vector3 _pos, int _listCount)
    {
        // Transform
        dist = Vector3.Distance(_pos, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, _pos, animSpeed * Time.deltaTime);

        //Rotation
        Vector3 targetDir = (_pos - transform.position).normalized;
        if(targetDir != Vector3.zero)
        {
            float rotZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, animRotSpeed * Time.deltaTime);
        }
        if (dist <= animReachDist)
        {
            currentWayPointID++;
        }
        if (currentWayPointID >= _listCount)
        {
            currentWayPointID = 0;

            if(enemyState == EnemyStates.DIVING)
            {
                Transform spawnManager = FindObjectOfType<SpawnManager>().GetComponent<Transform>();
                transform.position = spawnManager.position;
                Destroy(followPath.gameObject);
            }
            

            enemyState = EnemyStates.FORMATION;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] [Range(0f, 1500f)] float enemyHealth = 200f;
    
    [Space]

    [Header("Animation Settings")]
    [SerializeField] public Vector2 animSize = new Vector2(2f, 2f);
    [SerializeField] public Vector2 animOffset = new Vector2(0f, -2f);
    [SerializeField] [Range(0f, 4f)] public float animXSpeed = 1f;
    [SerializeField] [Range(0f, 4f)] public float animYSpeed = 1f;
    [Space]

    [Header("Bullet Settings")]
    [SerializeField] [Range(0f, 1f)] public float fireDelay = 0.2f;
    [SerializeField] [Range(0f, 100f)] public float bulletSpeed = 20f;
    [SerializeField] [Range(0, 8)] public int fireAmount = 2;
    [SerializeField] [Range(0, 200)] public float rotationSpeed = 20f;

    [System.Serializable]
    public class ExtraSettings
    {
        [Header("Debugging Options")]
        [SerializeField] public bool NoDeath = false;
        [SerializeField] public bool noRotation = false;
        [Header("Health Settings")]
        [SerializeField] public Image healthBar;
        [Header("Animation Settings")]
        [SerializeField] public GameObject enemySprite = null;
        [SerializeField] public Transform bulletSpawnPoint = null;
        [Header("Bullet Settings")]
        [SerializeField] public GameObject bulletPrefab = null;
        [SerializeField] public GameObject bulletParent = null;
        [SerializeField] [Range(0, 500)] public int pooledAmount = 20;
        [SerializeField] public bool dynamicList = true;
    }

    [SerializeField] private ExtraSettings extraSettings;



    //---------------------------------------------
    //Private settings
    //---------------------------------------------

    private List<GameObject> _bullets;
    private Rigidbody2D _rbd;
    private float _startHealth = 0f;

    //---------------------------------------------
    //Initialise values
    //---------------------------------------------

    void Start()
    {
        _rbd = gameObject.GetComponent<Rigidbody2D>();
        _startHealth = enemyHealth;

        _bullets = new List<GameObject>();
        for (int i = 0; i < extraSettings.pooledAmount; i++)
        {
            GameObject obj = Instantiate(extraSettings.bulletPrefab);
            obj.transform.parent = extraSettings.bulletParent.transform;
            obj.SetActive(false);
            _bullets.Add(obj);
        }

        StartCoroutine(ForceFiring(fireDelay));
    }

    //---------------------------------------------
    //Enemy movement and health system
    //---------------------------------------------
    void FixedUpdate()
    {
        
        Vector2 movement = new Vector2(Mathf.Sin(Time.time * animXSpeed) *  animSize.x - animOffset.x, Mathf.Cos(Time.time * animYSpeed) * animSize.y - animOffset.y);
        _rbd.position = movement;

        Quaternion enemyRotate = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Sin(Time.time) * 2f * Mathf.Rad2Deg));
        extraSettings.enemySprite.transform.rotation = enemyRotate;

        if(!extraSettings.noRotation)
            FireRotation();
    }


    public void ApplyDamage(float damage)
    {
        enemyHealth--;
        extraSettings.healthBar.fillAmount = enemyHealth / _startHealth;
        if(enemyHealth <= 0f)
        {
            if(!extraSettings.NoDeath)
                Destroy(gameObject);
        }
    }
    //---------------------------------------------
    //Bullet firing methods
    //---------------------------------------------


    IEnumerator ForceFiring(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Fire();
        }
    }
    
    public void Fire()
    {
        float angleStep = 360f / (float)fireAmount;
        float angle = 0f;
        Vector3 firePos = extraSettings.bulletSpawnPoint.position;
        for (int i = 0; i <= fireAmount; i++)
        {
            GameObject bullet = pooledObjects();
            if (bullet == null) break;

            

            float xDir = Mathf.Sin(angle * Mathf.PI / 180f);
            float yDir = Mathf.Cos(angle * Mathf.PI / 180f);

            float xy = Mathf.Atan2(xDir, yDir) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, xy));


            bullet.transform.position = firePos;
            bullet.transform.rotation = extraSettings.bulletSpawnPoint.rotation * rotation;

            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;

            angle += angleStep;
        }
        
    }

    private void FireRotation()
    {
        float h = Mathf.Sin(Time.time * rotationSpeed);
        float v = Mathf.Cos(Time.time * rotationSpeed);
        float hv = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
        Vector3 rot = new Vector3(0f, 0f, hv);
        Quaternion rotation = Quaternion.Euler(rot);

        extraSettings.bulletSpawnPoint.rotation = rotation;
    }

    //Bullet pooling - taken from Brackeys
    private GameObject pooledObjects()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (!_bullets[i].activeInHierarchy)
            {
                return _bullets[i];
            }
            if (extraSettings.dynamicList)
            {
                GameObject obj = Instantiate(extraSettings.bulletPrefab);
                obj.transform.parent = extraSettings.bulletParent.transform;
                obj.SetActive(false);
                _bullets.Add(obj);
                return obj;
            }
        }
        return null;
    }
}
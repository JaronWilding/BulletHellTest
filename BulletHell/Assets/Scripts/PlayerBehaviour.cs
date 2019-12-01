using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float playerHealth = 100f;

    [SerializeField, Range(0f, 1f)] private float fireRate = 0.5f;
    [SerializeField, Range(1f, 50f)] private float bulletSpeed = 10f;
    [SerializeField] private int pooledAmount = 20;
    [SerializeField] private bool dynamicList = true;
    
    [SerializeField] private Transform[] bulletSpawns = null;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject bulletParent = null;
    [SerializeField] private float bulletDamage = 5f;
    [SerializeField] private GameObject explosion = null;

    private bool isMoving;
    private Vector3 screenPoint;
    private Vector3 offset;

    private float nextBullet;
    private int bulletLevel = 2;
    private List<GameObject> bullets = null;
    private ScreenBoundary updateTransform;

    private float startHealth = 0f;

    private Vector3 initPosition;
    private bool isDead;

    private void Awake()
    {
        updateTransform = new ScreenBoundary(gameObject);
        initPosition = transform.position;
    }

    private void Start()
    {
        
        startHealth = playerHealth;

        bullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.parent = bulletParent.transform;
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }


    void Update()
    {
        if(isMoving && Time.time > nextBullet && !isDead)
        {
            nextBullet = Time.time + fireRate;
            for (int ii = 0; ii < bulletSpawns.Length; ii++)
            {
                BulletFire(bulletSpawns[ii]);
            }
        }
    }

    public void BulletFire(Transform firePoint)
    {
        GameObject obj = pooledObjects();

        if (obj == null) return;

        obj.transform.position = firePoint.position;
        obj.transform.rotation = firePoint.rotation;
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        obj.GetComponent<Bullet>().SetDamage(bulletDamage);
    }

    private GameObject pooledObjects()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
            if (dynamicList)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.transform.parent = bulletParent.transform;
                obj.SetActive(false);
                bullets.Add(obj);
                return obj;
            }
        }
        return null;
    }
    public void ApplyDamage(float damage)
    {
        playerHealth -= damage;
        GameManager.instance.HealthUpdate(playerHealth / startHealth);
        if (playerHealth <= 0f)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

            //Destroy(gameObject);
            StartCoroutine(ResetPlayer());
        }
    }


    IEnumerator ResetPlayer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        transform.position = initPosition;

        yield return new WaitForSeconds(2f);

        GameManager.instance.HealthUpdate(1);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        isDead = false;
    }


    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag()
    {
        if (!isDead)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            isMoving = true;
            transform.position = curPosition;
            transform.position = updateTransform.updateTrans(transform.position);
        }
    }

    private void OnMouseUp()
    {
        isMoving = false;

    }
}

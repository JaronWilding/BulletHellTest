using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject bulletParent = null;
    [SerializeField] private float fireDelay = 0.2f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int pooledAmount = 20;
    [SerializeField] private bool dynamicList = true;
    private List<GameObject> bullets;
    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.parent = bulletParent.transform;
            obj.SetActive(false);
            bullets.Add(obj);
        }
        StartCoroutine(ForceFiring(fireDelay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        GameObject obj = pooledObjects();

        if (obj == null) return;

        obj.transform.position = firePoint.position;
        obj.transform.rotation = firePoint.rotation;
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = -transform.up * bulletSpeed;

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
}

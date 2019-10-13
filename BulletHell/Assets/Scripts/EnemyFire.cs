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
    [SerializeField] private int fireAmount = 2;
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
        for(int i = 0; i < fireAmount; i++)
        {
            GameObject obj = pooledObjects();
            if (obj == null) break;
            int direction = (i % 2 == 1) ? 1 : -1;


            FireBullet(obj, firePoint, direction);
        }
        

    }

    private void FireBullet(GameObject bullet, Transform firePosition, int direction)
    {
        bullet.transform.position = firePosition.position;
        bullet.transform.rotation = firePosition.rotation;
        bullet.SetActive(true);
        if(direction == -1)
            bullet.GetComponent<Rigidbody2D>().velocity = -firePosition.transform.up * bulletSpeed;
        else
            bullet.GetComponent<Rigidbody2D>().velocity = firePosition.transform.up * bulletSpeed;
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

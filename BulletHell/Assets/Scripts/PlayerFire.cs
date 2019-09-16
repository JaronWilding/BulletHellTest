using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject bulletParent = null;
    [SerializeField] private float fireDelay = 0.2f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int pooledAmount = 20;
    [SerializeField] private bool dynamicList = true;
    private List<GameObject> bullets;

    private void Start()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.parent = bulletParent.transform;
            obj.SetActive(false);
            bullets.Add(obj);
        }
        //InvokeRepeating("fire", fireDelay, fireDelay);
        //StartCoroutine(fireBullet(fireDelay));
        //Invoke("Firing", 1);
        StartCoroutine(ForceFiring(fireDelay));
    }

    IEnumerator ForceFiring(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            fire2();
        }
    }


    public void fire2()
    {
        GameObject obj = pooledObjects();

        if (obj == null) return;

        obj.transform.position = firePoint.position;
        obj.transform.rotation = firePoint.rotation;
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;

    }



    public void fire()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = firePoint.position;
                bullets[i].transform.rotation = firePoint.rotation;
                bullets[i].SetActive(true);
                bullets[i].GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
                break;
            }
        }

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

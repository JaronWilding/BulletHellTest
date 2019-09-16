using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private ScreenBoundary isOutside;
    private void Start()
    {
        isOutside = new ScreenBoundary(gameObject);
    }
    private void OnEnable()
    {
        //Invoke("Destroy", 2f);
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isOutside.currentPos(transform.position))
        {
            Destroy();
        }
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}

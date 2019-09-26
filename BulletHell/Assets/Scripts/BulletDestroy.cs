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
    protected void Destroy()
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
    protected virtual void OnCollisionEnter2D(Collision2D cold)
    {
        Collider2D col = cold.collider;
        if(col != null && col.tag == "Enemy")
        {
            col.SendMessage("ApplyDamage", 5.0f);
            Destroy();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private ScreenBoundary isOutside;
    private void Start()
    {
        isOutside = new ScreenBoundary(gameObject);
    }
    private void OnEnable()
    {
        //Invoke("Destroy", 2f);
    }
    protected void DestroyObject()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isOutside.currentPos(transform.position))
        {
            DestroyObject();
        }
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    protected virtual void OnCollisionEnter2D(Collision2D cold)
    {
        Collider2D col = cold.collider;
        if(col != null && col.tag == "Enemy")
        {
            col.SendMessage("ApplyDamage", damage);
            DestroyObject();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}

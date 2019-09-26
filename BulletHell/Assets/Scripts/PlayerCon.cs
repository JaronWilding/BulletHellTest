using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    //[SerializeField] private float tilt = 2.0f;
    [SerializeField] private GameObject playerSprite = null;
    private Rigidbody2D rbd;
    private ScreenBoundary updateTransform;
    private Vector2 screenBounds;

    private void Awake()
    {
        updateTransform = new ScreenBoundary(playerSprite);
    }

    private void Start()
    {
        rbd = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveH, moveV);

        movement = movement.normalized * speed;

        rbd.velocity = movement;
        rbd.position = updateTransform.updateTrans(rbd.position);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col != null)
        {
            if (col.tag == "Enemy" || col.tag == "EnemyBullets")
            {
                Destroy(gameObject);
            }
        }
    }
}

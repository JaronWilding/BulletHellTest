using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private GameObject playerSprite = null;
    [SerializeField] private float playerHealth = 100f;
    //[SerializeField] private float tilt = 2.0f;


    private Rigidbody2D rbd;
    private ScreenBoundary updateTransform;
    private Vector2 screenBounds;
    private float startHealth = 0f;

    private void Awake()
    {
        updateTransform = new ScreenBoundary(playerSprite);
    }
    private void Start()
    {
        rbd = gameObject.GetComponent<Rigidbody2D>();
        startHealth = playerHealth;
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


    public void ApplyDamage(float damage)
    {
        playerHealth--;
        Debug.Log(playerHealth);
        //healthBar.fillAmount = enemyHealth / startHealth;
        if(playerHealth <= 0f)
        {

            //Destroy(gameObject);
        }
    }
}

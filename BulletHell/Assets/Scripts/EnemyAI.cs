using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float xSize = 0.5f;
    [SerializeField] float ySize = 0.5f;
    [SerializeField] float xOffset = 0.5f;
    [SerializeField] float yOffset = 0.5f;
    [SerializeField] float xSpeed = 1f;
    [SerializeField] float ySpeed = 1f;
    private Rigidbody2D _rbd;

    // Start is called before the first frame update
    void Start()
    {
        _rbd = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(Mathf.Sin(Time.time * xSpeed) - xOffset * xSize, Mathf.Cos(Time.time * ySpeed)  - yOffset * ySize);
        _rbd.position = movement;
    }
}

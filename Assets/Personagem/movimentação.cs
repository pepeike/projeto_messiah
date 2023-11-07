using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentação : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float inputDirection;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputMove();
    }

    private void FixedUpdate()
    {
       MoveLogic();
    }

    void GetInputMove()
    {
        inputDirection = Input.GetAxisRaw("Horizontal");
    }
    void MoveLogic()
    {
        rb2d.velocity = new Vector2(inputDirection * moveSpeed, rb2d.velocity.y);
    }
}

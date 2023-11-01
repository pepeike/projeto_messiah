using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    private enum EnemyState
    {
        Idle,
        Moving,
        Attacking,
        Damaged,
    }

    //[SerializeField]
    //private int hitPoints = 0;

    public GameObject Player;
    private Vector3 playerPos;
    private float difX;
    private float difY;
    private Vector2 moveDir;

    [SerializeField]
    private float enemySpeed;

    private Rigidbody2D rb;

    private EnemyState state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        state = EnemyState.Idle;
    }

    

    private void FixedUpdate()
    {
        

        if (Player != null)
        {
            playerPos = Player.transform.position;
            
            difX = playerPos.x - transform.position.x;
            difY = playerPos.y - transform.position.y;

            if (difX > 0 && difY > 0)
            {
                moveDir = Vector2.one;
            }

            if (difX < 0 && difY < 0)
            {
                moveDir = -Vector2.one;
            }

            if (difX < 0 && difY > 0)
            {
                moveDir = new Vector2(-1, 1);
            }

            if (difX > 0 && difY < 0)
            {
                moveDir = new Vector2(1, -1);
            }

            

        }

        switch (state)
        {
            case EnemyState.Idle:
                StartCoroutine(Idle());
                break;
            case EnemyState.Moving:
                StartCoroutine(Move());
                break;

        }

    }

    IEnumerator Move()
    {
        
        yield return new WaitForSeconds(1);
        rb.AddForce(moveDir * enemySpeed);
        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
        state = EnemyState.Idle;

    }

    IEnumerator Idle()
    {

        yield return new WaitForSeconds(2);
        state = EnemyState.Moving;

    }


}

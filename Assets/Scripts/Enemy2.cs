using System.Collections;
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

    [SerializeField]
    private int hitPoints = 0;

    public GameObject Player;
    private Vector3 playerPos;
    private Vector2 endPos;


    private float enemySpeed;

    private Rigidbody2D rb;

    private EnemyState state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        state = EnemyState.Idle;
    }

    private void FixedUpdate()
    {
        

        if (Player != null)
        {
            playerPos = Player.transform.position;
            endPos = transform.position - playerPos;


        }

        switch (state)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:

                break;

        }

    }

    IEnumerator Move()
    {
        
        yield return new WaitForSeconds(1);

    }

    IEnumerator Idle()
    {

    }


}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

    private enum EnemyState {
        Idle,
        Moving,
        Attacking,
        Damaged,
    }

    [SerializeField]
    private int hitPoints = 10;

    public GameObject Player;
    private Vector3 playerPos;
    private float difX;
    private float difY;
    private Vector2 moveDir;

    [SerializeField]
    private float enemySpeed;

    private Rigidbody2D rb;

    private EnemyState state;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        state = EnemyState.Idle;
    }



    private void FixedUpdate() {

        if (hitPoints <= 0) { Destroy(gameObject); }

        if (Player != null) {
            playerPos = Player.transform.position;

            difX = playerPos.x - transform.position.x;
            difY = playerPos.y - transform.position.y;

            if (difX > 0 && difY > 0) {
                moveDir = Vector2.one;
            }

            if (difX < 0 && difY < 0) {
                moveDir = -Vector2.one;
            }

            if (difX < 0 && difY > 0) {
                moveDir = new Vector2(-1, 1);
            }

            if (difX > 0 && difY < 0) {
                moveDir = new Vector2(1, -1);
            }



        }

        switch (state) {
            case EnemyState.Idle:
                //StartCoroutine(Idle());
                rb.velocity = Vector3.zero;
                break;
            case EnemyState.Moving:
                Move();
                //StartCoroutine(Move());
                break;

        }

    }

    void PassTick() {

        if (state == EnemyState.Idle) {
            state = EnemyState.Moving;
            Debug.Log("Moving");
        } else if (state == EnemyState.Moving) {
            state = EnemyState.Idle;
            Debug.Log("Stopping");
        }

    }

    void Move() {
        rb.AddForce(moveDir * enemySpeed);
    }

    void Attack() {

    }

    public void Damage(int dmg) {
        if (hitPoints > 0) {
            hitPoints -= dmg;
        }
    }
}

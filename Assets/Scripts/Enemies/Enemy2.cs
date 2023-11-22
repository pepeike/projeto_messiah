using System.Collections;
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

    [SerializeField]
    private float minAtkDist = 2;

    private Animator anim;

    public GameObject Player;
    private Vector3 playerPos;
    //private float difX;
    //private float difY;
    //private Vector2 moveDir;

    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private float playerDist;

    private float angle;

    private float playerX;
    private float playerY;

    private Camera cam;

    private LevelManager levelManager;

    //private Transform[] LPoints;
    private Transform target;

    [SerializeField]
    private GameObject atk;

    private short atkCount = 0;

    [SerializeField]
    private float enemySpeed;

    private Rigidbody2D rb;

    private EnemyState state;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        state = EnemyState.Idle;
        cam = GameObject.FindAnyObjectByType<Camera>();
        anim = GetComponent<Animator>();



        localScale = transform.localScale;

        levelManager = FindAnyObjectByType<LevelManager>();

    }

    private void Start() {

        int _randFloat = Random.Range(0, 4);

        atk.SetActive(false);

        target = levelManager.lPoints[_randFloat];

        Debug.Log(_randFloat);

    }

    private void FixedUpdate() {

        if (hitPoints <= 0) { Destroy(gameObject); }

        //RotateEnemy();



        if (Player != null) {
            //playerPos = Player.transform.position;

            playerDist = Vector2.Distance(transform.position, playerPos);

            //Debug.Log(playerDist);

            //difX = playerPos.x - transform.position.x;
            //difY = playerPos.y - transform.position.y;

            //if (difX > 0 && difY > 0) {
            //    moveDir = Vector2.one;
            //}

            //if (difX < 0 && difY < 0) {
            //    moveDir = -Vector2.one;
            //}

            //if (difX < 0 && difY > 0) {
            //    moveDir = new Vector2(-1, 1);
            //}

            //if (difX > 0 && difY < 0) {
            //    moveDir = new Vector2(1, -1);
            //}



        }

        switch (state) {
            case EnemyState.Idle:
                //StartCoroutine(Idle());
                RotateEnemy();
                rb.velocity = Vector3.zero;
                break;
            case EnemyState.Moving:
                RotateEnemy();
                Move();
                //StartCoroutine(Move());
                break;
            case EnemyState.Attacking:
                if (atkCount == 0) {
                    atkCount++;
                    StartCoroutine(EnemyAttack());
                }

                break;

        }

    }

    void PassTick() {

        if (state == EnemyState.Idle) {
            if (playerDist < minAtkDist) {
                state = EnemyState.Attacking;
                Debug.Log("Attacking");
            } else {
                state = EnemyState.Moving;
                Debug.Log("Moving");
            }
        } else if (state == EnemyState.Moving) {
            if (playerDist < minAtkDist) {
                state = EnemyState.Attacking;
                Debug.Log("Attacking");
            } else {
                state = EnemyState.Idle;
                Debug.Log("Idle");
            }
        }

    }

    void Move() {
        //rb.AddForce(moveDir * enemySpeed);
        if (Player != null) {
            directionToPlayer = (target.position - transform.position).normalized;
            rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * enemySpeed;
        }
    }



    public void Damage(int dmg) {
        if (hitPoints > 0) {
            hitPoints -= dmg;
        }
    }

    private void RotateEnemy() {

        if (Player != null) {
            playerPos = Player.transform.position;
        }


        //Vector3 pp = cam.ScreenToWorldPoint(playerPos);

        float AngleRad = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x);

        angle = (180 / Mathf.PI) * AngleRad;

        playerX = playerPos.x - transform.position.x;
        playerY = playerPos.y - transform.position.y;

        rb.rotation = angle;

        //Debug.Log(playerPos);

        //horroroso mas funciona
        //if (Mathf.Abs(playerY) > Mathf.Abs(playerX) && playerY > 0) {
        //    anim.SetBool("facingUp", true);
        //    anim.SetBool("facingRight", false);
        //    anim.SetBool("facingLeft", false);
        //    anim.SetBool("facingDown", false);
        //}
        //else if (Mathf.Abs(playerY) < Mathf.Abs(playerX) && playerX > 0) {
        //    anim.SetBool("facingUp", false);
        //    anim.SetBool("facingRight", true);
        //    anim.SetBool("facingLeft", false);
        //    anim.SetBool("facingDown", false);
        //}
        //else if (Mathf.Abs(playerY) > Mathf.Abs(playerX) && playerY < 0) {
        //    anim.SetBool("facingUp", false);
        //    anim.SetBool("facingRight", false);
        //    anim.SetBool("facingLeft", false);
        //    anim.SetBool("facingDown", true);
        //}
        //else if (Mathf.Abs(playerY) < Mathf.Abs(playerX) && playerX < 0) {
        //    anim.SetBool("facingUp", false);
        //    anim.SetBool("facingRight", false);
        //    anim.SetBool("facingLeft", true);
        //    anim.SetBool("facingDown", false);
        //}

    }

    IEnumerator EnemyAttack() {
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(.5f);
        atk.SetActive(true);
        yield return new WaitForSeconds(.2f);
        atk.SetActive(false);
        yield return new WaitForSeconds(.8f);
        if (playerDist > minAtkDist + 2) {
            atkCount = 0;
            yield return new WaitForSeconds(.5f);
            state = EnemyState.Moving;
        } else {
            rb.AddForce(new Vector2(directionToPlayer.x, directionToPlayer.y) * -enemySpeed * 50);
            yield return new WaitForSeconds(.5f);
            atkCount = 0;
            state = EnemyState.Moving;
        }
    }

}

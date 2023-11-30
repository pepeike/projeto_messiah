using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public enum BossState {
        Idle,
        Moving,
        Attacking,
        Damaged,
        Charge,
        ChargeWindup,
        RangeAttack
    }

    [SerializeField]
    private int hitPoints = 10;

    [SerializeField]
    private float minAtkDist = 2;

    private int tickTimer = 0;
    private int tickTarget = 10;

    private Animator anim;
    public GameObject chargeBox;

    public GameObject projectile;
    public Transform projectilePos;

    public GameObject Player;
    private Vector3 playerPos;
    //private float difX;
    //private float difY;
    //private Vector2 moveDir;

    private Vector3 directionToTarget;
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private float playerDist;

    private float angle;

    private float playerX;
    private float playerY;

    private Camera cam;
    private SpriteRenderer sprite;

    private LevelManager levelManager;

    //private Transform[] LPoints;
    private Transform target;

    [SerializeField]
    private GameObject atk;
    private SpriteRenderer atkSprite;

    private Animator turretAnim;

    private short atkCount = 0;

    [SerializeField]
    private float enemySpeed;

    private Rigidbody2D rb;
    public Rigidbody2D turret;

    public BossState state;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        state = BossState.Idle;
        cam = GameObject.FindAnyObjectByType<Camera>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        atkSprite = atk.GetComponent<SpriteRenderer>();
        turretAnim = turret.GetComponentInChildren<Animator>();
        chargeBox.SetActive(false);

        localScale = transform.localScale;

        levelManager = FindAnyObjectByType<LevelManager>();

    }

    private void Start() {

        //int _randFloat = Random.Range(0, 6);

        tickTimer = 0;

        atk.SetActive(false);

        //target = levelManager.lPoints[_randFloat];

        //Debug.Log(_randFloat);

    }

    private void FixedUpdate() {

        if (hitPoints <= 0) { Destroy(gameObject); }

        //RotateEnemy();

        //directionToTarget = (target.position - transform.position).normalized;

        if (Player != null) {
            directionToPlayer = (Player.transform.position - transform.position).normalized;
        }
       

        turret.gameObject.transform.position = transform.position;

        //Debug.Log(directionToTarget);
        //Debug.Log(playerDist);


        if (Player != null) {


            playerDist = Vector2.Distance(transform.position, playerPos);


        }

        switch (state) {
            case BossState.Idle:
                //StartCoroutine(Idle());
                if (Player != null) {
                    RotateEnemy();
                    sprite.color = Color.yellow;
                    anim.SetFloat("lookX", directionToPlayer.x);
                    anim.SetFloat("lookY", directionToPlayer.y);
                    anim.SetBool("isMoving", false);
                    rb.velocity = Vector3.zero;
                }

                break;
            case BossState.Moving:
                if (Player != null || target != null) {
                    sprite.color = Color.cyan;
                    RotateEnemy();
                    Move();
                    anim.SetFloat("lookX", directionToPlayer.x);
                    anim.SetFloat("lookY", directionToPlayer.y);
                    anim.SetBool("isMoving", true);

                } else {
                    rb.velocity = Vector3.zero;
                }

                //StartCoroutine(Move());
                break;
            case BossState.Attacking:
                if (atkCount == 0) {
                    atkCount++;
                    anim.SetBool("isAttacking", true);
                    sprite.color = Color.red;
                    //directionToPlayer = (target.position - transform.position).normalized;
                    StartCoroutine(EnemyAttack());
                }
                break;
            case BossState.Damaged:
                sprite.color = Color.magenta;
                anim.SetBool("isMoving", false);

                break;
            case BossState.Charge:
                if (rb.velocity.x <= 5) {
                    chargeBox.SetActive(false);
                    rb.velocity = Vector3.zero;
                    state = BossState.Idle;
                }
                break;

        }

    }

    void PassTick() {

        if (tickTimer == 4) {
            tickTimer++;
            state = BossState.RangeAttack;
            anim.SetTrigger("shoot");
        }

        if (tickTimer == tickTarget) {
            tickTimer = 0;
            
            state = BossState.ChargeWindup;
            StartCoroutine(ChargeAttack());
        }

        if (state != BossState.ChargeWindup) {
            if (state == BossState.Idle) {
                if (playerDist <= minAtkDist) {
                    tickTimer++;

                    state = BossState.Attacking;
                    //Debug.Log("Attacking");
                } else {
                    tickTimer++;

                    state = BossState.Moving;
                    //Debug.Log("Moving");
                }
            } else if (state == BossState.Moving) {
                if (playerDist <= minAtkDist) {
                    tickTimer++;

                    state = BossState.Attacking;
                    // Debug.Log("Attacking");
                } else {
                    tickTimer++;

                    state = BossState.Idle;
                    //Debug.Log("Idle");
                }
            } else if (state == BossState.Damaged) {
                if (playerDist < minAtkDist) {
                    tickTimer++;

                    state = BossState.Attacking;
                    // Debug.Log("Attacking");
                } else {
                    tickTimer++;

                    state = BossState.Idle;
                    //Debug.Log("Idle");
                }
            } else if (state == BossState.RangeAttack) {
                state = BossState.Moving;
            }

        }
        

    }

    void Move() {
        //rb.AddForce(moveDir * enemySpeed);
        if (Player != null) {

            rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * enemySpeed;
        }

        for (int i = levelManager.enemies.Count - 1; i >= 0; i--) {
            if (levelManager.enemies[i] != null) {
                if (Vector2.Distance(transform.position, levelManager.enemies[i].transform.position) <= 3) {
                    Vector2 dirToEnemy = (levelManager.enemies[i].transform.position - transform.position).normalized;
                    rb.velocity += -dirToEnemy * enemySpeed;
                }
            }

        }

    }

    void PlayAtk() {
        turretAnim.SetTrigger("atk");
    }



    public void Damage(int dmg) {
        if (hitPoints > 0) {
            StopCoroutine(EnemyAttack());
            rb.velocity = Vector2.zero;
            sprite.color = Color.magenta;
            state = BossState.Damaged;
            StartCoroutine(Knockback());
            hitPoints -= dmg;
        }
    }

    void Fire() {
        Instantiate(projectile, projectilePos.transform.position, turret.transform.rotation, projectilePos);
    }

    private void RotateEnemy() {

        if (Player != null) {
            playerPos = Player.transform.position;
        }


        //Vector3 pp = cam.ScreenToWorldPoint(playerPos);

        float AngleRad = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x);

        angle = (180 / Mathf.PI) * AngleRad;
        if (angle > 90) {

        }

        playerX = playerPos.x - transform.position.x;
        playerY = playerPos.y - transform.position.y;

        turret.rotation = angle;

        //Debug.Log(playerPos);



    }

    IEnumerator EnemyAttack() {

        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(.2f);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(.3f);
        atk.SetActive(true);
        yield return new WaitForSeconds(.2f);
        atk.SetActive(false);
        yield return new WaitForSeconds(.3f + levelManager.tickPeriod / 2);
        if (playerDist > minAtkDist + 1) {
            atkCount = 0;
            yield return new WaitForSeconds(levelManager.tickPeriod / 2);
            state = BossState.Moving;
        } else {
            rb.AddForce(new Vector2(-directionToPlayer.x, -directionToPlayer.y) * enemySpeed * 2, ForceMode2D.Impulse);
            yield return new WaitForSeconds(levelManager.tickPeriod / 2);
            atkCount = 0;
            state = BossState.Moving;
        }
    }

    void Charge(Vector2 dir) {
        chargeBox.SetActive(true);
        rb.AddForce(dir * 10, ForceMode2D.Impulse);

    }

    IEnumerator ChargeAttack() {
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(-directionToPlayer.x, -directionToPlayer.y) * enemySpeed * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.4f);
        sprite.color = Color.yellow;
        yield return new WaitForSeconds(.3f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(.2f);
        Vector2 directionToEnemy = new Vector2(directionToPlayer.x, directionToPlayer.y);
        //Debug.Log(directionToEnemy);
        sprite.color = Color.yellow;
        yield return new WaitForSeconds(.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(.2f);
        chargeBox.SetActive(true);
        rb.velocity = directionToEnemy * enemySpeed * 8;
        //Debug.Log(directionToEnemy * enemySpeed * 10);
        yield return new WaitForSeconds(1.5f);
        state = BossState.Charge;
        rb.velocity = Vector3.zero;


    }

    IEnumerator Knockback() {
        //rb.velocity = Vector3.zero;
        //rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-directionToPlayer.x, -directionToPlayer.y) * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.2f);
        state = BossState.Attacking;
    }
}

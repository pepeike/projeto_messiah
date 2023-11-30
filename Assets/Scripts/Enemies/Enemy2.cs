using System.Collections;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

    public enum EnemyState {
        Idle,
        Moving,
        Attacking,
        Damaged,
    }

    [SerializeField]
    private int hitPoints = 10;

    public AudioSource[] guardSFX;

    [SerializeField]
    private float minAtkDist = 2;

    private Animator anim;

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

    public EnemyState state;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        state = EnemyState.Idle;
        cam = GameObject.FindAnyObjectByType<Camera>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        atkSprite = atk.GetComponent<SpriteRenderer>();
        turretAnim = turret.GetComponentInChildren<Animator>();
        

        localScale = transform.localScale;

        levelManager = FindAnyObjectByType<LevelManager>();

    }

    private void Start() {

        int _randFloat = Random.Range(0, 6);

        atk.SetActive(false);

        target = levelManager.lPoints[_randFloat];

        //Debug.Log(_randFloat);

    }

    private void FixedUpdate() {

        if (hitPoints <= 0) { Destroy(gameObject); }

        //RotateEnemy();

        if (target != null) {
            directionToTarget = (target.position - transform.position).normalized;


            directionToPlayer = (Player.transform.position - transform.position).normalized;
        }

        turret.position = transform.position;

        //Debug.Log(directionToTarget);
        //Debug.Log(playerDist);


        if (Player != null) {
            

            playerDist = Vector2.Distance(transform.position, playerPos);


        }

        switch (state) {
            case EnemyState.Idle:
                //StartCoroutine(Idle());
                if (Player != null) {
                    RotateEnemy();
                    anim.SetFloat("lookX", directionToPlayer.x);
                    anim.SetFloat("lookY", directionToPlayer.y);
                    anim.SetBool("isMoving", false);
                    rb.velocity = Vector3.zero;
                }

                break;
            case EnemyState.Moving:
                if (Player != null || target != null) {

                    
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
            case EnemyState.Attacking:
                if (atkCount == 0) {
                    atkCount++;
                    //directionToPlayer = (target.position - transform.position).normalized;
                    StartCoroutine(EnemyAttack());
                }
                break;
            case EnemyState.Damaged:
                anim.SetBool("isMoving", false);

                break;

        }

    }

    void PassTick() {

        if (state == EnemyState.Idle) {
            if (playerDist <= minAtkDist) {
                sprite.color = Color.red;
                state = EnemyState.Attacking;
                //Debug.Log("Attacking");
            } else {
                sprite.color = Color.cyan;
                guardSFX[1].Play();
                state = EnemyState.Moving;
                //Debug.Log("Moving");
            }
        } else if (state == EnemyState.Moving) {
            if (playerDist <= minAtkDist) {
                
                sprite.color = Color.red;
                state = EnemyState.Attacking;
                // Debug.Log("Attacking");
            } else {
                sprite.color = Color.yellow;
                state = EnemyState.Idle;
                //Debug.Log("Idle");
            }
        } else if (state == EnemyState.Damaged) {
            if (playerDist < minAtkDist) {
                sprite.color = Color.red;
                state = EnemyState.Attacking;
                // Debug.Log("Attacking");
            } else {
                sprite.color = Color.yellow;
                state = EnemyState.Idle;
                //Debug.Log("Idle");
            }
        }

    }

    void Move() {
        //rb.AddForce(moveDir * enemySpeed);
        if (Player != null) {

            rb.velocity = new Vector2(directionToTarget.x, directionToTarget.y) * enemySpeed;
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
            state = EnemyState.Damaged;
            StartCoroutine(Knockback());
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
        guardSFX[0].Play();
        atk.SetActive(true);
        yield return new WaitForSeconds(.2f);
        atk.SetActive(false);
        yield return new WaitForSeconds(.3f + levelManager.tickPeriod / 2);
        if (playerDist > minAtkDist + 1) {
            atkCount = 0;
            yield return new WaitForSeconds(levelManager.tickPeriod / 2);
            state = EnemyState.Moving;
        } else {
            rb.AddForce(new Vector2(-directionToPlayer.x, -directionToPlayer.y) * enemySpeed * 2, ForceMode2D.Impulse);
            yield return new WaitForSeconds(levelManager.tickPeriod / 2);
            atkCount = 0;
            state = EnemyState.Moving;
        }
    }

    IEnumerator Knockback() {
        //rb.velocity = Vector3.zero;
        //rb.velocity = Vector2.zero;
        guardSFX[2].Play();
        rb.AddForce(new Vector2(-directionToPlayer.x, -directionToPlayer.y) * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.2f);
        state = EnemyState.Attacking;
    }

}

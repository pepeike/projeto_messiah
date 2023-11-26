using System.Collections;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

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

    //[SerializeField]
    //private GameObject atk;

    //private short atkCount = 0;

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
        sprite = GetComponent<SpriteRenderer>();


        localScale = transform.localScale;

        levelManager = FindAnyObjectByType<LevelManager>();

    }

    private void Start() {

        int _randFloat = Random.Range(0, 6);

        //atk.SetActive(false);

        target = levelManager.lPoints[_randFloat];

        Debug.Log(_randFloat);

    }

    private void FixedUpdate() {

        if (hitPoints <= 0) { hitPoints = 10; }



        //directionToTarget = (target.position - transform.position).normalized;
        //directionToPlayer = (Player.transform.position - transform.position).normalized;


        if (Player != null) {

            playerDist = Vector2.Distance(transform.position, playerPos);

        }

        switch (state) {
            case EnemyState.Idle:
                //StartCoroutine(Idle());
                if (Player != null) {
                    RotateEnemy();
                    rb.velocity = Vector3.zero;
                }

                break;
            case EnemyState.Damaged:
                
                break;

        }

    }

    void PassTick() {

        if (state == EnemyState.Idle) {
            sprite.color = Color.yellow;
        } else if (state == EnemyState.Damaged) {

            sprite.color = Color.yellow;
            state = EnemyState.Idle;
            //Debug.Log("Idle");

        }

    }

    



    public void Damage(int dmg) {
        if (hitPoints > 0) {
            //StopAllCoroutines();
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

        playerX = playerPos.x - transform.position.x;
        playerY = playerPos.y - transform.position.y;

        rb.rotation = angle;

        //Debug.Log(playerPos);



    }

    

    IEnumerator Knockback() {
        //rb.velocity = Vector3.zero;
        //rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-directionToPlayer.x, -directionToPlayer.y) * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.2f);
    }



}

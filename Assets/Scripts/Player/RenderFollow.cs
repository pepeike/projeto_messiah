using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using static PlayerMain;

public class RenderFollow : MonoBehaviour {

    #region Variables

    private GameObject core;

    private Vector2 movementInput;

    public Animator anim;

    private SpriteRenderer sprite;

    [SerializeField] private float speed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeForceDrop;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeTimer;
    [SerializeField] private float recoverTime;
    [SerializeField] private int flickerAmnt;

    [SerializeField] private BoxCollider2D hurtbox;

    public PlayerState playerState;

    private PlayerMain playerMain;

    private Rigidbody2D rb;

    private ContactFilter2D movementFilter;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public float collisionOffset;

    private int dodgeCount = 0;

    #endregion

    private void Awake() {
        core = GameObject.Find("Player");
        playerMain = GetComponentInChildren<PlayerMain>();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerState = PlayerState.Normal;
        //Debug.Log(core.name);

    }

    private void Update() {

        //playerState = playerMain.playerState;

        Debug.Log(playerState.ToString());

        switch (playerState) {
            case PlayerState.Normal:
                //RotatePlayer();
                break;
            case PlayerState.Dodging:
                Dodge();
                break;
        }

    }

    private void FixedUpdate() {
        //transform.position = core.transform.position;
        switch (playerState) {
            case PlayerState.Normal:
                anim.SetBool("isDodging", false);
                FixedMove();

                break;

            case PlayerState.Dodging:

                anim.SetBool("isDodging", true);

                FixedDodge();

                break;
            case PlayerState.Attacking:
                break;
            case PlayerState.Wounded:

                break;
        }
    }

    void FixedMove() {
        if (movementInput != Vector2.zero) {
            bool success = TryMove(movementInput);

            anim.SetFloat("velocity", 5);

            if (!success) {
                success = TryMove(new Vector2(movementInput.x, 0)); //se o player estiver encostado numa parede ele ainda pode deslizar verticalmente

                if (!success) {
                    success = TryMove(new Vector2(0, movementInput.y)); //mesma coisa mas horizontalmente
                }
            }

        }

        if (movementInput == Vector2.zero) {
            anim.SetFloat("velocity", 0);
        }
    }

    void Dodge() {
        dodgeSpeed -= dodgeSpeed * dodgeForceDrop * Time.deltaTime;

        float dodgeSpeedMinimum = 6f;
        if (dodgeSpeed < dodgeSpeedMinimum) {
            playerState = PlayerState.Normal;
            rb.velocity = Vector2.zero;
            //DisableInvincibility();
        }
    }

    void FixedDodge() {
        if (movementInput != Vector2.zero) {
            rb.velocity = movementInput * dodgeSpeed;

            bool success = TryDodge(movementInput);

            if (!success) {
                rb.velocity = Vector2.zero;
                playerState = PlayerState.Normal;
            }
        }
    }

    private bool TryMove(Vector2 direction) { //metodo pra detectar colisoes
        int count = rb.Cast(
                direction, //direcao do input
                movementFilter,
                castCollisions,
                speed * Time.fixedDeltaTime + collisionOffset); //distancia dos raycasts 

        if (count == 0) { //se o player n estiver encostado em nada ele pode se mover
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            return true;
        } else {
            return false;
        }
    }

    void OnMove(InputValue inputValue) { //detecta o input do jogador (metodo derivado do InputSystem)
        movementInput = inputValue.Get<Vector2>();
        anim.SetFloat("velocity", 5);
    }

    void OnDodge(InputValue inputValue) {
        if (dodgeCount == 0 && playerState != PlayerState.Attacking) {
            //EnableInvincibility();
            dodgeCount++;
            playerState = PlayerState.Dodging;
            dodgeSpeed = dodgeForce;
            StartCoroutine(DodgeTimer());
        }


    }

    private bool TryDodge(Vector2 direction) {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            dodgeSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count > 0) {
            return false;
        } else {
            return true;
        }

    }

    IEnumerator DodgeTimer() {
        if (dodgeCount != 0) {
            yield return new WaitForSeconds(dodgeTimer);
            dodgeCount = 0;
        }
    }

    private void EnableInvincibility() {
        hurtbox.enabled = false;
    }

    private void DisableInvincibility() {
        hurtbox.enabled = true;
    }

    IEnumerator Recover(float timer) {
        EnableInvincibility();
        yield return new WaitForSeconds(timer);
        playerState = PlayerState.Normal;
        playerMain.playerState = PlayerState.Normal;
        DisableInvincibility();
        
    }

    IEnumerator DamageFlicker(int flickerAmnt) {
        for (int i = flickerAmnt; i > 0; i--) {
            yield return new WaitForSeconds(.1f);
            sprite.color = Color.red;
            yield return new WaitForSeconds(.1f);
            sprite.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy Hitbox")) {
            Vector2 _dirAttacker = collision.transform.position - transform.position;
            rb.AddForce(-_dirAttacker * 4, ForceMode2D.Impulse);
            playerState = PlayerState.Wounded;
            //playerMain.Fuck();
            StartCoroutine(Recover(recoverTime));
            StartCoroutine(DamageFlicker(flickerAmnt));
        }
    }

}

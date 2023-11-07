using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour {

    //Script principal de controle para o player
    public enum PlayerState {
        Normal,
        Dodging,
        BouttaSprint,
        Sprinting,
        SprintingStop,
        Wounded,
        Attacking,
    }

    [SerializeField]
    private float speed; //velocidade de movimento
    [SerializeField]
    private float dodgeForce;
    [SerializeField]
    private float dodgeForceDrop;
    private int dodgeCount = 0;
    private float dodgeSpeed;
    private bool isSprinting = false;
    [SerializeField]
    private float dodgeTimer;
    [SerializeField]
    private float sprintMultiplier;
    [SerializeField]
    private float sprintWindupTimer;
    [SerializeField]
    private float sprintWinddownTimer;
    public int attackPhase = 0;

    public Animator anim;

    private Vector2 movementInput; //input do jogador

    public ContactFilter2D movementFilter; //filtro pra detectar oq colide com o player

    public float collisionOffset; //offset da colisao com o player

    public List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); //raycasts q detectam colisoes

    private Rigidbody2D rb; //rigidbody do player

    public PlayerState playerState;

    private PlayerAttacks atk;

    
    
    void Awake() //chamado antes d void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = PlayerState.Normal;

        

        

    }

    private void Update() {
        switch (playerState) {
            case PlayerState.Normal:
                break;
            case PlayerState.Dodging:
                Dodge();
                break;
            case PlayerState.BouttaSprint:
                break;
            case PlayerState.Sprinting:
                break;

        }
    }



    private void FixedUpdate() { //Fixed Update pra movimentacao n depender da taxa d quadros do jogo

        switch (playerState) {
            case PlayerState.Normal:
                anim.SetBool("isDodging", false);
                FixedMove();

                break;

            case PlayerState.Dodging:

                anim.SetBool("isDodging", true);

                FixedDodge();

                break;
            case PlayerState.BouttaSprint:
                anim.SetBool("isSprinting", true);
                break;
            case PlayerState.Sprinting:
                FixedSprint();
                break;
            case PlayerState.SprintingStop:
                break;
            case PlayerState.Attacking:
                break;
        }

        //Debug.Log(playerState);

    }

    #region MOVEMENT
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

    void Dodge() {
        dodgeSpeed -= dodgeSpeed * dodgeForceDrop * Time.deltaTime;

        float dodgeSpeedMinimum = 6f;
        if (dodgeSpeed < dodgeSpeedMinimum) {
            playerState = PlayerState.Normal;
            rb.velocity = Vector2.zero;
        }
    }

    void FixedMove() {
        if (movementInput != Vector2.zero) {
            bool success = TryMove(movementInput);

            anim.SetFloat("velocity", speed);

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

    void FixedSprint() {
        if (movementInput != Vector2.zero) {
            bool success = TrySprint(movementInput);

            anim.SetFloat("velocity", speed);

            if (!success) {
                success = TrySprint(new Vector2(movementInput.x, 0)); //se o player estiver encostado numa parede ele ainda pode deslizar verticalmente

                if (!success) {
                    success = TrySprint(new Vector2(0, movementInput.y)); //mesma coisa mas horizontalmente
                }
            }

        }

        if (movementInput == Vector2.zero) {
            anim.SetFloat("velocity", 0);
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

    private bool TrySprint(Vector2 direction) {
        int count = rb.Cast(
                direction, //direcao do input
                movementFilter,
                castCollisions,
                speed * Time.fixedDeltaTime + collisionOffset); //distancia dos raycasts 

        if (count == 0) { //se o player n estiver encostado em nada ele pode se mover
            rb.MovePosition(rb.position + direction * speed * sprintMultiplier * Time.fixedDeltaTime);
            return true;
        } else {
            return false;
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



    void OnMove(InputValue inputValue) { //detecta o input do jogador (metodo derivado do InputSystem)
        movementInput = inputValue.Get<Vector2>();
    }

    void OnDodge(InputValue inputValue) {
        if (dodgeCount == 0 && !isSprinting && playerState != PlayerState.Attacking) {
            dodgeCount++;
            playerState = PlayerState.Dodging;
            dodgeSpeed = dodgeForce;
            StartCoroutine(DodgeTimer());
        }


    }

    void OnSprint(InputValue inputValue) {
        if (!isSprinting) {

            playerState = PlayerState.BouttaSprint;
            StartCoroutine(SprintTimer());
            isSprinting = true;
        } else if (isSprinting) {
            playerState = PlayerState.SprintingStop;
            anim.SetBool("isSprinting", false);
            StartCoroutine(WindDown());

        }

    }


    IEnumerator DodgeTimer() {
        if (dodgeCount != 0) {
            yield return new WaitForSeconds(dodgeTimer);
            dodgeCount = 0;
        }
    }

    IEnumerator SprintTimer() {
        yield return new WaitForSeconds(sprintWindupTimer);
        playerState = PlayerState.Sprinting;
    }

    IEnumerator WindDown() {
        yield return new WaitForSeconds(sprintWinddownTimer);
        playerState = PlayerState.Normal;
        isSprinting = false;
    }

    #endregion

    #region ACTIONS

    //void OnFire0() {
    //    playerState = PlayerState.Attacking;
    //    switch (attackPhase) {
    //        case 0:
    //            StartCoroutine(atk.Attack00());
    //            break;
    //        case 1:
    //            StopCoroutine(atk.Attack00());
    //            StartCoroutine(atk.Attack01());
    //            break;
    //        case 2:
    //            StopCoroutine(atk.Attack01());
    //            StartCoroutine(atk.Attack02());
    //            break;

    //    }
        
    //}



    //IEnumerator Attack00() {
    //    state = State.Attacking;
    //    yield return new WaitForSeconds(.2f);
    //    attackHitboxes[0].SetActive(true);
    //    attackPhase++;
    //    yield return new WaitForSeconds(.5f);
    //    attack.SetActive(false);
    //    state = State.Normal;
    //}

    //IEnumerator Attack01() {

    //}

    #endregion

}

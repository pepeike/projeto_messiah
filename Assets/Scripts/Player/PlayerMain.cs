using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour {

    //Script principal de controle para o player
    private enum State {
        Normal,
        Dodging,
    }

    [SerializeField]
    private float speed; //velocidade de movimento
    [SerializeField]
    private float dodgeForce;
    [SerializeField]
    private float dodgeForceDrop;
    private int dodgeCount = 0;
    private float dodgeSpeed;

    private Vector2 movementInput; //input do jogador

    public ContactFilter2D movementFilter; //filtro pra detectar oq colide com o player

    public float collisionOffset; //offset da colisao com o player

    public List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); //raycasts q detectam colisoes

    private Rigidbody2D rb; //rigidbody do player

    private State state;


    void Awake() //chamado antes d void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;


    }

    private void Update() {
        switch (state) {
            case State.Normal:
                break;
            case State.Dodging:
                dodgeSpeed -= dodgeSpeed * dodgeForceDrop * Time.deltaTime;
                
                float dodgeSpeedMinimum = 6f;
                if (dodgeSpeed < dodgeSpeedMinimum ) {
                    state = State.Normal;
                    rb.velocity = Vector2.zero;
                }
                break;
        }
    }

    private void FixedUpdate() { //Fixed Update pra movimentacao n depender da taxa d quadros do jogo

        switch (state) {
            case State.Normal:
                if (movementInput != Vector2.zero) {
                    bool success = TryMove(movementInput);

                    if (!success) {
                        success = TryMove(new Vector2(movementInput.x, 0)); //se o player estiver encostado numa parede ele ainda pode deslizar verticalmente

                        if (!success) {
                            success = TryMove(new Vector2(0, movementInput.y)); //mesma coisa mas horizontalmente
                        }
                    }

                }

                if (movementInput != Vector2.zero) {
                    bool success = TryMove(movementInput);

                    if (!success) {
                        success = TryMove(new Vector2(movementInput.x, 0)); //se o player estiver encostado numa parede ele ainda pode deslizar verticalmente

                        if (!success) {
                            success = TryMove(new Vector2(0, movementInput.y)); //mesma coisa mas horizontalmente
                        }
                    }

                }

                break;

            case State.Dodging:

                
                

                if (movementInput != Vector2.zero) {
                    rb.velocity = movementInput * dodgeSpeed;

                    bool success = TryDodge(movementInput);

                    if (!success) {
                        rb.velocity = Vector2.zero;
                        state = State.Normal;
                    }

                }
                

                





                break;
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
        if (dodgeCount == 0) {
            dodgeCount++;
            state = State.Dodging;
            dodgeSpeed = dodgeForce;
            StartCoroutine(DodgeTimer());
        }


    }



    IEnumerator DodgeTimer() {
        if (dodgeCount != 0) {
            yield return new WaitForSeconds(2);
            dodgeCount = 0;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{

    //Script principal de controle para o player


    [SerializeField]
    private float speed; //velocidade de movimento

    private Vector2 movementInput; //input do jogador

    public ContactFilter2D movementFilter; //filtro pra detectar oq colide com o player

    public float collisionOffset; //offset da colisao com o player

    public List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); //raycasts q detectam colisoes

    private Rigidbody2D rb; //rigidbody do player
    

    void Awake() //chamado antes d void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
        
    }

    

    private void FixedUpdate() { //Fixed Update pra movimentacao n depender da taxa d quadros do jogo
        
        if (movementInput != Vector2.zero) {
            bool success = TryMove(movementInput);

            if (!success) {
                success = TryMove(new Vector2(movementInput.x, 0)); //se o player estiver encostado numa parede ele ainda pode deslizar verticalmente

                if (!success) {
                    success = TryMove(new Vector2(0, movementInput.y)); //mesma coisa mas horizontalmente
                }
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
    }

}

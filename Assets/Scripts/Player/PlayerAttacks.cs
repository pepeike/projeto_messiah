using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlayerMain;


public class PlayerAttacks : MonoBehaviour {

    PlayerMain player;
    [SerializeField]
    public GameObject[] attacks;
    private int attackPhase = 0;

    private Rigidbody2D rb;

    private Camera cam;

    [SerializeField] private Vector3 attackArea00;
    //[SerializeField] private Vector3 attackArea01;
    //[SerializeField] private Vector3 attackArea02;




    public LayerMask enemies;
    public ContactFilter2D enemyFilter;
    //public ContactPoint2D[] playerHits;

    private void Awake() {

        player = GetComponent<PlayerMain>();
        rb = GetComponent<Rigidbody2D>();


        foreach (GameObject attack in attacks) {

            attack.SetActive(false);

        }

    }

    private void FixedUpdate() {

        

    }

    #region ATTACKS

    public IEnumerator Attack00() {
        player.playerState = PlayerState.Attacking;
        attackPhase++;

        yield return new WaitForSeconds(.2f);
        attacks[0].SetActive(true);
        
        yield return new WaitForSeconds(.4f);
        attacks[0].SetActive(false);
        player.RotatePlayer();

        yield return new WaitForSeconds(.2f);
        player.playerState = PlayerMain.PlayerState.Normal;
        attackPhase = 0;
    }

    public IEnumerator Attack01() {
        
        player.playerState = PlayerState.Attacking;
        attacks[0].SetActive(false);
        player.RotatePlayer();
        attackPhase++;

        yield return new WaitForSeconds(.1f);
        rb.AddForce(new Vector2(player.mousePos.x - transform.position.x, player.mousePos.y - transform.position.y).normalized * 4, ForceMode2D.Impulse);
        attacks[1].SetActive(true);

        yield return new WaitForSeconds(.5f);
        attacks[1].SetActive(false);
        player.RotatePlayer();
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(.3f);
        player.playerState = PlayerMain.PlayerState.Normal;
        attackPhase = 0;
    }

    public IEnumerator Attack02() {
        player.playerState = PlayerState.Attacking;
        attacks[1].SetActive(false);
        player.RotatePlayer();
        attackPhase++;

        yield return new WaitForSeconds(.3f);
        rb.AddForce(new Vector2(player.mousePos.x - transform.position.x, player.mousePos.y - transform.position.y).normalized * 4, ForceMode2D.Impulse);
        attacks[2].SetActive(true);

        yield return new WaitForSeconds(.6f);
        player.RotatePlayer();
        attacks[2].SetActive(false);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(.5f);
        player.playerState = PlayerMain.PlayerState.Normal;
        attackPhase = 0;
    }

    #endregion


    

    

    void OnFire0() {
        //Debug.Log("Click");
        if (player.playerState != PlayerState.Sprinting) {

            

            if (attackPhase == 0) {
                StartCoroutine(Attack00());
                
                
            } else if (attackPhase == 1) {
                //StopCoroutine(Attack00());
                StopAllCoroutines();
                StartCoroutine(Attack01());
            } else if (attackPhase == 2) {
            //StopCoroutine(Attack01());
            StopAllCoroutines();
            StartCoroutine(Attack02());
        }

    }


        if (attackPhase > 3) { attackPhase = 0; }



    }

}

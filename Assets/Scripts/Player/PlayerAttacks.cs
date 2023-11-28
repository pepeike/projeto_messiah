using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlayerMain;


public class PlayerAttacks : MonoBehaviour {

    public RenderFollow player;
    public PlayerMain playerMain;
    [SerializeField]
    public GameObject[] attacks;
    private int attackPhase = 0;

    [SerializeField] private float atkTimer;
    private bool canAtk;

    [SerializeField]private Animator anim;
    [SerializeField]private Animator animAtk;

    private Rigidbody2D rb;

    //private Camera cam;

    //[SerializeField] private Vector3 attackArea00;
    //[SerializeField] private Vector3 attackArea01;
    //[SerializeField] private Vector3 attackArea02;

    


    public LayerMask enemies;
    public ContactFilter2D enemyFilter;
    //public ContactPoint2D[] playerHits;

    private void Awake() {

        
        rb = GetComponent<Rigidbody2D>();

        canAtk = true;

        foreach (GameObject attack in attacks) {

            attack.SetActive(false);

        }

    }

    void Atk() {
        //canAtk = false;
        animAtk.SetTrigger("atk");
        attacks[0].SetActive(true);
    }

    void AtkEnd() {
        attacks[0].SetActive(false);
        StartCoroutine(atkCooldown(atkTimer));
    }


    #region ATTACKS

    public IEnumerator Attack00() {
        player.playerState = PlayerState.Attacking;
        playerMain.playerState = PlayerState.Attacking;
        attackPhase++;
        //anim.SetTrigger("Attacking1");

        yield return new WaitForSeconds(.3f);
        attacks[0].SetActive(true);
        
        yield return new WaitForSeconds(.4f);
        attacks[0].SetActive(false);
        playerMain.RotatePlayer();

        yield return new WaitForSeconds(.2f);
        player.playerState = PlayerState.Normal;
        playerMain.playerState = PlayerState.Normal;
        attackPhase = 0;
    }

    public IEnumerator Attack01() {
        player.playerState = PlayerState.Attacking;
        playerMain.playerState = PlayerState.Attacking;
        attacks[0].SetActive(false);
        playerMain.RotatePlayer();
        attackPhase++;

        yield return new WaitForSeconds(.1f);
        rb.AddForce(new Vector2(playerMain.mousePos.x - transform.position.x, playerMain.mousePos.y - transform.position.y).normalized * 4, ForceMode2D.Impulse);
        attacks[1].SetActive(true);

        yield return new WaitForSeconds(.5f);
        attacks[1].SetActive(false);
        playerMain.RotatePlayer();
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(.3f);
        player.playerState = PlayerState.Normal;
        playerMain.playerState = PlayerState.Normal;
        attackPhase = 0;
    }

    public IEnumerator Attack02() {
        player.playerState = PlayerState.Attacking;
        playerMain.playerState = PlayerState.Attacking;
        attacks[1].SetActive(false);
        playerMain.RotatePlayer();
        attackPhase++;

        yield return new WaitForSeconds(.3f);
        rb.AddForce(new Vector2(playerMain.mousePos.x - transform.position.x, playerMain.mousePos.y - transform.position.y).normalized * 4, ForceMode2D.Impulse);
        attacks[2].SetActive(true);

        yield return new WaitForSeconds(.6f);
        playerMain.RotatePlayer();
        attacks[2].SetActive(false);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(.5f);
        player.playerState = PlayerState.Normal;
        playerMain.playerState = PlayerState.Normal;
        attackPhase = 0;
    }

    #endregion


    

    

    void OnFire0() {
        //Debug.Log("Click");
        if (playerMain.playerState != PlayerState.Dodging && canAtk) {

            canAtk = false;
            

            //if (attackPhase == 0) {
                anim.SetTrigger("Attacking1");
                //StartCoroutine(Attack00());
                
                
            //} 
        //    else if (attackPhase == 1) {
        //        //StopCoroutine(Attack00());
        //        StopAllCoroutines();
        //        StartCoroutine(Attack01());
        //    } else if (attackPhase == 2) {
        //    //StopCoroutine(Attack01());
        //    StopAllCoroutines();
        //    StartCoroutine(Attack02());
        //}

    }


        if (attackPhase > 1) { attackPhase = 0; }



    }

    IEnumerator atkCooldown(float atkTimer) {
        yield return new WaitForSeconds(atkTimer);
        canAtk = true;
    }


}
